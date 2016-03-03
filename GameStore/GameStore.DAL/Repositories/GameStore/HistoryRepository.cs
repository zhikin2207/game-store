using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore
{
	public class OperationHistoryRepository : GenericRepository<OperationHistory, int>, IOperationHistoryRepository
	{
		public OperationHistoryRepository(BaseDataContext context)
			: base(context)
		{
		}
	}
}