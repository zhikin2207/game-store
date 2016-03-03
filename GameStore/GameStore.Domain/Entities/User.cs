using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
	public class User
	{
		[Key]
		public Guid UserGuid { get; set; }

		[ForeignKey("Role")]
		public int RoleId { get; set; }

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

		public virtual Role Role { get; set; }

		public virtual Publisher Publisher { get; set; }
	}
}