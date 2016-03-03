using System;

namespace GameStore.WebUI.Authentication.Components
{
	/// <summary>
	/// User roles.
	/// </summary>
	[Flags]
	public enum UserRole
	{
		/// <summary>
		/// Administrator user role.
		/// </summary>
		Administrator = 32,
		
		/// <summary>
		/// Manager user role.
		/// </summary>
		Manager = 16,

		/// <summary>
		/// Publisher user role.
		/// </summary>
		Publisher = 8,

		/// <summary>
		/// Moderator user role.
		/// </summary>
		Moderator = 4,

		/// <summary>
		/// User user role.
		/// </summary>
		User = 2,

		/// <summary>
		/// Guest user role.
		/// </summary>
		Guest = 1,
	}
}