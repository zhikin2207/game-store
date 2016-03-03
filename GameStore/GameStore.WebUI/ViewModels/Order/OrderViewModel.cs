using System;
using System.Collections.Generic;
using GameStore.BLL.DTOs.Components;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.Order
{
	public class OrderViewModel
	{
		public OrderViewModel()
		{
			OrderDetails = new List<OrderDetailsViewModel>();
		}

		public int OrderId { get; set; }

		public OrderDtoStatus Status { get; set; }

		public int? ShipperId { get; set; }

		public string ShipperName { get; set; }

		public DateTime? OrderDate { get; set; }

		public DateTime? ShippedDate { get; set; }

		public decimal TotalPrice { get; set; }

		public IEnumerable<OrderDetailsViewModel> OrderDetails { get; set; }
	}
}