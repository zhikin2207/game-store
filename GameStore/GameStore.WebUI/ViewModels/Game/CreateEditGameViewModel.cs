using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;

namespace GameStore.WebUI.ViewModels.Game
{
	public class CreateEditGameViewModel : IValidatableObject
	{
		public CreateEditGameViewModel()
		{
			Game = new GameViewModel();

			SelectedGenreNames = new List<string>();
			SelectedPlatformTypeNames = new List<string>();

			Publishers = new List<PublisherViewModel>();
			Genres = new List<GenreViewModel>();
			PlatformTypes = new List<PlatformTypeViewModel>();
		}

		public GameViewModel Game { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Languages_Ukrainian")]
		public GameLocalizationViewModel UkrainianGameLocalization { get; set; }

		public string SelectedPublisherCompanyName { get; set; }

		public IEnumerable<string> SelectedGenreNames { get; set; }

		public IEnumerable<string> SelectedPlatformTypeNames { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Publishers")]
		public IEnumerable<PublisherViewModel> Publishers { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Genres")]
		public IEnumerable<GenreViewModel> Genres { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Platforms")]
		public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validationResults = new List<ValidationResult>();

			if (Game.IsDeleted)
			{
				validationResults.Add(new ValidationResult("Deleted game can not be updated."));
			}

			return validationResults;
		}
	}
}