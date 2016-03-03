﻿using System;
﻿using System.Web.Http;
﻿using System.Web.Http.Controllers;
﻿using GameStore.WebUI.Authentication;
﻿using GameStore.WebUI.Authentication.Components;

namespace GameStore.WebUI.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class AuthorizeAPIUserAttribute : AuthorizeAttribute
	{
		public UserRole UserRole { get; set; }

		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var context = (ApiController)actionContext.ControllerContext.Controller;
			return ((UserProvider)context.User).IsInRole(UserRole);
		}
	}
}