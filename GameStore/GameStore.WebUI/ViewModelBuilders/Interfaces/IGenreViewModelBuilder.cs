using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Genre;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
	public interface IGenreViewModelBuilder
	{
		IEnumerable<GenreViewModel> BuildGenresViewModel();

		GenreViewModel BuildGenreViewModel(string name);

		CreateEditGenreViewModel BuildCreateGenreViewModel(string parentGenre);

		CreateEditGenreViewModel BuildUpdateGenreViewModel(string name);

		void RebiuldCreateGenreViewModel(CreateEditGenreViewModel createGenreViewModel);

		void RebiuldUpdateGenreViewModel(CreateEditGenreViewModel updateGenreViewModel);
	}
}