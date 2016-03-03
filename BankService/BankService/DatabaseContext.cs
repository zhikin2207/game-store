using System.Collections.Generic;
using System.Collections.ObjectModel;
using BankService.DTO;

namespace BankService
{
	public static class DatabaseContext
	{
		static DatabaseContext()
		{
			Transfers = new Collection<TransferDto>();
			Users = new Collection<UserDto>();
			Accounts = new Collection<AccountDto>();
			Cards = new Collection<CardDto>();
		}

		public static ICollection<TransferDto> Transfers { get; set; }

		public static ICollection<UserDto> Users { get; set; }

		public static ICollection<AccountDto> Accounts { get; set; }

		public static ICollection<CardDto> Cards { get; set; }
	}
}