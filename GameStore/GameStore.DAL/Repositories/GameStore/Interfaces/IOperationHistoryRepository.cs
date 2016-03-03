using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IOperationHistoryRepository : IGenericRepository<OperationHistory, int>
	{
	}
}