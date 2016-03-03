using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTOs;
using GameStore.BLL.Filtering.Filters;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.BLL.Sorting.Sortings;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.WebUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.BLL.Services
{
	[TestClass]
	public class GameServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private IGameService _gameService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_gameService = new GameService(_mockUnitOfWork.Object);
		}

		#region Count method

		[TestMethod]
		public void Count_Does_Not_Change_Actual_Number_Of_Items_From_GameRepository()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Count()).Returns(10);

			int value = _gameService.Count();

			Assert.AreEqual(10, value);
		}

		#endregion

		#region Count method (overloaded)

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Count_Throws_NullArgumentException_When_Filters_Are_Null()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Count(It.IsNotNull<IEnumerable<Func<Game, bool>>>())).Returns(10);

			_gameService.Count(null);
		}

		[TestMethod]
		public void Count_With_Filters_Does_Not_Change_Actual_Number_Of_Items_From_GameRepository()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Count(It.IsAny<IEnumerable<Func<Game, bool>>>())).Returns(10);

			int value = _gameService.Count(Enumerable.Empty<IFilterBase>());

			Assert.AreEqual(10, value);
		}

		#endregion

		#region IsExist method

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsExist_Throws_Exception_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameService.IsExist(string.Empty);
		}

		[TestMethod]
		public void IsExist_Returns_True_If_Game_Exists()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.IsExist(It.IsAny<string>())).Returns(true);

			bool result = _gameService.IsExist("game-key");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsExist_Returns_False_If_Game_Exists()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.IsExist(It.IsAny<string>())).Returns(false);

			bool result = _gameService.IsExist("game-key");

			Assert.IsFalse(result);
		}

		#endregion

		#region Create method

		[TestMethod]
		public void CreateGame_Maps_Genres_Appropriately()
		{
			Game gameResult = null;
			_mockUnitOfWork.Setup(m => m.GenreRepository.Get(It.IsAny<int>()));
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>()));
			_mockUnitOfWork.Setup(m => m.GameRepository.Add(It.IsAny<Game>())).Callback<Game>(game =>
			{
				gameResult = game;
			});

			_gameService.Create(new GameDto(), "publisher", new[] { "1", "2", "3" }, Enumerable.Empty<string>());

			Assert.AreEqual(gameResult.Genres.Count, 3);
		}

		[TestMethod]
		public void CreateGame_Maps_Platforms_Appropriately()
		{
			Game gameResult = null;
			_mockUnitOfWork.Setup(m => m.PlatformTypeRepository.Get(It.IsAny<int>()));
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>()));
			_mockUnitOfWork.Setup(m => m.GameRepository.Add(It.IsAny<Game>())).Callback<Game>(game =>
			{
				gameResult = game;
			});

			_gameService.Create(new GameDto(), "publisher", Enumerable.Empty<string>(), new[] { "1", "2", "3" });

			Assert.AreEqual(gameResult.PlatformTypes.Count, 3);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void CreateGame_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>()));

			_mockUnitOfWork.Setup(m => m.GameRepository.Add(It.IsAny<Game>())).Throws<Exception>();

			_gameService.Create(new GameDto(), "publisher", Enumerable.Empty<string>(), Enumerable.Empty<string>());
		}

		[TestMethod]
		public void CreateGame_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Add(It.IsAny<Game>()));
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>()));

			_gameService.Create(new GameDto(), "publisher", Enumerable.Empty<string>(), Enumerable.Empty<string>());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		public void Create_Sets_Null_Into_Publisher_When_It_Does_Not_Exist()
		{
			Game gameResult = null;
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Throws(new InvalidOperationException());
			_mockUnitOfWork.Setup(m => m.GameRepository.Add(It.IsAny<Game>())).Callback<Game>(game =>
			{
				gameResult = game;
			});

			_gameService.Create(new GameDto(), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>());

			Assert.IsNull(gameResult.Publisher);
		}

		#endregion

		#region Update method

		[TestMethod]
		public void UpdateGame_Maps_Genres_Appropriately()
		{
			Game gameResult = null;
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.GenreRepository.Get(It.IsAny<int>()));
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game
			{
				Genres = new List<Genre>(),
				PlatformTypes = new List<PlatformType>()
			});
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>())).Callback<Game>(game =>
			{
				gameResult = game;
			});

			_gameService.Update(new GameDto(), "publisher", new[] { "1", "2", "3" }, Enumerable.Empty<string>());

			Assert.AreEqual(gameResult.Genres.Count, 3);
		}

		[TestMethod]
		public void UpdateGame_Maps_Platforms_Appropriately()
		{
			Game gameResult = null;
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.PlatformTypeRepository.Get(It.IsAny<int>()));
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game
			{
				Genres = new List<Genre>(),
				PlatformTypes = new List<PlatformType>()
			});
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>())).Callback<Game>(game =>
			{
				gameResult = game;
			});

			_gameService.Update(new GameDto(), "publisher", Enumerable.Empty<string>(), new[] { "1", "2", "3" });

			Assert.AreEqual(gameResult.PlatformTypes.Count, 3);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void UpdateGame_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>())).Throws<Exception>();
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game
			{
				Genres = new List<Genre>(),
				PlatformTypes = new List<PlatformType>()
			});

			_gameService.Update(new GameDto(), "publisher", Enumerable.Empty<string>(), Enumerable.Empty<string>());
		}

		[TestMethod]
		public void UpdateGame_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>()));
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game
			{
				Genres = new List<Genre>(),
				PlatformTypes = new List<PlatformType>()
			});

			_gameService.Update(new GameDto(), "publisher", Enumerable.Empty<string>(), Enumerable.Empty<string>());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		public void Update_Sets_Null_Into_Publisher_When_It_Does_Not_Exist()
		{
			Game gameResult = null;
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game
			{
				Genres = new List<Genre>(),
				PlatformTypes = new List<PlatformType>()
			});
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Throws(new InvalidOperationException());
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>())).Callback<Game>(game =>
			{
				gameResult = game;
			});

			_gameService.Update(new GameDto(), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>());

			Assert.IsNull(gameResult.Publisher);
		}

		#endregion

		#region Delete method

		[TestMethod]
		public void Delete_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game());
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>()));

			_gameService.Delete("game-key");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Delete_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game());
			_mockUnitOfWork.Setup(m => m.GameRepository.Update(It.IsAny<Game>())).Throws<Exception>();

			_gameService.Delete("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Delete_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameService.Delete(string.Empty);
		}

		#endregion

		#region GetGame method

		[TestMethod]
		public void GetGame_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game());

			GameDto game = _gameService.GetGame("game-key");

			Assert.IsNotNull(game);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetGame_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Throws<Exception>();

			_gameService.GetGame("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGame_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameService.GetGame(string.Empty);
		}

		[TestMethod]
		public void GetGame_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game
			{
				Key = "game-key",
				Name = "game-name",
				Description = "game-desc",
				UnitsInStock = 1
			});

			GameDto game = _gameService.GetGame("game-key");

			Assert.AreEqual("game-key", game.Key);
			Assert.AreEqual("game-name", game.Name);
			Assert.AreEqual("game-desc", game.Description);
			Assert.AreEqual(1, game.UnitsInStock);
		}

		#endregion

		#region GetGamePublisher method

		[TestMethod]
		public void GetGamePublisher_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGamePublisher(It.IsAny<string>())).Returns(new Publisher());

			PublisherDto publisher = _gameService.GetGamePublisher("game-key");

			Assert.IsNotNull(publisher);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetGamePublisher_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGamePublisher(It.IsAny<string>())).Throws<Exception>();

			_gameService.GetGamePublisher("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGamePublisher_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameService.GetGamePublisher(string.Empty);
		}

		[TestMethod]
		public void GetGamePublisher_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGamePublisher(It.IsAny<string>())).Returns(new Publisher
			{
				CompanyName = "publisher-company-name",
				Description = "publisher-desc",
				HomePage = "publisher-home-page",
			});

			PublisherDto publisher = _gameService.GetGamePublisher("game-key");

			Assert.AreEqual("publisher-company-name", publisher.CompanyName);
			Assert.AreEqual("publisher-desc", publisher.Description);
			Assert.AreEqual("publisher-home-page", publisher.HomePage);
		}

		#endregion

		#region GetGameGenres method

		[TestMethod]
		public void GetGameGenres_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGameGenres(It.IsAny<string>()))
				.Returns(Enumerable.Empty<Genre>());

			IEnumerable<GenreDto> genres = _gameService.GetGameGenres("game-key");

			Assert.IsNotNull(genres);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetGameGenres_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGameGenres(It.IsAny<string>())).Throws<Exception>();

			_gameService.GetGameGenres("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGameGenres_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameService.GetGameGenres(string.Empty);
		}

		[TestMethod]
		public void GetGameGenres_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGameGenres(It.IsAny<string>())).Returns(new[]
			{
				new Genre { Name = "genre-name-0" },
				new Genre { Name = "genre-name-1" }
			});

			IEnumerable<GenreDto> genres = _gameService.GetGameGenres("game-key");

			Assert.AreEqual(2, genres.Count());
			Assert.AreEqual("genre-name-0", genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", genres.ElementAt(1).Name);
		}

		#endregion

		#region GetGamePlatformTypes method

		[TestMethod]
		public void GetGamePlatformTypes_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGamePlatformTypes(It.IsAny<string>()))
				.Returns(Enumerable.Empty<PlatformType>());

			IEnumerable<PlatformTypeDto> platforms = _gameService.GetGamePlatformTypes("game-key");

			Assert.IsNotNull(platforms);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetGamePlatformTypes_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGamePlatformTypes(It.IsAny<string>())).Throws<Exception>();

			_gameService.GetGamePlatformTypes("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGamePlatformTypes_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameService.GetGamePlatformTypes(string.Empty);
		}

		[TestMethod]
		public void GetGamePlatformTypes_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.GameRepository.GetGamePlatformTypes(It.IsAny<string>())).Returns(new[]
			{
				new PlatformType { Type = "platform-name-0" },
				new PlatformType { Type = "platform-name-1" }
			});

			IEnumerable<PlatformTypeDto> platforms = _gameService.GetGamePlatformTypes("game-key");

			Assert.AreEqual(2, platforms.Count());
			Assert.AreEqual("platform-name-0", platforms.ElementAt(0).Type);
			Assert.AreEqual("platform-name-1", platforms.ElementAt(1).Type);
		}

		#endregion

		#region GetAllPublishers method

		[TestMethod]
		public void GetAllPublishers_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetList()).Returns(new Publisher[] { });

			IEnumerable<PublisherDto> publishers = _gameService.GetAllPublishers();

			Assert.IsNotNull(publishers);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetAllPublishers_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetList()).Throws<Exception>();

			_gameService.GetAllPublishers();
		}

		[TestMethod]
		public void GetAllPublishers_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetList()).Returns(new[]
			{
				new Publisher { CompanyName = "publisher-name-0" },
				new Publisher { CompanyName = "publisher-name-1" }
			});

			IEnumerable<PublisherDto> publishers = _gameService.GetAllPublishers();

			Assert.AreEqual(2, publishers.Count());
			Assert.AreEqual("publisher-name-0", publishers.ElementAt(0).CompanyName);
			Assert.AreEqual("publisher-name-1", publishers.ElementAt(1).CompanyName);
		}

		#endregion

		#region GetAllGenres method

		[TestMethod]
		public void GetAllGenres_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.GetList()).Returns(new Genre[] { });

			IEnumerable<GenreDto> genres = _gameService.GetAllGenres();

			Assert.IsNotNull(genres);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetAllGenres_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.GetList()).Throws<Exception>();

			_gameService.GetAllGenres();
		}

		[TestMethod]
		public void GetAllGenres_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.GetList()).Returns(new[]
			{
				new Genre { Name = "genre-name-0" },
				new Genre { Name = "genre-name-1" }
			});

			IEnumerable<GenreDto> genres = _gameService.GetAllGenres();

			Assert.AreEqual(2, genres.Count());
			Assert.AreEqual("genre-name-0", genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", genres.ElementAt(1).Name);
		}

		#endregion

		#region GetAllPlatformTypes method

		[TestMethod]
		public void GetAllPlatformTypes_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.PlatformTypeRepository.GetList()).Returns(new PlatformType[] { });

			IEnumerable<PlatformTypeDto> platformTypes = _gameService.GetAllPlatformTypes();

			Assert.IsNotNull(platformTypes);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetAllPlatformTypes_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.PlatformTypeRepository.GetList()).Throws<Exception>();

			_gameService.GetAllPlatformTypes();
		}

		[TestMethod]
		public void GetAllPlatformTypes_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.PlatformTypeRepository.GetList()).Returns(new[]
			{
				new PlatformType { Type = "platform-type-0" },
				new PlatformType { Type = "platform-type-1" }
			});

			IEnumerable<PlatformTypeDto> platformTypes = _gameService.GetAllPlatformTypes();

			Assert.AreEqual(2, platformTypes.Count());
			Assert.AreEqual("platform-type-0", platformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", platformTypes.ElementAt(1).Type);
		}

		#endregion

		#region GetGames acc. to filters method (overloaded)

		[TestMethod]
		public void GetGames_With_Filters_Does_Not_Return_Null()
		{
			SetupShortGetListInGameRepository();
			var sortingStub = new GameByPriceSorting(false);

			IEnumerable<GameDto> games = _gameService.GetGames(Enumerable.Empty<IFilterBase>(), sortingStub);

			Assert.IsNotNull(games);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGames_With_Filters_Throws_ArgumentNullException_When_Sorting_Is_Null()
		{
			SetupShortGetListInGameRepository();

			_gameService.GetGames(Enumerable.Empty<IFilterBase>(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGames_With_Filters_Throws_ArgumentNullException_When_Filters_Are_Null()
		{
			SetupShortGetListInGameRepository();
			var sortingStub = new GameByPriceSorting(false);

			_gameService.GetGames(null, sortingStub);
		}

		[TestMethod]
		public void GetGames_With_Filters_Maps_Games_Appropriately()
		{
			SetupShortGetListInGameRepository();
			var sortingStub = new GameByPriceSorting(false);

			var filters = new IFilterBase[]
			{
				new GamesByPriceFilter(),
				new GamesByDateFilter()
			};

			IEnumerable<GameDto> games = _gameService.GetGames(filters, sortingStub);

			Assert.IsNotNull(games);
			Assert.AreEqual("game-key-0", games.ElementAt(0).Key);
			Assert.AreEqual("game-key-1", games.ElementAt(1).Key);
		}

		#endregion

		#region GetGames with paging method (overloaded)

		[TestMethod]
		public void GetGames_With_Paging_Does_Not_Return_Null()
		{
			SetupFullGetListInGameRepository();
			var sortingStub = new GameByPriceSorting(false);

			IEnumerable<GameDto> games = _gameService.GetGames(Enumerable.Empty<IFilterBase>(), sortingStub, 0, 0);

			Assert.IsNotNull(games);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGames_With_Paging_Throws_ArgumentNullException_When_Sorting_Is_Null()
		{
			SetupFullGetListInGameRepository();

			_gameService.GetGames(Enumerable.Empty<IFilterBase>(), null, 0, 0);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGames_With_Paging_Throws_ArgumentNullException_When_Filters_Are_Null()
		{
			SetupFullGetListInGameRepository();
			var sortingStub = new GameByPriceSorting(false);

			_gameService.GetGames(null, sortingStub, 0, 0);
		}

		[TestMethod]
		public void GetGames_With_Paging_Maps_Games_Appropriately()
		{
			SetupFullGetListInGameRepository();
			var sortingStub = new GameByPriceSorting(false);

			var filters = new IFilterBase[]
			{
				new GamesByPriceFilter(),
				new GamesByDateFilter()
			};

			IEnumerable<GameDto> games = _gameService.GetGames(filters, sortingStub, 0, 0);

			Assert.IsNotNull(games);
			Assert.AreEqual("game-key-0", games.ElementAt(0).Key);
			Assert.AreEqual("game-key-1", games.ElementAt(1).Key);
		}

		#endregion

		#region Test helpers

		private void SetupShortGetListInGameRepository()
		{
			_mockUnitOfWork
				.Setup(m => m.GameRepository.GetList(
					It.IsAny<IEnumerable<Func<Game, bool>>>(),
					It.IsAny<Func<Game, object>>(),
					It.IsAny<bool>()))
				.Returns(new[]
				{
					new Game { Key = "game-key-0" },
					new Game { Key = "game-key-1" }
				});
		}

		private void SetupFullGetListInGameRepository()
		{
			_mockUnitOfWork
				.Setup(m => m.GameRepository.GetList(
					It.IsAny<IEnumerable<Func<Game, bool>>>(),
					It.IsAny<Func<Game, object>>(),
					It.IsAny<bool>(),
					It.IsAny<int>(),
					It.IsAny<int>()))
				.Returns(new[]
				{
					new Game { Key = "game-key-0" },
					new Game { Key = "game-key-1" }
				});
		}

		#endregion
	}
}