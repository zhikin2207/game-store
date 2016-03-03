using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;

namespace GameStore.WebUI.ViewModels.Publisher
{
	public class CreateEditPublisherViewModel
	{
		public CreateEditPublisherViewModel()
		{
			CurrentPublisher = new PublisherViewModel();
		}

		public PublisherViewModel CurrentPublisher { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Languages_Ukrainian")]
		public PublisherLocalizationViewModel UkrainianPublisherLocalization { get; set; }  
	}
}