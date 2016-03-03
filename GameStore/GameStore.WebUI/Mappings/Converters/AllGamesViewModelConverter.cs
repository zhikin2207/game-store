using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.Filtering.Filters;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.WebUI.ViewModels.Game;

namespace GameStore.WebUI.Mappings.Converters
{
	public class AllGamesViewModelConverter : ITypeConverter<AllGamesViewModel, IEnumerable<IFilterBase>>
	{
		public IEnumerable<IFilterBase> Convert(ResolutionContext context)
		{
			var viewModel = context.SourceValue as AllGamesViewModel;

			if (viewModel != null)
			{
				yield return Mapper.Map<GamesByPriceFilter>(viewModel.PriceFilter);
				yield return Mapper.Map<GamesByGenreFilter>(viewModel.GenreFilter);
				yield return Mapper.Map<GamesByPlatformFilter>(viewModel.PlatformFilter);
				yield return Mapper.Map<GamesByPublisherFilter>(viewModel.PublisherFilter);
				yield return Mapper.Map<GamesByNameFilter>(viewModel.NameFilter);
				yield return Mapper.Map<GamesByDateFilter>(viewModel.DateFilter);
				yield return Mapper.Map<GamesByExistanceFilter>(viewModel.ExistanceFilter);
			}
		}
	}
}