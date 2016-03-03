using System.ComponentModel.DataAnnotations;
using GameStore.BLL.DTOs.Components;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic.EntitiesLocalization
{
	public class PlatformLocalizationViewModel
	{
		public LanguageDto Language { get; set; }

		[Required]
		[StringLength(50)]
		[Display(ResourceType = typeof(Resource), Name = "Common_Platform")]
		public string Type { get; set; }
	}
}