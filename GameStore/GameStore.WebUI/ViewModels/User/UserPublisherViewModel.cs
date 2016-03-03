using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.User
{
	public class UserPublisherViewModel
	{
		public UserViewModel User { get; set; }

		public string CurrentCompanyName { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Publishers")]
		public IEnumerable<PublisherViewModel> Publishers { get; set; } 
	}
}