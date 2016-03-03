using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	public class RoleViewModel
	{
		public int RoleId { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 3)]
		[Display(ResourceType = typeof(Resource), Name = "Common_Role")]
		public string Name { get; set; }

		public bool IsSystem { get; set; }
	}
}
