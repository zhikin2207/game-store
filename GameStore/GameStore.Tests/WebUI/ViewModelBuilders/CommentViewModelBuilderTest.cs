using System;
using System.Collections.ObjectModel;
using System.Linq;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.Components;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.ViewModelBuilders;
using GameStore.WebUI.ViewModels.Comment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.ViewModelBuilders
{
	[TestClass]
	public class CommentViewModelBuilderTest
	{
		private Mock<IGameService> _mockGameService;
		private Mock<ICommentService> _mockCommentService;
		private CommentViewModelBuilder _commentViewModelBuilder;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockGameService = new Mock<IGameService>();
			_mockCommentService = new Mock<ICommentService>();

			_mockCommentService.Setup(m => m.GetComments(It.IsAny<string>())).Returns(new[]
			{
				new CommentDto { CommentId = 0, GameKey = "game-key-0", Name = "comment-name-0" },
				new CommentDto { CommentId = 1, GameKey = "game-key-1", Name = "comment-name-1" }
			});

			_mockGameService.Setup(m => m.GetGame(It.IsAny<string>())).Returns(new GameDto
			{
				Key = "game-key",
				GameLocalizations = new Collection<GameLocalizationDto>
				{
					new GameLocalizationDto { Language = LanguageDto.Uk }
				}
			});

			_commentViewModelBuilder = new CommentViewModelBuilder(_mockCommentService.Object, _mockGameService.Object);
		}

		#region BuildGameCommentsViewModel method

		[TestMethod]
		public void BuildGameCommensViewModel_Does_Not_Return_Null()
		{
			GameCommentsViewModel viewModel = _commentViewModelBuilder.BuildGameCommentsViewModel("game-key");

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildGameCommensViewModel_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_commentViewModelBuilder.BuildGameCommentsViewModel(string.Empty);
		}

		[TestMethod]
		public void BuildGameCommensViewModel_Maps_Game_Appropriately()
		{
			GameCommentsViewModel viewModel = _commentViewModelBuilder.BuildGameCommentsViewModel("game-key");

			Assert.AreEqual("game-key", viewModel.Game.Key);
		}

		[TestMethod]
		public void BuildGameCommensViewModel_Maps_Comments_Appropriately()
		{
			GameCommentsViewModel viewModel = _commentViewModelBuilder.BuildGameCommentsViewModel("game-key");

			Assert.AreEqual(0, viewModel.Comments.ElementAt(0).CommentId);
			Assert.AreEqual("comment-name-0", viewModel.Comments.ElementAt(0).Name);
			Assert.AreEqual("game-key-0", viewModel.Comments.ElementAt(0).GameKey);

			Assert.AreEqual(1, viewModel.Comments.ElementAt(1).CommentId);
			Assert.AreEqual("comment-name-1", viewModel.Comments.ElementAt(1).Name);
			Assert.AreEqual("game-key-1", viewModel.Comments.ElementAt(1).GameKey);
		}

		#endregion
	}
}