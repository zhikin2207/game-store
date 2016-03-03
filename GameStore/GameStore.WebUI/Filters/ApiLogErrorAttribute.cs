using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace GameStore.WebUI.Filters
{
	public class ApiLogErrorAttribute : ExceptionFilterAttribute
	{
		public HttpStatusCode StatusCode { get; set; }

		public Type ExceptionType { get; set; }

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			bool isExpectedException = ExceptionType != null && ExceptionType.IsInstanceOfType(actionExecutedContext.Exception);

			if (!isExpectedException)
			{
				return;
			}

			var returnedExeption = new HttpResponseException(new HttpResponseMessage(StatusCode));
			returnedExeption.Response.Content = new StringContent(actionExecutedContext.Exception.Message);
			
			throw returnedExeption;
		}
	}
}