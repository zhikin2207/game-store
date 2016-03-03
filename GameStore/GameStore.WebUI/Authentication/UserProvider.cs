using System;
using System.Security.Principal;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Authentication.Interfaces;

namespace GameStore.WebUI.Authentication
{
	public class UserProvider : IUserProvider
	{
		private readonly UserIdentity _userIdentity;
		private readonly IUserService _userService;

		public UserProvider(string email, IUserService userService)
		{
			_userService = userService;

			UserDto user = GetUser(email);
			_userIdentity = new UserIdentity(user);
		}

		public IIdentity Identity
		{
			get
			{
				return _userIdentity;
			}
		}

		public bool IsBanned
		{
			get
			{
				return _userIdentity.CurrentUser.IsBanned;
			}
		}

		public bool IsInRole(string role)
		{
            return _userService.IsUserInRole(_userIdentity.CurrentUser.Email, role);
		}

		public bool IsInRole(UserRole role)
		{
			string[] roles = role.ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

			return _userService.IsUserInRole(_userIdentity.CurrentUser.Email, roles);
		}

		private UserDto GetUser(string email)
		{
			UserDto user;

			try
			{
				user = _userService.GetUser(email);
			}
			catch (InvalidOperationException)
			{
				user = InitializeGuestUser();
			}
			catch (ArgumentNullException)
			{
				user = InitializeGuestUser();
			}

			return user;
		}

		private UserDto InitializeGuestUser()
		{
			return new UserDto
			{
				UserGuid = Guid.Empty,
				Name = _userService.NonAuthenticatedUserName,
				IsAuthenticated = false
			};
		}
	}
}