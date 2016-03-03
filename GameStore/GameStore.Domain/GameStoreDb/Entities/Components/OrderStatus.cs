namespace GameStore.Domain.GameStoreDb.Entities.Components
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

		/// <summary>
		/// Status when order is shipped.
		/// </summary>
		Shipped
	}
}