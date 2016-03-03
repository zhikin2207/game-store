using System;
using System.ServiceModel;
using BankService.DTO.Components;

namespace BankService
{
	[ServiceContract]
	public interface IPaymentService
	{
		[OperationContract]
		bool IsCardExists(string cardNumber);

		[OperationContract]
		PaymentStatus PayWithVisa(
			string cardNumber,
			string userName,
			string cvv2,
			int expirationMonth,
			int expiretionYear,
			string purpose,
			decimal summ,
			string email = null,
			string phone = null);

		[OperationContract]
		PaymentStatus PayWithMasterCard(
			string cardNumber,
			string userName,
			string cvv2,
			int expirationMonth,
			int expiretionYear,
			string purpose,
			decimal summ,
			string email = null,
			string phone = null);
	}
}
