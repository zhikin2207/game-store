using System.Collections.Generic;
using GameStore.BLL.DTOs;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Sorting.Interfaces;

namespace GameStore.BLL.Services.Interfaces
{
	public interface IGameService
	{
		/// <summary>
		/// Get count of games.
		/// </summary>
		/// <returns>Count of entities</returns>
		int Count();

		/// <summary>
		/// Get count of games acc. to filters.
		/// </summary>
		/// <param name="filters">List of filters</param>
		/// <returns>Count of entities</returns>
		int Count(IEnumerable<IFilterBase> filters);

		/// <summary>
		/// Create game with genres and platforms.
		/// </summary>
		/// <param name="gameDto">Game model</param>
		/// <param name="companyName">Publisher's company name</param>
		/// <param name="selectedGenreNames">List with names of selected genres</param>
		/// <param name="selectedPlatformTypeNames">List with types of selected platforms</param>
		void Create(GameDto gameDto, string companyName, IEnumerable<string> selectedGenreNames, IEnumerable<string> selectedPlatformTypeNames);

		/// <summary>
		/// Update game with genres and platforms.
		/// </summary>
		/// <param name="gameDto">Game model</param>
		/// <param name="companyName">Publisher's company name</param>
		/// <param name="selectedGenreNames">List with names of selected genres</param>
		/// <param name="selectedPlatformTypeNames">List with types of selected platforms</param>
		void Update(GameDto gameDto, string companyName, IEnumerable<string> selectedGenreNames, IEnumerable<string> selectedPlatformTypeNames);

		/// <summary>
		/// Delete game by key.
		/// </summary>
		/// <param name="key">Game key</param>
		void Delete(string key);

		#region Get game related items

		/// <summary>
		/// Get game by key.
		/// </summary>
		/// <param name="key">Game key</param>
		/// <returns>Game</returns>
		GameDto GetGame(string key);

		/// <summary>
		/// Get game publisher by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>Game publisher</returns>
		PublisherDto GetGamePublisher(string gameKey);

		/// <summary>
		/// Get game genres by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>Game genres</returns>
		IEnumerable<GenreDto> GetGameGenres(string gameKey);

		/// <summary>
		/// Get game platforms by game key.
		/// </summary>
		/// <param name="gameKey">Game key</param>
		/// <returns>Game platforms</returns>
		IEnumerable<PlatformTypeDto> GetGamePlatformTypes(string gameKey);

		#endregion

		#region Get all items

		/// <summary>
		/// Get all genres.
		/// </summary>
		/// <returns>Genres</returns>
		IEnumerable<GenreDto> GetAllGenres();

		/// <summary>
		/// Get all publishers.
		/// </summary>
		/// <returns>Publishers</returns>
		IEnumerable<PublisherDto> GetAllPublishers();

		/// <summary>
		/// Get all platforms.
		/// </summary>
		/// <returns>Platforms</returns>
		IEnumerable<PlatformTypeDto> GetAllPlatformTypes();

		#endregion

		#region Get list of items

		/// <summary>
		/// Get sorted list of games acc. to filters.
		/// </summary>
		/// <param name="filters">List of filters</param>
		/// <param name="sorting">Sort info</param>
		/// <returns>Games</returns>
		IEnumerable<GameDto> GetGames(IEnumerable<IFilterBase> filters, ISortBase sorting);

		/// <summary>
		/// Get sorted list of games acc. to filters for current page number.
		/// </summary>
		/// <param name="filtersDto">List of filters</param>
		/// <param name="sorting">Sort info</param>
		/// <param name="pageNumber">Current page number</param>
		/// <param name="itemsPerPage">Items per page</param>
		/// <returns>Games</returns>
		IEnumerable<GameDto> GetGames(IEnumerable<IFilterBase> filtersDto, ISortBase sorting, int pageNumber, int itemsPerPage);

		#endregion

		bool IsExist(string key);
	}
}