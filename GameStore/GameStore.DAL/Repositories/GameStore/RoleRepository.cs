using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore
{
	public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
	{
		public RoleRepository(BaseDataContext context)
			: base(context)
		{
		}

		public Role GetRole(string name)
		{
			return Get(r => r.Name == name);
		}
	}
}