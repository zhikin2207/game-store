using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.Order
{
	public class BasketViewModel
	{
		public BasketViewModel()
		{
			OrderDetails = new List<OrderDetailsViewModel>();
		}

		public IEnumerable<OrderDetailsViewModel> OrderDetails { get; set; }

		public decimal TotalPrice { get; set; }
	}
}