using System.Collections.Specialized;
using System.Web;

namespace GameStore.Tests.WebUI.Routes.HttpStubsForRouting
{
	public class StubHttpRequestForRouting : HttpRequestBase
	{
		private readonly string _appPath;
		private readonly string _requestUrl;

		public StubHttpRequestForRouting(string appPath, string requestUrl)
		{
			_appPath = appPath;
			_requestUrl = requestUrl;
		}

		public override string ApplicationPath
		{
			get { return _appPath; }
		}

		public override string AppRelativeCurrentExecutionFilePath
		{
			get { return _requestUrl; }
		}

		public override string PathInfo
		{
			get { return string.Empty; }
		}

		public override NameValueCollection ServerVariables
		{
			get { return new NameValueCollection(); }
		}
	}
}