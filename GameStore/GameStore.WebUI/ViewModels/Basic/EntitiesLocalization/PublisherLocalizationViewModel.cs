using System.ComponentModel.DataAnnotations;
using GameStore.BLL.DTOs.Components;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic.EntitiesLocalization
{
	public class PublisherLocalizationViewModel
	{
		public LanguageDto Language { get; set; }

		[Required]
		[StringLength(40)]
		[Display(ResourceType = typeof(Resource), Name = "PublisherViewModel_CompanyName")]
		public string CompanyName { get; set; }

		[Required]
		[StringLength(1000)]
		[Display(ResourceType = typeof(Resource), Name = "PublisherViewModel_Description")]
		public string Description { get; set; }
	}
}