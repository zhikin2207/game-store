using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.Order
{
	public class AllShippersViewModel
	{
		public AllShippersViewModel()
		{
			Shippers = new List<ShipperViewModel>();
		}

		public IEnumerable<ShipperViewModel> Shippers { get; set; }

		public int? SelectedShipperId { get; set; }
	}
}