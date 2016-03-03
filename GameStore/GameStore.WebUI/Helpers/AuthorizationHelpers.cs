using System.Web.Mvc;
using GameStore.BLL.DTOs;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;

namespace GameStore.WebUI.Helpers
{
	public static class AuthorizationHelpers
	{
		public static bool IsUserInRole(this HtmlHelper htmlHelper, UserRole role)
		{
			return ((UserProvider)htmlHelper.ViewContext.HttpContext.User).IsInRole(role);
		}

		public static bool IsUserSpecificPublisher(this HtmlHelper htmlHelper, string companyName)
		{
			var userProvider = (UserProvider)htmlHelper.ViewContext.HttpContext.User;
			var userIndentity = (UserIdentity)userProvider.Identity;
			PublisherDto publisher = userIndentity.CurrentUser.Publisher;

			bool userIsPublisher = userProvider.IsInRole(UserRole.Publisher);

			return userIsPublisher && publisher != null && publisher.CompanyName == companyName;
		}

		public static bool IsUserBanned(this HtmlHelper htmlHelper)
		{
			var userProvider = (UserProvider)htmlHelper.ViewContext.HttpContext.User;
			return userProvider.IsBanned;
		}
	}
}