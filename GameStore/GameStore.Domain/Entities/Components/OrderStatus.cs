namespace GameStore.Domain.Entities.Components
{
	/// <summary>
	/// Order status.
	/// </summary>
	public enum OrderStatus
	{
		/// <summary>
		/// Status when order paid.
		/// </summary>
		Paid,

		/// <summary>
		/// Status when order is basket.
		/// </summary>
		NotPaid,

		Shipped
	}
}