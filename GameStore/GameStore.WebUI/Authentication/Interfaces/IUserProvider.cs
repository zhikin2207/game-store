using System.Security.Principal;
using GameStore.WebUI.Authentication.Components;

namespace GameStore.WebUI.Authentication.Interfaces
{
	public interface IUserProvider : IPrincipal
	{
		/// <summary>
		/// Gets a value indicating whether user is banned.
		/// </summary>
		bool IsBanned { get; }

		/// <summary>
		/// Check whether user is in specified role.
		/// </summary>
		/// <param name="role">User role</param>
		/// <returns>True if user is in role</returns>
		bool IsInRole(UserRole role);
	}
}