using System.Web;

namespace GameStore.Tests.WebUI.Routes.HttpStubsForRouting
{
	public class StubHttpResponseForRouting : HttpResponseBase
	{
		public override string ApplyAppPathModifier(string virtualPath)
		{
			return virtualPath;
		}
	}
}