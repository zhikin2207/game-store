using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Filtering;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.BLL.Services
{
	public class GameService : IGameService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public GameService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public int Count()
		{
			return _unitOfWork.GameRepository.Count();
		}

		public int Count(IEnumerable<IFilterBase> filters)
		{
			if (filters == null)
			{
				throw new ArgumentNullException("filters");
			}

			FilterExecutor<Game> filterPipeLine = RegisterFilterExecutor(filters);
			return _unitOfWork.GameRepository.Count(filterPipeLine.GetConditions());
		}

		public bool IsExist(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			bool isExist = _unitOfWork.GameRepository.IsExist(key);
			return isExist;
		}

		public void Create(GameDto gameDto, string companyName, IEnumerable<string> selectedGenreNames, IEnumerable<string> selectedPlatformTypeNames)
		{
			var game = Mapper.Map<Game>(gameDto);

			Publisher publisher;

			try
			{
				publisher = _unitOfWork.PublisherRepository.GetPublisher(companyName);
			}
			catch (InvalidOperationException)
			{
				publisher = null;
			}

			game.Publisher = publisher;

			game.Genres = new List<Genre>();
			AddGenresToGame(game, selectedGenreNames);

			game.PlatformTypes = new List<PlatformType>();
			AddPlatformsToGame(game, selectedPlatformTypeNames);

			_unitOfWork.GameRepository.Add(game);
			_unitOfWork.Save();
		}

		public void Update(GameDto gameDto, string companyName, IEnumerable<string> selectedGenresId, IEnumerable<string> selectedPlatformTypeNames)
		{
			Game game = _unitOfWork.GameRepository.Get(gameDto.Key);
			Publisher publisher;

			try
			{
				publisher = _unitOfWork.PublisherRepository.GetPublisher(companyName);
			}
			catch (InvalidOperationException)
			{
				publisher = null;
			}

			game.Publisher = publisher;
			game.Name = gameDto.Name;
			game.Description = gameDto.Description;
			game.Price = gameDto.Price;
			game.UnitsInStock = gameDto.UnitsInStock;
			game.DatePublished = gameDto.DatePublished;
			game.Discontinued = gameDto.Discontinued;
			game.GameLocalizations = Mapper.Map<ICollection<GameLocalization>>(gameDto.GameLocalizations);

			game.Genres.Clear();
			AddGenresToGame(game, selectedGenresId);

			game.PlatformTypes.Clear();
			AddPlatformsToGame(game, selectedPlatformTypeNames);

			_unitOfWork.GameRepository.Update(game);
			_unitOfWork.Save();
		}

		public void Delete(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException();
			}

			Game game = _unitOfWork.GameRepository.Get(key);
			game.IsDeleted = true;

			_unitOfWork.GameRepository.Update(game);
			_unitOfWork.Save();
		}

		#region Get game related items

		public GameDto GetGame(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException("key");
			}

			Game game = _unitOfWork.GameRepository.Get(key);
			return Mapper.Map<GameDto>(game);
		}

		public PublisherDto GetGamePublisher(string gameKey)
		{
			if (string.IsNullOrWhiteSpace(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			Publisher publisher;
			
			try
			{
				publisher = _unitOfWork.GameRepository.GetGamePublisher(gameKey);
			}
			catch (InvalidOperationException)
			{
				publisher = null;
			}

			return Mapper.Map<PublisherDto>(publisher);
		}

		public IEnumerable<GenreDto> GetGameGenres(string gameKey)
		{
			if (string.IsNullOrWhiteSpace(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			IEnumerable<Genre> genres = _unitOfWork.GameRepository.GetGameGenres(gameKey);
			return Mapper.Map<IEnumerable<GenreDto>>(genres);
		}

		public IEnumerable<PlatformTypeDto> GetGamePlatformTypes(string gameKey)
		{
			if (string.IsNullOrWhiteSpace(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			IEnumerable<PlatformType> paltformTypes = _unitOfWork.GameRepository.GetGamePlatformTypes(gameKey);
			return Mapper.Map<IEnumerable<PlatformTypeDto>>(paltformTypes);
		}

		#endregion

		#region Get all items

		public IEnumerable<PublisherDto> GetAllPublishers()
		{
			IEnumerable<Publisher> publishers = _unitOfWork.PublisherRepository.GetList();
			return Mapper.Map<IEnumerable<PublisherDto>>(publishers);
		}

		public IEnumerable<GenreDto> GetAllGenres()
		{
			IEnumerable<Genre> genres = _unitOfWork.GenreRepository.GetList();
			return Mapper.Map<IEnumerable<GenreDto>>(genres);
		}

		public IEnumerable<PlatformTypeDto> GetAllPlatformTypes()
		{
			IEnumerable<PlatformType> platformTypes = _unitOfWork.PlatformTypeRepository.GetList();
			return Mapper.Map<IEnumerable<PlatformTypeDto>>(platformTypes);
		}

		#endregion

		#region Get list of items

		public IEnumerable<GameDto> GetGames(IEnumerable<IFilterBase> filters, ISortBase sorting)
		{
			if (filters == null)
			{
				throw new ArgumentNullException("filters");
			}

			if (sorting == null)
			{
				throw new ArgumentNullException("sorting");
			}

			FilterExecutor<Game> filterPipeLine = RegisterFilterExecutor(filters);
			var gameSorting = (ISorting<Game>)sorting;

			IEnumerable<Game> games = _unitOfWork.GameRepository.GetList(
				filterPipeLine.GetConditions(),
				gameSorting.SortCondition,
				gameSorting.IsAscending);

			return Mapper.Map<IEnumerable<GameDto>>(games);
		}

		public IEnumerable<GameDto> GetGames(IEnumerable<IFilterBase> filters, ISortBase sorting, int pageNumber, int itemsPerPage)
		{
			if (filters == null)
			{
				throw new ArgumentNullException("filters");
			}

			if (sorting == null)
			{
				throw new ArgumentNullException("sorting");
			}

			FilterExecutor<Game> filterPipeLine = RegisterFilterExecutor(filters);
			var gameSorting = (ISorting<Game>)sorting;

			IEnumerable<Game> games = _unitOfWork.GameRepository.GetList(
				filterPipeLine.GetConditions(),
				gameSorting.SortCondition,
				gameSorting.IsAscending,
				pageNumber,
				itemsPerPage);

			return Mapper.Map<IEnumerable<GameDto>>(games);
		}

		#endregion

		private void AddGenresToGame(Game game, IEnumerable<string> selectedGenreNames)
		{
			foreach (string genreName in selectedGenreNames)
			{
				Genre genre = _unitOfWork.GenreRepository.GetGenre(genreName);
				game.Genres.Add(genre);
			}
		}

		private void AddPlatformsToGame(Game game, IEnumerable<string> selectedPlatformsId)
		{
			foreach (string platformType in selectedPlatformsId)
			{
				PlatformType platform = _unitOfWork.PlatformTypeRepository.GetPlatformType(platformType);
				game.PlatformTypes.Add(platform);
			}
		}

		private FilterExecutor<Game> RegisterFilterExecutor(IEnumerable<IFilterBase> filters)
		{
			var filterPipeLine = new FilterExecutor<Game>();

			foreach (IFilterBase filter in filters)
			{
				filterPipeLine.Register(filter as IFilter<Game>);
			}

			return filterPipeLine;
		}
	}
}