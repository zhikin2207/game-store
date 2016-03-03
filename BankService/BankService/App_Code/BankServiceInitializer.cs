using System.ServiceModel;
using BankService.DTO;

namespace BankService
{
	public static class BankServiceInitializer
	{
		public static void AppInitialize()
		{
			var account = new AccountDto
			{
				Balance = 10000,
				Number = "26004563298423"
			};

			var cards = new[]
			{
				new CardDto
				{
					CardNumber = "1234-5678-90",
					CVV2 = "1234",
					ExpirationMonth = 7,
					ExpirationYear = 2015,
					Account = account
				}
			};

			account.Cards = cards;

			var user = new UserDto
			{
				FirstName = "Dmytro",
				LastName = "Zhykin",
				Email = "dmytro_zhykin@epam.com",
				Account = account
			};

			DatabaseContext.Users.Add(user);
			DatabaseContext.Accounts.Add(account);

			foreach (CardDto card in cards)
			{
				DatabaseContext.Cards.Add(card);
			}

			var paymentHost = new ServiceHost(typeof(PaymentService));

			paymentHost.Open();
		}
	}
}