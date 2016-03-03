using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IRoleRepository : IGenericRepository<Role, int>
	{
		Role GetRole(string name);
	}
}