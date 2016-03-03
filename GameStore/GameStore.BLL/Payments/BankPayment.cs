using System;
using GameStore.BLL.Banking;
using GameStore.BLL.Payments.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Payments
{
	public class BankPayment : IPaymentMethod
	{
		public Guid UserGuid { get; set; }

		public string BuildInvoiceFile()
		{
			return "Invoice file: " + UserGuid;
		}

		public PaymentStatus Pay(Order basket)
		{
			return PaymentStatus.Success;
		}
	}
}