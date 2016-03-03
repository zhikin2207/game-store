using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Mappings;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.BLL.Services
{
	[TestClass]
	public class CommentServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private ICommentService _commentService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingBllProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_commentService = new CommentService(_mockUnitOfWork.Object);
		}

		#region Create method

		[TestMethod]
		public void Create_Maps_Comment_Appropriately()
		{
			Comment testComment = null;
			_mockUnitOfWork.Setup(m => m.CommentRepository.Add(It.IsAny<Comment>())).Callback<Comment>(comment =>
			{
				testComment = comment;
			});

			_commentService.Create(new CommentDto
			{
				GameKey = "game-key",
				Name = "comment-name",
				Body = "comment-body"
			});

			Assert.AreEqual("game-key", testComment.GameKey);
			Assert.AreEqual("comment-name", testComment.Name);
			Assert.AreEqual("comment-body", testComment.Body);
		}

		[TestMethod]
		public void Create_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Add(It.IsAny<Comment>()));

			_commentService.Create(new CommentDto());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Create_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Add(It.IsAny<Comment>())).Throws<Exception>();

			_commentService.Create(new CommentDto());
		}

		#endregion

		#region GetComments method

		[TestMethod]
		public void GetComments_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Comments(It.IsAny<string>()))
				.Returns(Enumerable.Empty<Comment>());

			IEnumerable<CommentDto> comments = _commentService.GetComments("game-key");

			Assert.IsNotNull(comments);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetComments_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Comments(It.IsAny<string>())).Throws<Exception>();

			_commentService.GetComments("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetComments_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_commentService.GetComments(string.Empty);
		}

		[TestMethod]
		public void GetComments_Maps_Game_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Comments(It.IsAny<string>())).Returns(new[]
			{
				new Comment { Name = "comment-name-0" },
				new Comment { Name = "comment-name-1" }
			});

			IEnumerable<CommentDto> comments = _commentService.GetComments("game-key");

			Assert.AreEqual(2, comments.Count());
			Assert.AreEqual("comment-name-0", comments.ElementAt(0).Name);
			Assert.AreEqual("comment-name-1", comments.ElementAt(1).Name);
		}

		#endregion

		#region Delete method

		[TestMethod]
		public void Delete_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Delete(It.IsAny<Comment>()));

			_commentService.Delete(1);

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Delete_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.CommentRepository.Delete(It.IsAny<Comment>())).Throws<Exception>();

			_commentService.Delete(1);
		}

		#endregion
	}
}