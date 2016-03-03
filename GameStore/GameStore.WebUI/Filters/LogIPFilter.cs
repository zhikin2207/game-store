using System.Web.Mvc;
using GameStore.Logging;
using GameStore.Logging.Interfaces;
using Ninject;

namespace GameStore.WebUI.Filters
{
	public class LogIpFilter : ActionFilterAttribute
	{
		[Inject]
		public ILoggingService LoggingService { get; set; }

		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
			string actoinName = actionContext.ActionDescriptor.ActionName;
			string address = actionContext.HttpContext.Request.UserHostAddress;

			LoggingService.Log(
				LoggingLevel.Info, 
				"Controller: {0, 10}; Action: {1, 15}; IP: {2,17}", 
				controllerName, 
				actoinName,
				address);
		}
	}
}
