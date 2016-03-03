using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTOs;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class GameControllerTest
	{
		private Mock<IGameService> _mockGameService;
		private Mock<ILoggingService> _mockLogger;
		private Mock<IGameViewModelBuilder> _mockViewModelBuilder;
		private Mock<IOperationHistoryService> _mockOperationHistoryService;

		private GameController _gameController;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockGameService = new Mock<IGameService>();
			_mockLogger = new Mock<ILoggingService>();
			_mockViewModelBuilder = new Mock<IGameViewModelBuilder>();
			_mockOperationHistoryService = new Mock<IOperationHistoryService>();

			_gameController = new GameController(
				_mockGameService.Object,
				_mockOperationHistoryService.Object,
				_mockViewModelBuilder.Object,
				_mockLogger.Object)
			{
				ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Guest).Get()).Object
			};
		}

		#region Games action

		[TestMethod]
		public void Games_Does_Not_Return_Null()
		{
			_mockGameService.Setup(s => s.GetGames(
				It.IsAny<IEnumerable<IFilterBase>>(), 
				It.IsAny<ISortBase>(),
				It.IsAny<int>(), 
				It.IsAny<int>())).Returns(new List<GameDto>());

			var result = _gameController.Games(new AllGamesViewModel()) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Games_Hides_Deleted_Games_When_User_Gues_Or_User()
		{
			var allGamesViewModel = new AllGamesViewModel();

			_mockGameService.Setup(s => s.GetGames(
				It.IsAny<IEnumerable<IFilterBase>>(),
				It.IsAny<ISortBase>(),
				It.IsAny<int>(),
				It.IsAny<int>())).Returns(new List<GameDto>());

			_gameController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Guest).Get()).Object;

			_gameController.Games(allGamesViewModel);

			Assert.IsTrue(allGamesViewModel.ExistanceFilter.HideDeletedGames);
		}

		[TestMethod]
		public void Games_Does_Not_Hide_Deleted_Games_When_User_Not_Gues_And_Not_User()
		{
			var allGamesViewModel = new AllGamesViewModel();

			_mockGameService.Setup(s => s.GetGames(
				It.IsAny<IEnumerable<IFilterBase>>(),
				It.IsAny<ISortBase>(),
				It.IsAny<int>(),
				It.IsAny<int>())).Returns(new List<GameDto>());

			_gameController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Manager).Get()).Object;

			_gameController.Games(allGamesViewModel);

			Assert.IsFalse(allGamesViewModel.ExistanceFilter.HideDeletedGames);
		}

		#endregion

		#region Details action

		[TestMethod]
		public void Details_Does_Not_Return_Null()
		{
			_mockGameService.Setup(s => s.GetGame(It.IsAny<string>())).Returns(new GameDto());

			ActionResult result = _gameController.Details("game-key");

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Details_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockViewModelBuilder.Setup(m => m.BuildGameDetailsViewModel(It.IsAny<string>())).Throws<InvalidOperationException>();

			_gameController.Details("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Details_Does_Not_Catch_Exception_When_GameKey_Is_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildGameDetailsViewModel(It.IsAny<string>())).Throws<ArgumentNullException>();

			_gameController.Details(null);
		}

		[TestMethod]
		public void Details_Writes_View_Statistics()
		{
			_gameController.Details("game-key");

			_mockOperationHistoryService.Verify(s => s.WriteGameViewStatistics(It.IsAny<string>()), Times.AtLeastOnce());
		}

		#endregion

		#region Create action

		[TestMethod]
		public void Create_By_Get_Method_Does_Not_Return_Null()
		{
			var result = _gameController.Create() as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Create_By_Post_Method_Returns_ViewResult_If_Model_Has_Errors()
		{
			_gameController.ModelState.AddModelError(string.Empty, string.Empty);

			ActionResult result = _gameController.Create(new CreateEditGameViewModel());

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public void Create_By_Post_Method_Maps_Game_Appropriately()
		{
			var game = new GameViewModel
			{
				Key = "game-key",
				Name = "game-name",
				Description = "game-description",
				Discontinued = true,
				Price = 100.0M,
				PublisherId = 1,
				UnitsInStock = 10
			};

			GameDto gameDto = null;
			_mockGameService
				.Setup(s => s.Create(It.IsAny<GameDto>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
				.Callback<GameDto, string, IEnumerable<string>, IEnumerable<string>>((item, l0, l1, l2) => gameDto = item);

			_gameController.Create(new CreateEditGameViewModel { Game = game });

			Assert.AreEqual(game.Key, gameDto.Key);
			Assert.AreEqual(game.Name, gameDto.Name);
			Assert.AreEqual(game.Description, gameDto.Description);
			Assert.AreEqual(game.Discontinued, gameDto.Discontinued);
			Assert.AreEqual(game.Price, gameDto.Price);
			Assert.AreEqual(game.PublisherId, gameDto.PublisherId);
			Assert.AreEqual(game.UnitsInStock, gameDto.UnitsInStock);
		}

		[TestMethod]
		public void Create_By_Post_Method_Redirects_To_Games_Action_After_Successfull_Creating()
		{
			var result = _gameController.Create(new CreateEditGameViewModel()) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("AllGames", result.RouteValues["action"].ToString());
		}

		[TestMethod]
		public void Create_By_Post_Method_Writes_Adding_Statistics()
		{
			_gameController.Create(new CreateEditGameViewModel());

			_mockOperationHistoryService.Verify(s => s.WriteGameAddStatistics(It.IsAny<string>()), Times.AtLeastOnce());
		}

		//[TestMethod]
		//public void Create_By_Post_Method_Adds_Module_Error_When_Game_With_Specified_Key_Exists()
		//{
		//	var createGameViewModel = new CreateEditGameViewModel();

		//	_mockGameService.Setup(m => m.IsExist(It.IsAny<string>())).Returns(true);

		//	_gameController.Create(createGameViewModel);

		//	Assert.IsTrue(_gameController.ModelState.Keys.Contains("Game.Key"));
		//}

		#endregion

		#region Update action

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Update_By_Get_Method_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockViewModelBuilder.Setup(m => m.BuildEditGameViewModel(It.IsAny<string>())).Throws<InvalidOperationException>();

			_gameController.Update("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Update_By_Get_Method_Does_Not_Catch_Exception_When_GameKey_Is_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildEditGameViewModel(It.IsAny<string>())).Throws<ArgumentNullException>();

			_gameController.Update((string)null);
		}

		[TestMethod]
		public void Update_By_Post_Method_Returns_ViewResult_If_Model_Has_Errors()
		{
			_gameController.ModelState.AddModelError(string.Empty, string.Empty);

			var result = _gameController.Update(new CreateEditGameViewModel()) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Update_By_Post_Method_Maps_Game_Appropriately()
		{
			var game = new GameViewModel
			{
				Key = "game-key",
				Name = "game-name",
				Description = "game-description",
				Discontinued = true,
				Price = 100.0M,
				PublisherId = 1,
				UnitsInStock = 10
			};

			GameDto gameDto = null;
			_mockGameService
				.Setup(s => s.Update(It.IsAny<GameDto>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
				.Callback<GameDto, string, IEnumerable<string>, IEnumerable<string>>((item, l0, l1, l2) => gameDto = item);

			_gameController.Update(new CreateEditGameViewModel { Game = game });

			Assert.AreEqual(game.Key, gameDto.Key);
			Assert.AreEqual(game.Name, gameDto.Name);
			Assert.AreEqual(game.Description, gameDto.Description);
			Assert.AreEqual(game.Discontinued, gameDto.Discontinued);
			Assert.AreEqual(game.Price, gameDto.Price);
			Assert.AreEqual(game.PublisherId, gameDto.PublisherId);
			Assert.AreEqual(game.UnitsInStock, gameDto.UnitsInStock);
		}

		[TestMethod]
		public void Update_By_Post_Method_Redirects_To_Games_Action_After_Successfull_Updating()
		{
			var game = new GameViewModel { Key = "game-key" };
			var result = _gameController.Update(new CreateEditGameViewModel { Game = game }) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("AllGames", result.RouteValues["action"].ToString());
		}

		[TestMethod]
		public void Update_By_Get_Method_Redirects_To_AllGames_When_User_Not_Manager()
		{
			_mockViewModelBuilder.Setup(m => m.BuildEditGameViewModel(It.IsAny<string>())).Returns(new CreateEditGameViewModel());

			var result = _gameController.Update("game") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("AllGames", result.RouteValues["action"]);
		}

		[TestMethod]
		public void Update_By_Get_Method_Redirects_To_AllGames_When_User_Not_Specific_Publisher()
		{
			_mockViewModelBuilder.Setup(m => m.BuildEditGameViewModel(It.IsAny<string>())).Returns(new CreateEditGameViewModel());
			_gameController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Publisher).SetPublisher("publisher").Get()).Object;

			var result = _gameController.Update("game") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("AllGames", result.RouteValues["action"]);
		}

		[TestMethod]
		public void Update_By_Get_Method_Returns_View_When_User_Manager()
		{
			_gameController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Manager).Get()).Object;

			var result = _gameController.Update("game") as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Update_By_Get_Method_Returns_View_When_User_Specific_Publisher()
		{
			_mockViewModelBuilder
				.Setup(m => m.BuildEditGameViewModel(It.IsAny<string>()))
				.Returns(new CreateEditGameViewModel { SelectedPublisherCompanyName = "publisher" });

			_gameController.ControllerContext = ControllerContextMockBuilder.GetContextMock(
				AuthMockBuilder
					.Build(UserRole.Publisher)
					.SetPublisher("publisher")
					.Get()).Object;

			var result = _gameController.Update("game") as ViewResult;

			Assert.IsNotNull(result);
		}

		#endregion

		#region Delete action

		[TestMethod]
		public void Delete_Does_Not_Return_Null()
		{
			var result = _gameController.Delete("game-key") as RedirectToRouteResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Delete_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockGameService.Setup(s => s.Delete(It.IsAny<string>())).Throws<InvalidOperationException>();

			_gameController.Delete("game-key");
		}

		[TestMethod]
		public void Delete_Redirects_To_Games_Action_After_Successfull_Deleting()
		{
			var result = _gameController.Delete("game-key") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("AllGames", result.RouteValues["action"].ToString());
		}

		#endregion

		#region GamesNumber action

		[TestMethod]
		public void GamesNumber_Action_Does_Not_Return_Null()
		{
			ContentResult result = _gameController.GamesNumber();

			Assert.IsNotNull(result);
		}

		#endregion
	}
}