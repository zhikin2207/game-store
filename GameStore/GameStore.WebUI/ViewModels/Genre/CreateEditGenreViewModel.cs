using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;

namespace GameStore.WebUI.ViewModels.Genre
{
	public class CreateEditGenreViewModel
	{
		public CreateEditGenreViewModel()
		{
			ParentGenres = new List<GenreViewModel>();
		}

		public string SelectedParentGenre { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "CreateGenreViewModel_ParentGenre")]
		public IEnumerable<GenreViewModel> ParentGenres { get; set; }

		public GenreViewModel CurrentGenre { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Languages_Ukrainian")]
		public GenreLocalizationViewModel UkrainianGenreLocalization { get; set; } 
	}
}