using System;
using System.Collections.Generic;
using GameStore.BLL.DTOs.Components;

namespace GameStore.BLL.DTOs
{
	public class OrderDto
	{
		public int OrderId { get; set; }

		public Guid BasketGuid { get; set; }

		public Guid? UserGuid { get; set; }

		public DateTime? OrderDate { get; set; }

		public OrderDtoStatus Status { get; set; }

		public int? ShipperId { get; set; }

		public DateTime? ShippedDate { get; set; }

		public UserDto User { get; set; }

		public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
	}
}
