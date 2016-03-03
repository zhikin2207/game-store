using System.Runtime.Serialization;

namespace BankService.DTO
{
	[DataContract]
	public class UserDto
	{
		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public AccountDto Account { get; set; }

		[DataMember]
		public string Email { get; set; }
	}
}