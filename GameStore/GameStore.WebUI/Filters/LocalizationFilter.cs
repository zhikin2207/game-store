using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Components;
using Ninject;

namespace GameStore.WebUI.Filters
{
	public class LocalizationFilter : ActionFilterAttribute
	{
		private const string CultureCookieName = "__CULTURE";
		private const string RouteLangSection = "lang";

		[Inject]
		public IUserService UserService { get; set; }

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string currentLang;
			var routeLang = filterContext.RouteData.Values[RouteLangSection] as string;

			var userIdentity = (UserIdentity)filterContext.HttpContext.User.Identity;
			bool isLangNone = routeLang == Language.None || routeLang == null;

			if (isLangNone)
			{
				if (userIdentity.IsAuthenticated)
				{
					currentLang = userIdentity.CurrentUser.Language;
				}
				else
				{
					currentLang = InitCultureNameCookie(filterContext.HttpContext);
				}
			}
			else
			{
				currentLang = routeLang;
			}

			if (userIdentity.IsAuthenticated && (userIdentity.CurrentUser.Language == null || userIdentity.CurrentUser.Language != currentLang))
			{
				UserService.ChangeUserLanguage(userIdentity.CurrentUser.UserGuid, currentLang);
			}

			userIdentity.CurrentUser.Language = currentLang;

			string cultureCookieValue = GetCultureNameFromCookie(filterContext.HttpContext);

			if (cultureCookieValue != currentLang)
			{
				SetCultureNameToCookie(filterContext.HttpContext, currentLang);
			}

			filterContext.RouteData.Values[RouteLangSection] = currentLang;

			var cultureInfo = new CultureInfo(currentLang);
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);


			bool isChildAction = filterContext.IsChildAction;
			bool isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();

			if (isLangNone && !isChildAction && !isAjaxRequest)
			{
				filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
			}
		}

		private string InitCultureNameCookie(HttpContextBase httpContext)
		{
			string cultureName = GetCultureNameFromCookie(httpContext);

			bool isCookieExist = cultureName != null;
			bool isCookieValid = isCookieExist && (Language.All.Contains(cultureName) && cultureName != Language.None);

			if (!isCookieValid)
			{
				cultureName = Language.English;
				SetCultureNameToCookie(httpContext, cultureName);
			}

			return cultureName;
		}

		private string GetCultureNameFromCookie(HttpContextBase httpContext)
		{
			HttpCookie cultureCookie = httpContext.Request.Cookies[CultureCookieName];

			if (cultureCookie != null)
			{
				return cultureCookie.Value;
			}

			return null;
		}

		private void SetCultureNameToCookie(HttpContextBase httpContext, string name)
		{
			httpContext.Response.SetCookie(new HttpCookie(CultureCookieName) { Value = name });
		}
	}
}