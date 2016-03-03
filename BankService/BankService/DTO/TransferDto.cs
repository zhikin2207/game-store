using System;
using System.Runtime.Serialization;
using BankService.DTO.Components;

namespace BankService.DTO
{
	[DataContract]
	public class TransferDto
	{
		[DataMember]
		public AccountDto Account { get; set; }

		[DataMember]
		public DateTime PerformedAt { get; set; }

		[DataMember]
		public OperationType OperationType { get; set; }
	}
}