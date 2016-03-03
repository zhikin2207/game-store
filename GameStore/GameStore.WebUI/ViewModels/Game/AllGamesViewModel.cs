using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Filter;
using GameStore.WebUI.ViewModels.Sorting;

namespace GameStore.WebUI.ViewModels.Game
{
	public class AllGamesViewModel
	{
		public AllGamesViewModel()
		{
			PriceFilter = new GamesByPriceFilterViewModel();
			GenreFilter = new GamesByGenreFilterViewModel();
			PlatformFilter = new GamesByPlatformFilterViewModel();
			PublisherFilter = new GamesByPublisherFilterViewModel();
			NameFilter = new GamesByNameFilterViewModel();
			DateFilter = new GamesByDateFilterViewModel();
			Paging = new PagingViewModel();
			ExistanceFilter = new GamesByExistanceFilterViewModel();
			Sorting = new GameSortingViewModel();
		}

		public IEnumerable<GameViewModel> Games { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "AllGamesViewModel_Price")]
		public GamesByPriceFilterViewModel PriceFilter { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Genres")]
		public GamesByGenreFilterViewModel GenreFilter { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Platforms")]
		public GamesByPlatformFilterViewModel PlatformFilter { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Publishers")]
		public GamesByPublisherFilterViewModel PublisherFilter { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "AllGamesViewModel_Name")]
		public GamesByNameFilterViewModel NameFilter { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "AllGamesViewModel_Sort")]
		public GameSortingViewModel Sorting { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "AllGamesViewModel_Date")]
		public GamesByDateFilterViewModel DateFilter { get; set; }

		public GamesByExistanceFilterViewModel ExistanceFilter { get; set; }

		public PagingViewModel Paging { get; set; }
	}
}