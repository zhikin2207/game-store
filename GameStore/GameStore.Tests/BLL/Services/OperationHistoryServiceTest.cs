using System;
using AutoMapper;
using GameStore.BLL.Mappings;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.BLL.Services
{
	[TestClass]
	public class OperationHistoryServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private IOperationHistoryService _operationHistoryService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingBllProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_operationHistoryService = new OperationHistoryService(_mockUnitOfWork.Object);

			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>()));
		}

		#region WriteGameViewStatistics method

		[TestMethod]
		public void WriteGameViewStatistics_Sets_Correct_Operation_Type()
		{
			OperationHistory testHistoryItem = null;
			_mockUnitOfWork
				.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()))
				.Callback<OperationHistory>(history =>
				{
					testHistoryItem = history;
				});

			_operationHistoryService.WriteGameViewStatistics("game-key");

			Assert.AreEqual(OperationType.View, testHistoryItem.Type);
		}

		[TestMethod]
		public void WriteGameViewStatistics_Creates_GameHistory_Appropriately()
		{
			GameHistory testHistoryItem = null;
			_mockUnitOfWork
				.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()))
				.Callback<OperationHistory>(history =>
				{
					testHistoryItem = history as GameHistory;
				});

			_operationHistoryService.WriteGameViewStatistics("game-key");

			Assert.IsNotNull(testHistoryItem);
			Assert.AreEqual("game-key", testHistoryItem.GameKey);
		}

		[TestMethod]
		public void WriteGameViewStatistics_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()));

			_operationHistoryService.WriteGameViewStatistics("game-key");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void WriteGameViewStatistics_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>())).Throws<Exception>();

			_operationHistoryService.WriteGameViewStatistics("game-key");
		}

		#endregion

		#region WriteGameAddStatistics method

		[TestMethod]
		public void WriteGameAddStatistics_Sets_Correct_Operation_Type()
		{
			OperationHistory testHistoryItem = null;
			_mockUnitOfWork
				.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()))
				.Callback<OperationHistory>(history =>
				{
					testHistoryItem = history;
				});

			_operationHistoryService.WriteGameAddStatistics("game-key");

			Assert.AreEqual(OperationType.Add, testHistoryItem.Type);
		}

		[TestMethod]
		public void WriteGameAddStatistics_Creates_GameHistory_Appropriately()
		{
			GameHistory testHistoryItem = null;
			_mockUnitOfWork
				.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()))
				.Callback<OperationHistory>(history =>
				{
					testHistoryItem = history as GameHistory;
				});

			_operationHistoryService.WriteGameAddStatistics("game-key");

			Assert.IsNotNull(testHistoryItem);
			Assert.AreEqual("game-key", testHistoryItem.GameKey);
		}

		[TestMethod]
		public void WriteGameAddStatistics_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()));

			_operationHistoryService.WriteGameAddStatistics("game-key");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void WriteGameAddStatistics_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>())).Throws<Exception>();

			_operationHistoryService.WriteGameAddStatistics("game-key");
		}

		#endregion

		#region WriteGameCommentStatistics method

		[TestMethod]
		public void WriteGameCommentStatistics_Sets_Correct_Operation_Type()
		{
			OperationHistory testHistoryItem = null;
			_mockUnitOfWork
				.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()))
				.Callback<OperationHistory>(history =>
				{
					testHistoryItem = history;
				});

			_operationHistoryService.WriteGameCommentStatistics("game-key");

			Assert.AreEqual(OperationType.Comment, testHistoryItem.Type);
		}

		[TestMethod]
		public void WriteGameCommentStatistics_Creates_GameHistory_Appropriately()
		{
			GameHistory testHistoryItem = null;
			_mockUnitOfWork
				.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()))
				.Callback<OperationHistory>(history =>
				{
					testHistoryItem = history as GameHistory;
				});

			_operationHistoryService.WriteGameCommentStatistics("game-key");

			Assert.IsNotNull(testHistoryItem);
			Assert.AreEqual("game-key", testHistoryItem.GameKey);
		}

		[TestMethod]
		public void WriteGameCommentStatistics_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>()));

			_operationHistoryService.WriteGameCommentStatistics("game-key");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void WriteGameCommentStatistics_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.OperationHistoryRepository.Add(It.IsAny<OperationHistory>())).Throws<Exception>();

			_operationHistoryService.WriteGameCommentStatistics("game-key");
		}

		#endregion
	}
}