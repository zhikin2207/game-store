using System;
using GameStore.BLL.Banking;
using GameStore.BLL.Payments.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Payments
{
	public class IBoxTerminalPayment : IPaymentMethod
	{
		public Guid UserGuid { get; set; }

		public int InvoiceNumber { get; set; }

		public PaymentStatus Pay(Order basket)
		{
			return PaymentStatus.Success;
		}
	}
}