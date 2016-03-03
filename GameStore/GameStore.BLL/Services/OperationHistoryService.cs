using System;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.BLL.Services
{
	public class OperationHistoryService : IOperationHistoryService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public OperationHistoryService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public void WriteGameViewStatistics(string gameKey)
		{
			WriteGameStatistics(gameKey, OperationType.View);
		}

		public void WriteGameAddStatistics(string gameKey)
		{
			WriteGameStatistics(gameKey, OperationType.Add);
		}

		public void WriteGameCommentStatistics(string gameKey)
		{
			WriteGameStatistics(gameKey, OperationType.Comment);
		}

		private void WriteGameStatistics(string gameKey, OperationType type)
		{
			var gameHistory = new GameHistory
			{
				GameKey = gameKey,
				Type = type,
				Date = DateTime.UtcNow,
				Game = _unitOfWork.GameRepository.Get(gameKey)
			};

			_unitOfWork.OperationHistoryRepository.Add(gameHistory);
			_unitOfWork.Save();
		}
	}
}