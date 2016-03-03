using System.ComponentModel.DataAnnotations;
using GameStore.BLL.DTOs.Components;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic.EntitiesLocalization
{
	public class GenreLocalizationViewModel
	{
		public LanguageDto Language { get; set; }

		[Required]
		[StringLength(50)]
		[Display(ResourceType = typeof(Resource), Name = "GenreViewModel_Name")]
		public string Name { get; set; }
	}
}