using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	public class OrderDetailsViewModel
	{
		public int OrderDetailsId { get; set; }

		public int OrderId { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Key")]
		public string GameKey { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "OrderDetailsViewModel_Price")]
		public decimal Price { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "OrderDetailsViewModel_Quantity")]
		public short Quantity { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "OrderDetailsViewModel_Discount")]
		public float Discount { get; set; }

		public GameViewModel Game { get; set; }
	}
}
