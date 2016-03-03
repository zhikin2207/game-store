using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
	public class OrderDetails
	{
		[Key]
		public int OrderDetailsId { get; set; }

		public int? OrderId { get; set; }

		[Required]
		public string GameKey { get; set; }

		public decimal Price { get; set; }

		public short Quantity { get; set; }

		public float Discount { get; set; }

		public virtual Order Order { get; set; }

		public virtual Game Game { get; set; }
	}
}