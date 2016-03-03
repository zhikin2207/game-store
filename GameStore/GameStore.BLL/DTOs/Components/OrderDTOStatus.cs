namespace GameStore.BLL.DTOs.Components
{
	/// <summary>
	/// Order status.
	/// </summary>
	public enum OrderDtoStatus
	{
		/// <summary>
		/// Status when order is not paid.
		/// </summary>
		NotPaid = 0,

		/// <summary>
		/// Status when order is paid.
		/// </summary>
		Paid = 1,

		/// <summary>
		/// Status when order is shipped.
		/// </summary>
		Shipped = 2
	}
}