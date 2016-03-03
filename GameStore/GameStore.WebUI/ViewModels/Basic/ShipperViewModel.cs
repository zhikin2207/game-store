using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	public class ShipperViewModel
	{
		public int ShipperId { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "ShipperViewModel_CompanyName")]
		public string CompanyName { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "ShipperViewModel_Phone")]
		public string Phone { get; set; }
	}
}