using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore
{
	public class GameRepository : GenericRepository<Game, string>, IGameRepository
	{
		public GameRepository(BaseDataContext context)
			: base(context)
		{
		}

		public virtual IEnumerable<Genre> GetGameGenres(string key)
		{
			return Get(key).Genres.ToList();
		}

		public virtual IEnumerable<PlatformType> GetGamePlatformTypes(string key)
		{
			return Get(key).PlatformTypes.ToList();
		}

		public virtual Publisher GetGamePublisher(string key)
		{
			return Get(key).Publisher;
		}
	}
}