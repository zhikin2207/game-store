using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.User
{
	public class ChangeRoleViewModel
	{
		public UserViewModel User { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Roles")]
		public IEnumerable<RoleViewModel> Roles { get; set; }

		public int SelectedRoleId { get; set; }
	}
}