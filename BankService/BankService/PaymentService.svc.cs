using System;
using System.Linq;
using BankService.DTO;
using BankService.DTO.Components;

namespace BankService
{
	public class PaymentService : IPaymentService
	{
		public bool IsCardExists(string cardNumber)
		{
			return DatabaseContext.Cards.Any(a => a.CardNumber == cardNumber);
		}

		public PaymentStatus PayWithVisa(
			string cardNumber, 
			string userName, 
			string cvv2, 
			int expirationMonth, 
			int expiretionYear,
			string purpose, 
			decimal summ, 
			string email = null, 
			string phone = null)
		{
			var status = PaymentStatus.Success;

			CardDto card = GetUserCard(cardNumber);

			bool isCardValid = ValidateCard(ref status, card, cvv2, expirationMonth, expiretionYear, summ);

			if (isCardValid)
			{
				ApplyWithdrawal(card, summ);
				NotifyUserAboutTransfer(email, phone);
			}

			return status;
		}

		public PaymentStatus PayWithMasterCard(
			string cardNumber,
			string userName,
			string cvv2,
			int expirationMonth,
			int expiretionYear,
			string purpose,
			decimal summ,
			string email = null,
			string phone = null)
		{
			var status = PaymentStatus.Success;

			CardDto card = GetUserCard(cardNumber);

			bool isCardValid = ValidateCard(ref status, card, cvv2, expirationMonth, expiretionYear, summ);

			if (isCardValid)
			{
				ApplyWithdrawal(card, summ);
				NotifyUserAboutTransfer(email, phone);
			}

			return status;
		}

		private bool ValidateCard(ref PaymentStatus status, CardDto card, string cvv2, int expirationMonth, int expiretionYear, decimal summ)
		{
			bool check = SetExistStatus(ref status, true, card);
			check = SetValidStatus(ref status, check, card, cvv2, expirationMonth, expiretionYear);
			check = SetExpiredStatus(ref status, check, card);
			SetMoneyStatus(ref status, check, card, summ);

			return status == PaymentStatus.Success;
		}

		private bool SetExistStatus(ref PaymentStatus status, bool check, CardDto card)
		{
			if (!check)
			{
				return false;
			}

			if (card == null)
			{
				status = PaymentStatus.PaymentFailed;
			}

			return card != null;
		}

		private bool SetValidStatus(ref PaymentStatus status, bool check, CardDto card, string cvv2, int expirationMonth, int expiretionYear)
		{
			if (!check)
			{
				return false;
			}

			if (card != null)
			{
				bool isCardValid = card.CVV2 == cvv2 &&
				              card.ExpirationMonth == expirationMonth &&
				              card.ExpirationYear == expiretionYear;

				if (isCardValid)
				{
					return true;
				}
			}

			status = PaymentStatus.PaymentFailed;
			return false;
		}

		private bool SetExpiredStatus(ref PaymentStatus status, bool check, CardDto card)
		{
			if (!check)
			{
				return false;
			}

			if (card != null)
			{
				DateTime dateNow = DateTime.UtcNow;

				bool isCardExpired =
					card.ExpirationYear < dateNow.Year ||
					(card.ExpirationYear == dateNow.Year && card.ExpirationMonth < dateNow.Month);

				if (!isCardExpired)
				{
					return true;
				}
			}

			status = PaymentStatus.PaymentFailed;
			return false;
		}

		private bool SetMoneyStatus(ref PaymentStatus status, bool check, CardDto card, decimal summ)
		{
			if (!check)
			{
				return false;
			}

			if (card != null)
			{
				bool isEnoughMoney = card.Account.Balance - summ > 0;

				if (isEnoughMoney)
				{
					return true;
				}
			}

			status = PaymentStatus.NotEnoughMoney;
			return false;
		}

		private CardDto GetUserCard(string cardNumber)
		{
			return DatabaseContext.Cards.FirstOrDefault(c => c.CardNumber == cardNumber);
		}

		private void WriteTransferHistory(CardDto card, OperationType type)
		{
			var transfer = new TransferDto
			{
				Account = card.Account,
				PerformedAt = DateTime.UtcNow,
				OperationType = type
			};
			
			DatabaseContext.Transfers.Add(transfer);
		}

		private void ApplyWithdrawal(CardDto card, decimal summ)
		{
			card.Account.Balance -= summ;

			WriteTransferHistory(card, OperationType.Withdrawal);
		}

		private void NotifyUserAboutTransfer(string email, string phone)
		{
			if (string.IsNullOrEmpty(email))
			{
				SendTransferInfoByEmail(email);
			}

			if (string.IsNullOrEmpty(phone))
			{
				SendTransferInfoBySms(phone);
			}
		}

		private void SendTransferInfoByEmail(string email)
		{
			// Code for sending email
		}

		private void SendTransferInfoBySms(string phone)
		{
			// Code for sending sms
		}
	}
}
