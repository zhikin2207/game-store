using System.Runtime.Serialization;

namespace GameStore.BLL.DTOs
{
	[DataContract]
	public class OrderDetailsDto
	{
		[DataMember]
		public int OrderDetailsId { get; set; }

		[DataMember]
		public int OrderId { get; set; }

		[DataMember]
		public string GameKey { get; set; }

		[DataMember]
		public decimal Price { get; set; }

		[DataMember]
		public short Quantity { get; set; }

		[DataMember]
		public float Discount { get; set; }

		public GameDto Game { get; set; }
	}
}
