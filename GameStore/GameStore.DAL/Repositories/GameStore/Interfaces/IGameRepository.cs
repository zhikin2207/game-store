using System.Collections.Generic;
using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IGameRepository : IGenericRepository<Game, string>
	{
		/// <summary>
		/// Get all genres by game key.
		/// </summary>
		/// <param name="key">Game key</param>
		/// <returns>Genres</returns>
		IEnumerable<Genre> GetGameGenres(string key);

		/// <summary>
		/// Get all platform types by game key.
		/// </summary>
		/// <param name="key">Game key</param>
		/// <returns>Platforms</returns>
		IEnumerable<PlatformType> GetGamePlatformTypes(string key);

		/// <summary>
		/// Get publisher by game key.
		/// </summary>
		/// <param name="key">Game key</param>
		/// <returns>Publisher</returns>
		Publisher GetGamePublisher(string key);
	}
}