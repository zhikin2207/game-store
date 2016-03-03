using System.Security.Principal;
using System.Web.Mvc;
using GameStore.WebUI.Authentication.Interfaces;
using Moq;

namespace GameStore.Tests.WebUI
{
	public static class ControllerContextMockBuilder
	{
		public static Mock<ControllerContext> GetContextMock(Mock<IUserProvider> userProviderMock)
		{
			IPrincipal user = userProviderMock.Object;

			var mock = new Mock<ControllerContext>();
			mock.SetupGet(x => x.HttpContext.User).Returns(user);
			mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(user.Identity.IsAuthenticated);

			return mock;
		}
	}
}
