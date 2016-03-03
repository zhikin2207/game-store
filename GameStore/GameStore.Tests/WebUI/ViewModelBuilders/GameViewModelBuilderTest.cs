using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.Components;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Filtering.Components;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.BLL.Sorting.Components;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.BLL.Sorting.Sortings;
using GameStore.WebUI;
using GameStore.WebUI.ViewModelBuilders;
using GameStore.WebUI.ViewModels;
using GameStore.WebUI.ViewModels.Filter;
using GameStore.WebUI.ViewModels.Game;
using GameStore.WebUI.ViewModels.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.ViewModelBuilders
{
	[TestClass]
	public class GameViewModelBuilderTest
	{
		private Mock<IGameService> _mockGameService;
		private GameViewModelBuilder _gameViewModelBuilder;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockGameService = new Mock<IGameService>();

			PopulateServiceMock();

			_gameViewModelBuilder = new GameViewModelBuilder(_mockGameService.Object);
		}

		#region BuildCreateGameViewModel method

		[TestMethod]
		public void BuildCreateGameViewModel_Does_Not_Return_Null()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildCreateGameViewModel();

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		public void BuildCreateGameViewModel_Maps_Genres_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildCreateGameViewModel();

			Assert.AreEqual("genre-name-0", viewModel.Genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", viewModel.Genres.ElementAt(1).Name);
		}

		[TestMethod]
		public void BuildCreateGameViewModel_Maps_Platforms_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildCreateGameViewModel();

			Assert.AreEqual("platform-type-0", viewModel.PlatformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", viewModel.PlatformTypes.ElementAt(1).Type);
		}

		[TestMethod]
		public void BuildCreateGameViewModel_Maps_Publishers_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildCreateGameViewModel();

			Assert.AreEqual("company-name-0", viewModel.Publishers.ElementAt(1).CompanyName);
			Assert.AreEqual("company-name-1", viewModel.Publishers.ElementAt(2).CompanyName);
		}

		#endregion

		#region BuildEditGameViewModel method

		[TestMethod]
		public void BuildEditGameViewModel_Does_Not_Return_Null()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildEditGameViewModel_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameViewModelBuilder.BuildEditGameViewModel(string.Empty);
		}

		[TestMethod]
		public void BuildEditGameViewModel_Maps_Game_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.AreEqual("game-key", viewModel.Game.Key);
		}

		[TestMethod]
		public void BuildEditGameViewModel_Maps_Publishers_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.AreEqual("company-name-0", viewModel.Publishers.ElementAt(1).CompanyName);
			Assert.AreEqual("company-name-1", viewModel.Publishers.ElementAt(2).CompanyName);
		}

		[TestMethod]
		public void BuildEditGameViewModel_Maps_Genres_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.AreEqual("genre-name-0", viewModel.Genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", viewModel.Genres.ElementAt(1).Name);
		}

		[TestMethod]
		public void BuildEditGameViewModel_Maps_Platforms_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.AreEqual("platform-type-0", viewModel.PlatformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", viewModel.PlatformTypes.ElementAt(1).Type);
		}

		[TestMethod]
		public void BuildEditGameViewModel_Selects_Genres_Id_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.AreEqual("genre-name-0", viewModel.SelectedGenreNames.ElementAt(0));
			Assert.AreEqual("genre-name-1", viewModel.SelectedGenreNames.ElementAt(1));
		}

		[TestMethod]
		public void BuildEditGameViewModel_Selects_Platforms_Id_Appropriately()
		{
			CreateEditGameViewModel viewModel = _gameViewModelBuilder.BuildEditGameViewModel("game-key");

			Assert.AreEqual("platform-type-0", viewModel.SelectedPlatformTypeNames.ElementAt(0));
			Assert.AreEqual("platform-type-1", viewModel.SelectedPlatformTypeNames.ElementAt(1));
		}

		#endregion

		#region BuildGameDetailsViewModel method

		[TestMethod]
		public void BuildGameDetailsViewModel_Does_Not_Return_Null()
		{
			GameDetailsViewModel viewModel = _gameViewModelBuilder.BuildGameDetailsViewModel("game-key");

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildGameDetailsViewModel_Throws_ArgumentNullException_When_Game_Key_Is_Null_Or_Empty()
		{
			_gameViewModelBuilder.BuildGameDetailsViewModel(string.Empty);
		}

		[TestMethod]
		public void BuildGameDetailsViewModel_Maps_Game_Appropriately()
		{
			GameDetailsViewModel viewModel = _gameViewModelBuilder.BuildGameDetailsViewModel("game-key");

			Assert.AreEqual("game-key", viewModel.Game.Key);
		}

		[TestMethod]
		public void BuildGameDetailsViewModel_Maps_Publisher_Appropriately()
		{
			GameDetailsViewModel viewModel = _gameViewModelBuilder.BuildGameDetailsViewModel("game-key");

			Assert.AreEqual("company-name", viewModel.Publisher.CompanyName);
		}

		[TestMethod]
		public void BuildGameDetailsViewModel_Maps_Genres_Appropriately()
		{
			GameDetailsViewModel viewModel = _gameViewModelBuilder.BuildGameDetailsViewModel("game-key");

			Assert.AreEqual("genre-name-0", viewModel.Genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", viewModel.Genres.ElementAt(1).Name);
		}

		[TestMethod]
		public void BuildGameDetailsViewModel_Maps_Platforms_Appropriately()
		{
			GameDetailsViewModel viewModel = _gameViewModelBuilder.BuildGameDetailsViewModel("game-key");

			Assert.AreEqual("platform-type-0", viewModel.PlatformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", viewModel.PlatformTypes.ElementAt(1).Type);
		}

		#endregion

		#region RebuildAllGamesViewModel method

		[TestMethod]
		public void RebuildAllGamesViewModel_Does_Not_Return_Null()
		{
			var viewModel = new AllGamesViewModel();

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RebuildAllGamesViewModel_Throws_ArgumentNullException_When_ViewModel_Is_Null()
		{
			_gameViewModelBuilder.RebuildAllGamesViewModel(null);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Fills_Genres_Appropriately()
		{
			var viewModel = new AllGamesViewModel();

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.AreEqual("genre-name-0", viewModel.GenreFilter.Genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", viewModel.GenreFilter.Genres.ElementAt(1).Name);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Fills_Platforms_Appropriately()
		{
			var viewModel = new AllGamesViewModel();

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.AreEqual("platform-type-0", viewModel.PlatformFilter.PlatformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", viewModel.PlatformFilter.PlatformTypes.ElementAt(1).Type);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Fills_Publishers_Appropriately()
		{
			var viewModel = new AllGamesViewModel();

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.AreEqual("company-name-0", viewModel.PublisherFilter.Publishers.ElementAt(0).CompanyName);
			Assert.AreEqual("company-name-1", viewModel.PublisherFilter.Publishers.ElementAt(1).CompanyName);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Calls_GetGamesList_Without_Page_Patameter_When_ItemsPerPage_Equals_Zero()
		{
			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel
				{
					ItemsPerPage = 0
				}
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			_mockGameService.Verify(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()), Times.Once);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Calls_GetGamesList_With_Page_Patameter_When_ItemsPerPage_Assigned()
		{
			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel
				{
					ItemsPerPage = 10
				}
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			_mockGameService.Verify(
				m => m.GetGames(
					It.IsAny<IEnumerable<IFilterBase>>(),
					It.IsAny<ISortBase>(),
					It.IsAny<int>(),
					It.IsAny<int>()),
				Times.Once);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Maps_All_Filters_Appropriately()
		{
			IEnumerable<IFilterBase> filters = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => filters = filterList);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel
				{
					ItemsPerPage = 0,
					CurrentPage = 0,
					TotalGamesNumber = 0
				},
				DateFilter = new GamesByDateFilterViewModel
				{
					SelectedDateOption = GameDateDisplayOptions.All
				},
				GenreFilter = new GamesByGenreFilterViewModel
				{
					GenreNames = new[] { "1", "2" }
				},
				NameFilter = new GamesByNameFilterViewModel
				{
					Name = "game-key"
				},
				PlatformFilter = new GamesByPlatformFilterViewModel
				{
					SelectedPlatformTypes = new[] { "1", "2" }
				},
				PriceFilter = new GamesByPriceFilterViewModel
				{
					MinPrice = 10,
					MaxPrice = 10
				},
				PublisherFilter = new GamesByPublisherFilterViewModel
				{
					PublisherCompanies = new[] { "1", "2" }
				}
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.AreEqual(6, filters.Count(f => f.IsSet));
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Applies_MostCommented_Sorting()
		{
			ISortBase sortBase = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => sortBase = sorting);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel { ItemsPerPage = 0 },
				Sorting = new GameSortingViewModel { SelectedSortOption = GameSortDisplayOptions.MostCommented }
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(sortBase);
			Assert.IsInstanceOfType(sortBase, typeof(GameByMostCommentedSorting));
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Applies_MostViewed_Sorting()
		{
			ISortBase sortBase = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => sortBase = sorting);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel { ItemsPerPage = 0 },
				Sorting = new GameSortingViewModel { SelectedSortOption = GameSortDisplayOptions.MostViewed }
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(sortBase);
			Assert.IsInstanceOfType(sortBase, typeof(GameByMostViewedSorting));
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Applies_DateAdding_Sorting()
		{
			ISortBase sortBase = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => sortBase = sorting);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel { ItemsPerPage = 0 },
				Sorting = new GameSortingViewModel { SelectedSortOption = GameSortDisplayOptions.New }
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(sortBase);
			Assert.IsInstanceOfType(sortBase, typeof(GameByDateAddingSorting));
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Applies_PriceAsc_Sorting()
		{
			ISortBase sortBase = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => sortBase = sorting);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel { ItemsPerPage = 0 },
				Sorting = new GameSortingViewModel { SelectedSortOption = GameSortDisplayOptions.PriceAsc }
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(sortBase);
			Assert.IsInstanceOfType(sortBase, typeof(GameByPriceSorting));
			Assert.IsTrue((sortBase as GameByPriceSorting).IsAscending);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Applies_PriceDesc_Sorting()
		{
			ISortBase sortBase = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => sortBase = sorting);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel { ItemsPerPage = 0 },
				Sorting = new GameSortingViewModel { SelectedSortOption = GameSortDisplayOptions.PriceDesc }
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(sortBase);
			Assert.IsInstanceOfType(sortBase, typeof(GameByPriceSorting));
			Assert.IsFalse((sortBase as GameByPriceSorting).IsAscending);
		}

		[TestMethod]
		public void RebuildAllGamesViewModel_Applies_MostViewed_Sorting_By_Default()
		{
			ISortBase sortBase = null;
			_mockGameService
				.Setup(m => m.GetGames(It.IsAny<IEnumerable<IFilterBase>>(), It.IsAny<ISortBase>()))
				.Callback<IEnumerable<IFilterBase>, ISortBase>((filterList, sorting) => sortBase = sorting);

			var viewModel = new AllGamesViewModel
			{
				Paging = new PagingViewModel { ItemsPerPage = 0 }
			};

			_gameViewModelBuilder.RebuildAllGamesViewModel(viewModel);

			Assert.IsNotNull(sortBase);
			Assert.IsInstanceOfType(sortBase, typeof(GameByMostViewedSorting));
		}

		#endregion

		#region RebuildCreateGameViewModel method

		[TestMethod]
		public void RebuildCreateGameViewModel_Does_Not_Return_Null()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildCreateGameViewModel(viewModel);

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RebuildCreateGameViewModel_Throws_ArgumentNullException_When_ViewModel_Is_Null()
		{
			_gameViewModelBuilder.RebuildCreateGameViewModel(null);
		}

		[TestMethod]
		public void RebuildCreateGameViewModel_Fills_Genres_Appropriately()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildCreateGameViewModel(viewModel);

			Assert.AreEqual("genre-name-0", viewModel.Genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", viewModel.Genres.ElementAt(1).Name);
		}

		[TestMethod]
		public void RebuildCreateGameViewModel_Fills_Platforms_Appropriately()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildCreateGameViewModel(viewModel);

			Assert.AreEqual("platform-type-0", viewModel.PlatformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", viewModel.PlatformTypes.ElementAt(1).Type);
		}

		[TestMethod]
		public void RebuildCreateGameViewModel_Fills_Publishers_Appropriately()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildCreateGameViewModel(viewModel);

			Assert.AreEqual("company-name-0", viewModel.Publishers.ElementAt(1).CompanyName);
			Assert.AreEqual("company-name-1", viewModel.Publishers.ElementAt(2).CompanyName);
		}

		#endregion

		#region RebuildEditGameViewModel method

		[TestMethod]
		public void RebuildEditGameViewModel_Does_Not_Return_Null()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildEditGameViewModel(viewModel);

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RebuildEditGameViewModel_Throws_ArgumentNullException_When_ViewModel_Is_Null()
		{
			_gameViewModelBuilder.RebuildEditGameViewModel(null);
		}

		[TestMethod]
		public void RebuildEditGameViewModel_Fills_Genres_Appropriately()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildEditGameViewModel(viewModel);

			Assert.AreEqual("genre-name-0", viewModel.Genres.ElementAt(0).Name);
			Assert.AreEqual("genre-name-1", viewModel.Genres.ElementAt(1).Name);
		}

		[TestMethod]
		public void RebuildEditGameViewModel_Fills_Platforms_Appropriately()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildEditGameViewModel(viewModel);

			Assert.AreEqual("platform-type-0", viewModel.PlatformTypes.ElementAt(0).Type);
			Assert.AreEqual("platform-type-1", viewModel.PlatformTypes.ElementAt(1).Type);
		}

		[TestMethod]
		public void RebuildEditGameViewModel_Fills_Publishers_Appropriately()
		{
			var viewModel = new CreateEditGameViewModel();

			_gameViewModelBuilder.RebuildEditGameViewModel(viewModel);

			Assert.AreEqual("company-name-0", viewModel.Publishers.ElementAt(1).CompanyName);
			Assert.AreEqual("company-name-1", viewModel.Publishers.ElementAt(2).CompanyName);
		}

		#endregion

		private void PopulateServiceMock()
		{
			_mockGameService.Setup(m => m.GetGame(It.IsAny<string>())).Returns(new GameDto
			{
				Key = "game-key",
				GameLocalizations = new Collection<GameLocalizationDto>
				{
					new GameLocalizationDto { Language = LanguageDto.Uk }
				}
			});

			_mockGameService.Setup(m => m.GetGamePublisher(It.IsAny<string>())).Returns(new PublisherDto
			{
				CompanyName = "company-name",
				PublisherLocalizations = new Collection<PublisherLocalizationDto>
				{
					new PublisherLocalizationDto { Language = LanguageDto.Uk }
				}
			});

			_mockGameService.Setup(m => m.GetAllGenres()).Returns(new[]
			{
				new GenreDto { Name = "genre-name-0", GenreLocalizations = new Collection<GenreLocalizationDto>() },
				new GenreDto { Name = "genre-name-1", GenreLocalizations = new Collection<GenreLocalizationDto>() }
			});

			_mockGameService.Setup(m => m.GetAllPlatformTypes()).Returns(new[]
			{
				new PlatformTypeDto { Type = "platform-type-0", PlatformLocalizations = new Collection<PlatformLocalizationDto>() },
				new PlatformTypeDto { Type = "platform-type-1", PlatformLocalizations = new Collection<PlatformLocalizationDto>( )}
			});

			_mockGameService.Setup(m => m.GetAllPublishers()).Returns(new[]
			{
				new PublisherDto { CompanyName = "company-name-0", PublisherLocalizations = new Collection<PublisherLocalizationDto>() },
				new PublisherDto { CompanyName = "company-name-1" , PublisherLocalizations = new Collection<PublisherLocalizationDto>() }
			});

			_mockGameService.Setup(m => m.GetGameGenres(It.IsAny<string>())).Returns(new[]
			{
				new GenreDto { GenreId = 0, Name = "genre-name-0", GenreLocalizations = new Collection<GenreLocalizationDto>() },
				new GenreDto { GenreId = 1, Name = "genre-name-1", GenreLocalizations = new Collection<GenreLocalizationDto>() }
			});

			_mockGameService.Setup(m => m.GetGamePlatformTypes(It.IsAny<string>())).Returns(new[]
			{
				new PlatformTypeDto { PlatformTypeId = 0, Type = "platform-type-0", PlatformLocalizations = new Collection<PlatformLocalizationDto>() },
				new PlatformTypeDto { PlatformTypeId = 1, Type = "platform-type-1", PlatformLocalizations = new Collection<PlatformLocalizationDto>() }
			});
		}
	}
}