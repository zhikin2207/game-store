using System.Web.Security;
using GameStore.BLL.Services.Interfaces;

namespace GameStore.WebUI.Authentication
{
	public class GameStoreWebSecurity
	{
		private readonly IUserService _userService;

		public GameStoreWebSecurity(IUserService userService)
		{
			_userService = userService;
		}

		public bool Login(string email, string password, bool isPersistent)
		{
			bool isSuccess = _userService.ValidateUser(email, password);

			if (isSuccess)
			{
				FormsAuthentication.SetAuthCookie(email, isPersistent);
			}

			return isSuccess;
		}

		public void Logout()
		{
			FormsAuthentication.SignOut();
		}
	}
}