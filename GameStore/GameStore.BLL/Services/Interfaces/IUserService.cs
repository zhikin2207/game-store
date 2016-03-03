using System;
using System.Collections.Generic;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Components;

namespace GameStore.BLL.Services.Interfaces
{
	public interface IUserService
	{
		/// <summary>
		/// Ban current user.
		/// </summary>
		/// <param name="userGuid">Selected user guid</param>
		/// <param name="banOption">Ban option</param>
		void Ban(Guid userGuid, BanUserOption banOption);

		bool ValidateUser(string email, string password);

		UserDto GetUser(string email);

		bool IsUserInRole(string email, params string[] roles);

		void Create(UserDto userDto, string role);

		string NonAuthenticatedRoleName { get; }

		string NonAuthenticatedUserName { get; }
	    IEnumerable<UserDto> GetUsers();
	    UserDto GetUser(Guid userGuid);
		void Unban(Guid userGuid);
		void Delete(Guid userGuid);
		IEnumerable<RoleDto> GetRoles();
		void ChangeUserRole(Guid userGuid, int newRoleId);
		void ChangeUserLanguage(Guid userGuid, string lang);
		IEnumerable<PublisherDto> GetPublishers();
		void ChangeUserPublisher(Guid userGuid, string companyName);
	}
}