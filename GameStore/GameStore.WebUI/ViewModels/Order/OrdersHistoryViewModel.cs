using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Order
{
	public class OrdersHistoryViewModel
	{
		public OrdersHistoryViewModel()
		{
			Orders = new List<OrderViewModel>();
		}

		public Guid? UserGuid { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "OrdersHistoryViewModel_StartDate")]
		public DateTime? StartDate { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "OrdersHistoryViewModel_EndDate")]
		public DateTime? EndDate { get; set; }

		public IEnumerable<OrderViewModel> Orders { get; set; }
	}
}