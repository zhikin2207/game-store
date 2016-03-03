using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.Components;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Genre;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class GenreViewModelBuilder : IGenreViewModelBuilder
	{
		private const string ParentGenreName = "None";

		private readonly IGenreService _genreService;

		public GenreViewModelBuilder(IGenreService genreService)
		{
			_genreService = genreService;
		}

		public IEnumerable<GenreViewModel> BuildGenresViewModel()
		{
			IEnumerable<GenreDto> genres = _genreService.GetGenres();
			return Mapper.Map<IEnumerable<GenreViewModel>>(genres);
		}

		public CreateEditGenreViewModel BuildCreateGenreViewModel(string parentGenre)
		{
			return new CreateEditGenreViewModel
			{
				ParentGenres = GetGenres(),
				SelectedParentGenre = parentGenre
			};
		}

		public CreateEditGenreViewModel BuildUpdateGenreViewModel(string name)
		{
			GenreDto genre = _genreService.GetGenre(name);

			List<GenreViewModel> genres = GetGenres().ToList();
			genres.RemoveAll(g => g.Name == genre.Name);

			GenreLocalizationDto ukrainianLocalization = genre.GenreLocalizations.FirstOrDefault(lg => lg.Language == LanguageDto.Uk);

			var updateGenreViewModel = new CreateEditGenreViewModel
			{
				ParentGenres = genres,
				CurrentGenre = Mapper.Map<GenreViewModel>(genre),
				SelectedParentGenre = genre.Parent == null ? ParentGenreName : genre.Parent.Name,
				UkrainianGenreLocalization = Mapper.Map<GenreLocalizationViewModel>(ukrainianLocalization)
			};

			return updateGenreViewModel;
		}

		public GenreViewModel BuildGenreViewModel(string name)
		{
			GenreDto genre = _genreService.GetGenre(name);
			return Mapper.Map<GenreViewModel>(genre);
		}

		public void RebiuldCreateGenreViewModel(CreateEditGenreViewModel createGenreViewModel)
		{
			createGenreViewModel.ParentGenres = GetGenres();
		}

		public void RebiuldUpdateGenreViewModel(CreateEditGenreViewModel updateGenreViewModel)
		{
			updateGenreViewModel.ParentGenres = GetGenres();
		}

		private IEnumerable<GenreViewModel> GetGenres()
		{
			List<GenreDto> genres = _genreService.GetGenres().ToList();

			var mappedGenres = Mapper.Map<List<GenreViewModel>>(genres);
			mappedGenres.Insert(0, GetParentGenreStub());

			return mappedGenres;
		}

		private GenreViewModel GetParentGenreStub()
		{
			return new GenreViewModel
			{
				DisplayName = Resource.GameViewModelBuilder_OtherGenre
			};
		}
	}
}