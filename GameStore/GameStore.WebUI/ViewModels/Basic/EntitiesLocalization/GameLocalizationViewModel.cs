using System.ComponentModel.DataAnnotations;
using GameStore.BLL.DTOs.Components;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic.EntitiesLocalization
{
	public class GameLocalizationViewModel
	{
		public LanguageDto Language { get; set; }

		[Required]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Name")]
		public string Name { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Description")]
		public string Description { get; set; }
	}
}