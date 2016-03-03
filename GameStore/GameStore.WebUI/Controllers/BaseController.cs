using System;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.DTOs;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Authentication.Interfaces;

namespace GameStore.WebUI.Controllers
{
	public abstract class BaseController : Controller
	{
		private const string UserCookieName = "__USER_GUID";

		protected readonly ILoggingService LoggingService;

		protected IUserProvider CurrentUser
		{
			get
			{
				return (IUserProvider)User;
			}
		}

		public Guid CurrentUserGuid
		{
			get
			{
				return ((UserIdentity)User.Identity).CurrentUser.UserGuid;
			}
		}
	
		protected BaseController(ILoggingService loggingService)
		{
			LoggingService = loggingService;
		}

		protected bool IsUserSpecificPublisher(string companyName)
		{
			var userIndentity = (UserIdentity)CurrentUser.Identity;
			PublisherDto publisher = userIndentity.CurrentUser.Publisher;

			bool userIsPublisher = CurrentUser.IsInRole(UserRole.Publisher);

			return userIsPublisher && publisher != null && publisher.CompanyName == companyName;
		}

		protected Guid InitializeBasketGuid()
		{
			Guid userGuid = GetGuidFromCookie();

			if (userGuid == Guid.Empty)
			{
				userGuid = Guid.NewGuid();
				CreateGuidCookie(userGuid);
			}

			return userGuid;
		}

		protected Guid GetGuidFromCookie()
		{
			Guid basketGuid = Guid.Empty;

			HttpCookie cookie = HttpContext.Request.Cookies[UserCookieName];

			if (cookie != null)
			{
				Guid.TryParse(cookie.Value, out basketGuid);
			}

			return basketGuid;
		}

		private void CreateGuidCookie(Guid userGuid)
		{
			var guidCookie = new HttpCookie(UserCookieName)
			{
				Value = userGuid.ToString()
			};

			HttpContext.Response.Cookies.Set(guidCookie);
		}
	}
}
