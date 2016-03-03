using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IGenreRepository : IGenericRepository<Genre, int>
	{
		Genre GetGenre(string genreName);

		bool IsExist(string name);
	}
}