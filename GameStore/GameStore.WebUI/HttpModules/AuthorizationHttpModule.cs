using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Authentication;

namespace GameStore.WebUI.HttpModules
{
	public class AuthorizationHttpModule : IHttpModule
	{
		private HttpContext _httpContext;

		private IUserService _userService;

		public void Init(HttpApplication httpApplication)
		{
			_userService = DependencyResolver.Current.GetService<IUserService>();

			httpApplication.AuthenticateRequest += HttpApplication_AuthenticateRequest;
		}

		public void Dispose()
		{
		}

		private void HttpApplication_AuthenticateRequest(object sender, EventArgs e)
		{
			var httpApplication = (HttpApplication)sender;
			HttpContext httpContext = httpApplication.Context;

			_httpContext = httpContext;

			httpContext.User = GetCurrentUser();
		}

		private IPrincipal GetCurrentUser()
		{
			FormsAuthenticationTicket ticket;

			try
			{
				ticket = GetTicketFromCookie();
			}
			catch (ArgumentException)
			{
				ticket = null;
			}

			var email = string.Empty;

			if (ticket != null)
			{
				email = ticket.Name;
			}

			return new UserProvider(email, _userService);
		}

		private FormsAuthenticationTicket GetTicketFromCookie()
		{
			FormsAuthenticationTicket ticket = null;

			HttpCookie authCookie = _httpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

			if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
			{
				ticket = FormsAuthentication.Decrypt(authCookie.Value);
			}

			return ticket;
		}
	}
}