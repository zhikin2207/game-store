using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Comment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class CommentControllerTest
	{
		private Mock<ICommentService> _mockCommentService;
		private Mock<ILoggingService> _mockLogger;
		private Mock<ICommentViewModelBuilder> _mockViewModelBuilder;
		private Mock<IOperationHistoryService> _mockOperationHistoryService;

		private CommentController _commentController;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockCommentService = new Mock<ICommentService>();
			_mockLogger = new Mock<ILoggingService>();
			_mockViewModelBuilder = new Mock<ICommentViewModelBuilder>();
			_mockOperationHistoryService = new Mock<IOperationHistoryService>();

			_commentController = new CommentController(
				_mockCommentService.Object,
				_mockOperationHistoryService.Object,
				_mockViewModelBuilder.Object,
				_mockLogger.Object)
			{
				ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Guest).Get()).Object
			};
		}

		#region Comments action

		[TestMethod]
		public void Comments_Action_Returns_PartialViewResult_If_Request_Is_Ajax()
		{
			var mockHttpContext = new Mock<HttpContextBase>();
			var mockControllerContext = new ControllerContext
			{
				HttpContext = mockHttpContext.Object
			};

			_commentController.ControllerContext = mockControllerContext;

			mockHttpContext.Setup(c => c.Request["X-Requested-With"]).Returns("XMLHttpRequest");
			_mockViewModelBuilder.Setup(b => b.BuildGameCommentsViewModel(It.IsAny<string>()))
				.Returns(new GameCommentsViewModel
				{
					Comments = new List<CommentViewModel>()
				});

			ActionResult result = _commentController.Comments("game-key");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(PartialViewResult));
		}

		[TestMethod]
		public void Comments_Action_Returns_ViewResult_If_Request_Is_Not_Ajax()
		{
			var mockHttpContext = new Mock<HttpContextBase>();
			var mockControllerContext = new ControllerContext
			{
				HttpContext = mockHttpContext.Object
			};

			_commentController.ControllerContext = mockControllerContext;

			mockHttpContext.Setup(c => c.Request["X-Requested-With"]);
			_mockViewModelBuilder.Setup(b => b.BuildGameCommentsViewModel(It.IsAny<string>()))
				.Returns(new GameCommentsViewModel
				{
					Comments = new List<CommentViewModel>()
				});

			ActionResult result = _commentController.Comments("game-key");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Comments_Action_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockViewModelBuilder.Setup(m => m.BuildGameCommentsViewModel(It.IsAny<string>()))
				.Throws<InvalidOperationException>();

			_commentController.Comments("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Comments_Action_Does_Not_Catch_Exception_When_GameKey_Is_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildGameCommentsViewModel(It.IsAny<string>())).Throws<ArgumentNullException>();

			_commentController.Comments(null);
		}

		#endregion

		#region Create action

		[TestMethod]
		public void Create_Action_Maps_Comment_Appropriately()
		{
			var gameKey = "game-key";
			var comment = new CommentViewModel
			{
				CommentId = 1,
				Body = "comment-body",
				Name = "comment-name"
			};

			CommentDto commentDto = null;

			_mockCommentService
				.Setup(s => s.Create(It.IsAny<CommentDto>()))
				.Callback<CommentDto>(item => commentDto = item);

			_commentController.Create(gameKey, comment);

			Assert.AreEqual(gameKey, commentDto.GameKey);
			Assert.AreEqual(comment.CommentId, commentDto.CommentId);
			Assert.AreEqual(comment.Body, commentDto.Body);
			Assert.AreEqual(comment.Name, commentDto.Name);
		}

		[TestMethod]
		public void Create_Action_Writes_User_Guid_When_User_Not_Guest()
		{
			CommentDto commentDto = null;

			_mockCommentService
				.Setup(s => s.Create(It.IsAny<CommentDto>()))
				.Callback<CommentDto>(item => commentDto = item);

			_commentController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Manager).Get()).Object;

			_commentController.Create("game-key", new CommentViewModel());

			Assert.AreEqual(_commentController.CurrentUserGuid, commentDto.UserGuid);
		}

		[TestMethod]
		public void Create_Action_Does_Not_Write_User_Guid_When_User_Guest()
		{
			CommentDto commentDto = null;

			_mockCommentService
				.Setup(s => s.Create(It.IsAny<CommentDto>()))
				.Callback<CommentDto>(item => commentDto = item);

			_commentController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Guest).Get()).Object;

			_commentController.Create("game-key", new CommentViewModel());

			Assert.AreEqual(null, commentDto.UserGuid);
		}

		[TestMethod]
		public void Create_Action_Redirects_To_Comments_Action_After_Successfull_Creating()
		{
			var result = _commentController.Create("game-key", new CommentViewModel()) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Comments", result.RouteValues["action"].ToString());
			Assert.AreEqual("game-key", result.RouteValues["gameKey"].ToString());
		}

		[TestMethod]
		public void Create_Action_Writes_Comment_Statistics()
		{
			_commentController.Create("game-key", new CommentViewModel());

			_mockOperationHistoryService.Verify(s => s.WriteGameCommentStatistics(It.IsAny<string>()), Times.AtLeastOnce());
		}

		#endregion

		#region Delete action

		[TestMethod]
		public void Delete_Action_Redirects_To_Comments_Action_After_Successfull_Deleting()
		{
			var result = _commentController.Delete(5, "game-key") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("comments", result.RouteValues["action"].ToString().ToLower());
			Assert.AreEqual("game-key", result.RouteValues["gameKey"].ToString().ToLower());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Delete_Action_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockCommentService.Setup(s => s.Delete(It.IsAny<int>())).Throws<InvalidOperationException>();

			_commentController.Delete(5, "game-key");
		}

		#endregion
	}
}