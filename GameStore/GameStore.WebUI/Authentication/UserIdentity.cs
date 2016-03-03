using System.Security.Principal;
using GameStore.BLL.DTOs;

namespace GameStore.WebUI.Authentication
{
	public class UserIdentity : IIdentity
	{
		private readonly UserDto _currentUser;

		public UserIdentity(UserDto currentUser)
		{
			_currentUser = currentUser;
		}

		public UserDto CurrentUser
		{
			get
			{
				return _currentUser;
			}
		}

		public string AuthenticationType
		{
			get
			{
				return typeof(UserDto).ToString();
			}
		}

		public bool IsAuthenticated
		{
			get
			{
				return _currentUser.IsAuthenticated;
			}
		}

		public string Name
		{
			get
			{
				return _currentUser.Name;
			}
		}
	}
}