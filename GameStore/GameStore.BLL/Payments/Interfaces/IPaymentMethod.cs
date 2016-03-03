using GameStore.BLL.Banking;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Payments.Interfaces
{
	public interface IPaymentMethod
	{
		/// <summary>
		/// Perform pay action.
		/// </summary>
		/// <returns>True when order was paid</returns>
		PaymentStatus Pay(Order basket);
	}
}