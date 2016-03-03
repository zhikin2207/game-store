using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GameStore.Logging;
using GameStore.WebUI.Components.AttributeAdapters;
using GameStore.WebUI.Controllers;

namespace GameStore.WebUI
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			AutoMapperConfig.RegisterMappings();
			WebApiConfig.Register(GlobalConfiguration.Configuration);

			InitializeLocalization();

			Error += Application_Error;
		}

		public static void InitializeLocalization()
		{
			ClientDataTypeModelValidatorProvider.ResourceClassKey = "GlobalResource";
			DefaultModelBinder.ResourceClassKey = "GlobalResource";

			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(LocalizedRequiredAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(LocalizedStringLengthAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RegularExpressionAttribute), typeof(LocalizedRegularExpressionAttributeAdapter));
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			HttpContext httpContext = ((MvcApplication)sender).Context;
			Exception ex = Server.GetLastError();

			LogError(httpContext, ex);

			var controller = new ErrorController();
			var routeData = new RouteData();
			var action = "Http500";

			var httpEx = ex as HttpException;
			if (httpEx != null)
			{
				switch (httpEx.GetHttpCode())
				{
					case 404:
						action = "Http404";
						break;
				}
			}

			httpContext.ClearError();
			httpContext.Response.Clear();
			httpContext.Response.StatusCode = httpEx != null ? httpEx.GetHttpCode() : 500;
			httpContext.Response.TrySkipIisCustomErrors = true;

			routeData.Values["controller"] = "Error";
			routeData.Values["action"] = action;

			((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
		}

		private void LogError(HttpContext httpContext, Exception exception)
		{
			var currentController = " ";
			var currentAction = " ";
			var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

			if (currentRouteData != null)
			{
				if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
				{
					currentController = currentRouteData.Values["controller"].ToString();
				}

				if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
				{
					currentAction = currentRouteData.Values["action"].ToString();
				}
			}

			var loggingService = new LoggingService("WebLogger");
			loggingService.Log(exception, LoggingLevel.Error, string.Format("Controller: {0}\nAction: {1}", currentController, currentAction));
		}
	}
}