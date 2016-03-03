using AutoMapper;
using GameStore.BLL.Sorting.Components;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.BLL.Sorting.Sortings;
using GameStore.WebUI.ViewModels.Sorting;

namespace GameStore.WebUI.Mappings.Converters
{
	public class GameSortingViewModelConverter : ITypeConverter<GameSortingViewModel, ISortBase>
	{
		public ISortBase Convert(ResolutionContext context)
		{
			var viewModel = (GameSortingViewModel)context.SourceValue;

			switch (viewModel.SelectedSortOption)
			{
				case GameSortDisplayOptions.MostViewed:
					return new GameByMostViewedSorting();
				case GameSortDisplayOptions.MostCommented:
					return new GameByMostCommentedSorting();
				case GameSortDisplayOptions.New:
					return new GameByDateAddingSorting();
				case GameSortDisplayOptions.PriceAsc:
					return new GameByPriceSorting(true);
				case GameSortDisplayOptions.PriceDesc:
					return new GameByPriceSorting(false);
				default:
					return new GameByMostViewedSorting();
			}
		}
	}
}