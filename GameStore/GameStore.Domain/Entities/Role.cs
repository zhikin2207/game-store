using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
	public class Role
	{
		[Key]
		public int RoleId { get; set; }

		[Required]
		[StringLength(int.MaxValue, MinimumLength = 3)]
		public string Name { get; set; }

        public bool IsSystem { get; set; }

		public virtual ICollection<User> Users { get; set; } 
	}
}
