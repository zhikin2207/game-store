using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.Domain.GameStoreDb.Entities
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }

		public Guid BasketGuid { get; set; }

		[ForeignKey("User")]
		public Guid? UserGuid { get; set; }

		public DateTime? OrderDate { get; set; }

		public OrderStatus Status { get; set; }

		public int? ShipperId { get; set; }

		public DateTime? ShippedDate { get; set; }

		public virtual User User { get; set; }

		public virtual ICollection<OrderDetails> OrderDetails { get; set; }
	}
}