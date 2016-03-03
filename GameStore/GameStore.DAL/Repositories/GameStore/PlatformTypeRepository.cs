using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore
{
	public class PlatformTypeRepository : GenericRepository<PlatformType, int>, IPlatformTypeRepository
	{
		public PlatformTypeRepository(BaseDataContext context)
			: base(context)
		{
		}

		public PlatformType GetPlatformType(string type)
		{
			return Get(p => p.Type == type);
		}
	}
}
