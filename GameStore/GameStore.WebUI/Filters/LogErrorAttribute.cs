using System.Web.Mvc;
using GameStore.Logging;
using GameStore.Logging.Interfaces;
using Ninject;

namespace GameStore.WebUI.Filters
{
	public class LogErrorAttribute : HandleErrorAttribute
	{
		public const string DefaultLogMessage = "Unhandled error";

		public const string DefaultViewName = "Http500";

		private string _logMessage;

		private string _viewName;

		[Inject]
		public ILoggingService LoggingService { get; set; }

		public LoggingLevel LogLevel { get; set; }

		public string Parameter { get; set; }

		public string LogMessage
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_logMessage))
				{
					return DefaultLogMessage;
				}

				return _logMessage;
			}

			set
			{
				_logMessage = value;
			}
		}

		public new string View
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_viewName))
				{
					return DefaultViewName;
				}

				return _viewName;
			}

			set
			{
				_viewName = value;
			}
		}

		public override void OnException(ExceptionContext filterContext)
		{
			bool isExceptionHandled = filterContext.ExceptionHandled;
			bool isExpectedException = ExceptionType.IsInstanceOfType(filterContext.Exception);

			if (isExceptionHandled || !isExpectedException)
			{
				return;
			}

			object actualParameter = null;

			if (!string.IsNullOrWhiteSpace(Parameter))
			{
				actualParameter = filterContext.RouteData.Values[Parameter];
			}

			LoggingService.Log(filterContext.Exception, LogLevel, string.Format(LogMessage, actualParameter));

			Redirect(filterContext);
		}

		private void Redirect(ExceptionContext filterContext)
		{
			filterContext.Result = new ViewResult
			{
				ViewName = View,
				MasterName = Master
			};

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = 500;
		}
	}
}