using System.Linq;
using GameStore.BLL.Banking;
using GameStore.BLL.DTOs;
using GameStore.BLL.Payments.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Payments
{
	public class VisaPayment : IPaymentMethod
	{
		public const string Purpose = "Online game purchase.";

		public string CardHolderName { get; set; }

		public string CardNumber { get; set; }

		public int ExpirationMonth { get; set; }

		public int ExpirationYear { get; set; }

		public string CVV2 { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public PaymentStatus Pay(Order basket)
		{
			var paymentService = new PaymentServiceClient();

			if (!paymentService.IsCardExists(CardNumber))
			{
				return PaymentStatus.CardDoesNotExist;
			}

			PaymentStatus status = paymentService.PayWithVisa(
				CardNumber,
				CardHolderName,
				CVV2,
				ExpirationMonth,
				ExpirationYear,
				Purpose,
				basket.OrderDetails.Sum(order => order.Price * order.Quantity),
				Email,
				Phone);

			return status;
		}
	}
}