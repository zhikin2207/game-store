using System;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	public class PublisherViewModel
	{
		public int PublisherId { get; set; }

		public Guid? UserGuid { get; set; }

		public string DisplayCompanyName { get; set; }

		public string DisplayDescription { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		[Display(ResourceType = typeof(Resource), Name = "PublisherViewModel_CompanyName")]
		public string CompanyName { get; set; }

		[Required]
		[StringLength(1000)]
		[Display(ResourceType = typeof(Resource), Name = "PublisherViewModel_Description")]
		public string Description { get; set; }

		[Required]
		[StringLength(100)]
		[Display(ResourceType = typeof(Resource), Name = "PublisherViewModel_HomePage")]
		public string HomePage { get; set; }
	}
}