using System.Runtime.Serialization;

namespace BankService.DTO.Components
{
	[DataContract]
	public enum PaymentStatus
	{
		[EnumMember]
		Success,

		[EnumMember]
		NotEnoughMoney,

		[EnumMember]
		CardDoesNotExist,

		[EnumMember]
		PaymentFailed
	}
}