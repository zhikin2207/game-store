using System.Web.Mvc;
using GameStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class ErrorControllerTest
	{
		#region Http404 action method

		[TestMethod]
		public void Http404_Does_Not_Return_Null()
		{
			var controller = new ErrorController();

			ActionResult result = controller.Http404();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Http404_Returns_ViewResult()
		{
			var controller = new ErrorController();

			ActionResult result = controller.Http404();

			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		#endregion

		#region Http500 action method

		[TestMethod]
		public void Http500_Does_Not_Return_Null()
		{
			var controller = new ErrorController();

			ActionResult result = controller.Http500();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Http500_Returns_ViewResult()
		{
			var controller = new ErrorController();

			ActionResult result = controller.Http500();

			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		#endregion
	}
}