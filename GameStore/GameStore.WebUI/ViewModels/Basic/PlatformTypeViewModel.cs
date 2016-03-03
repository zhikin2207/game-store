using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	public class PlatformTypeViewModel
	{
		public int PlatformTypeId { get; set; }

		[Required]
		[StringLength(50)]
		[Display(ResourceType = typeof(Resource), Name = "Common_Platform")]
		public string Type { get; set; }

		public string DisplayType { get; set; }
	}
}