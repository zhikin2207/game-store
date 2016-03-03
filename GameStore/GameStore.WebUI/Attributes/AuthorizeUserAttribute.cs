using System;
using System.Web;
using System.Web.Mvc;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;

namespace GameStore.WebUI.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class AuthorizeUserAttribute : AuthorizeAttribute
	{
		public UserRole UserRole { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return ((UserProvider)httpContext.User).IsInRole(UserRole);
		}
	}
}