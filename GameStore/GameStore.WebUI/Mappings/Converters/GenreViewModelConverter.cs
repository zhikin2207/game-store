using System.Collections.ObjectModel;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Genre;

namespace GameStore.WebUI.Mappings.Converters
{
	public class GenreViewModelConverter : ITypeConverter<CreateEditGenreViewModel, GenreDto>
	{
		public GenreDto Convert(ResolutionContext context)
		{
			var viewModel = (CreateEditGenreViewModel)context.SourceValue;
			var genre = Mapper.Map<GenreDto>(viewModel.CurrentGenre);

			genre.GenreLocalizations = new Collection<GenreLocalizationDto>();

			if (viewModel.UkrainianGenreLocalization != null)
			{
				var ukrainianLocalization = Mapper.Map<GenreLocalizationDto>(viewModel.UkrainianGenreLocalization);
				genre.GenreLocalizations.Add(ukrainianLocalization);
			}

			return genre;
		}
	}
}