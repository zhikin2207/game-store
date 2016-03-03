using System.Web;

namespace GameStore.Tests.WebUI.Routes.HttpStubsForRouting
{
	public class StubHttpContextForRouting : HttpContextBase
	{
		private readonly StubHttpRequestForRouting _request;
		private readonly StubHttpResponseForRouting _response;

		public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/")
		{
			_request = new StubHttpRequestForRouting(appPath, requestUrl);
			_response = new StubHttpResponseForRouting();
		}

		public override HttpRequestBase Request
		{
			get { return _request; }
		}

		public override HttpResponseBase Response
		{
			get { return _response; }
		}
	}
}