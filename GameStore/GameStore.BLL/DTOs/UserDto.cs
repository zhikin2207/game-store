using System;

namespace GameStore.BLL.DTOs
{
	public class UserDto
	{
		public Guid UserGuid { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public DateTime? CreatedAt { get; set; }

		public bool IsAuthenticated { get; set; }

        public bool IsBanned { get; set; }

        public DateTime? BanReleaseAt { get; set; }

		public string Language { get; set; }

		public RoleDto Role { get; set; }

		public virtual PublisherDto Publisher { get; set; }
	}
}