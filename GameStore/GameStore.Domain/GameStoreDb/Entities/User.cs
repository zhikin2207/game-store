using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.GameStoreDb.Entities
{
	public class User
	{
		[Key]
		public Guid UserGuid { get; set; }

		[ForeignKey("Role")]
		public int RoleId { get; set; }

		[ForeignKey("Publisher")]
		public int? PublisherId { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 4)]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[StringLength(50)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public DateTime CreatedAt { get; set; }

        public bool IsBanned { get; set; }

        public DateTime? BanReleaseAt { get; set; }

		[Required]
		[StringLength(2)]
		[RegularExpression("en|uk")]
		public string Language { get; set; }

		public virtual Role Role { get; set; }

		public virtual Publisher Publisher { get; set; }
	}
}