using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.Components;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Game;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class GameViewModelBuilder : IGameViewModelBuilder
	{
		private readonly IGameService _gameService;

		public GameViewModelBuilder(IGameService gameService)
		{
			_gameService = gameService;
		}

		public CreateEditGameViewModel BuildCreateGameViewModel()
		{
			return new CreateEditGameViewModel
			{
				Game = new GameViewModel(),
				Genres = GetAllGenres(),
				PlatformTypes = GetAllPlatformTypes(),
				Publishers = GetAllPublishers(true)
			};
		}

		public CreateEditGameViewModel BuildEditGameViewModel(string gameKey)
		{
			if (string.IsNullOrEmpty(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			IEnumerable<GenreDto> selectedGenres = _gameService.GetGameGenres(gameKey);
			IEnumerable<PlatformTypeDto> selectedPlatformTypes = _gameService.GetGamePlatformTypes(gameKey);

			GameDto gameDto = _gameService.GetGame(gameKey);
			GameLocalizationDto ukrainianGameLocalization = gameDto.GameLocalizations.FirstOrDefault(lg => lg.Language == LanguageDto.Uk);

			return new CreateEditGameViewModel
			{
				Game = Mapper.Map<GameViewModel>(gameDto),
				UkrainianGameLocalization = Mapper.Map<GameLocalizationViewModel>(ukrainianGameLocalization),

				SelectedPublisherCompanyName = GetGamePublisher(gameKey).CompanyName, 

				SelectedGenreNames = selectedGenres.Select(g => g.Name),
				SelectedPlatformTypeNames = selectedPlatformTypes.Select(p => p.Type),

				Genres = GetAllGenres(),
				PlatformTypes = GetAllPlatformTypes(),
				Publishers = GetAllPublishers(true)
			};
		}

		public GameDetailsViewModel BuildGameDetailsViewModel(string gameKey)
		{
			if (string.IsNullOrEmpty(gameKey))
			{
				throw new ArgumentNullException("gameKey");
			}

			return new GameDetailsViewModel
			{
				Game = GetGame(gameKey),
				Genres = GetGameGenres(gameKey),
				PlatformTypes = GetGamePlatformTypes(gameKey),
				Publisher = GetGamePublisher(gameKey)
			};
		}

		public void RebuildAllGamesViewModel(AllGamesViewModel allGamesViewModel)
		{
			if (allGamesViewModel == null)
			{
				throw new ArgumentNullException("allGamesViewModel");
			}

			var filters = Mapper.Map<IEnumerable<IFilterBase>>(allGamesViewModel);

			var sorting = Mapper.Map<ISortBase>(allGamesViewModel.Sorting);

			IEnumerable<GameDto> games;
			if (allGamesViewModel.Paging.ItemsPerPage == 0)
			{
				games = _gameService.GetGames(filters, sorting);
			}
			else
			{
				games = _gameService.GetGames(
					filters, 
					sorting, 
					allGamesViewModel.Paging.CurrentPage,
					allGamesViewModel.Paging.ItemsPerPage);
			}

			allGamesViewModel.Games = Mapper.Map<IEnumerable<GameViewModel>>(games);

			allGamesViewModel.Paging.TotalGamesNumber = _gameService.Count(filters);

			allGamesViewModel.GenreFilter.Genres = GetAllGenres();
			allGamesViewModel.PlatformFilter.PlatformTypes = GetAllPlatformTypes();
			allGamesViewModel.PublisherFilter.Publishers = GetAllPublishers();
		}

		public void RebuildCreateGameViewModel(CreateEditGameViewModel createGameViewModel)
		{
			if (createGameViewModel == null)
			{
				throw new ArgumentNullException("createGameViewModel");
			}

			createGameViewModel.Genres = GetAllGenres();
			createGameViewModel.PlatformTypes = GetAllPlatformTypes();
			createGameViewModel.Publishers = GetAllPublishers(true);
		}

		public void RebuildEditGameViewModel(CreateEditGameViewModel editGameViewModel)
		{
			if (editGameViewModel == null)
			{
				throw new ArgumentNullException("editGameViewModel");
			}

			editGameViewModel.Genres = GetAllGenres();
			editGameViewModel.PlatformTypes = GetAllPlatformTypes();
			editGameViewModel.Publishers = GetAllPublishers(true);
		}

		#region Basic mappings

		private IEnumerable<GenreViewModel> GetAllGenres(bool includeDefault = false)
		{
			IEnumerable<GenreDto> genres = _gameService.GetAllGenres();

			var mappedGenres = Mapper.Map<ICollection<GenreViewModel>>(genres);
			mappedGenres = mappedGenres.OrderBy(g => g.DisplayName).ToList();

			if (includeDefault)
			{
				mappedGenres.Add(GetOtherGenre());
			}

			return mappedGenres;
		}

		private IEnumerable<PlatformTypeViewModel> GetAllPlatformTypes()
		{
			IEnumerable<PlatformTypeDto> platforms = _gameService.GetAllPlatformTypes().OrderBy(p => p.Type);
			return Mapper.Map<IEnumerable<PlatformTypeViewModel>>(platforms);
		}

		private IEnumerable<PublisherViewModel> GetAllPublishers(bool includeDefault = false)
		{
			IEnumerable<PublisherDto> publishers = _gameService.GetAllPublishers().ToList();

			var mappedPublishers = Mapper.Map<IList<PublisherViewModel>>(publishers);
			mappedPublishers = mappedPublishers.OrderBy(p => p.DisplayCompanyName).ToList();

			if (includeDefault)
			{
				mappedPublishers.Insert(0, GetUnknownPublisher());
			}

			return mappedPublishers;
		}

		private GameViewModel GetGame(string key)
		{
			GameDto game = _gameService.GetGame(key);
			return Mapper.Map<GameViewModel>(game);
		}

		private IEnumerable<GenreViewModel> GetGameGenres(string key)
		{
			ICollection<GenreDto> genres = _gameService.GetGameGenres(key).ToList();
			var mappedGenres = Mapper.Map<ICollection<GenreViewModel>>(genres);

			if (!genres.Any())
			{
				mappedGenres.Add(GetOtherGenre());
			}

			return mappedGenres;
		}

		private IEnumerable<PlatformTypeViewModel> GetGamePlatformTypes(string key)
		{
			IEnumerable<PlatformTypeDto> platforms = _gameService.GetGamePlatformTypes(key);
			return Mapper.Map<IEnumerable<PlatformTypeViewModel>>(platforms);
		}

		private PublisherViewModel GetGamePublisher(string key)
		{
			PublisherDto publisher = _gameService.GetGamePublisher(key);

			var mappedPublisher = Mapper.Map<PublisherViewModel>(publisher);

			if (mappedPublisher == null)
			{
				mappedPublisher = GetUnknownPublisher();
			}

			return mappedPublisher;
		}

		private PublisherViewModel GetUnknownPublisher()
		{
			return new PublisherViewModel
			{
				DisplayCompanyName = Resource.GameViewModelBuilder_UnknownPublisher
			};
		}

		private GenreViewModel GetOtherGenre()
		{
			return new GenreViewModel
			{
				DisplayName = Resource.GameViewModelBuilder_OtherGenre
			};
		}

		#endregion
	}
}