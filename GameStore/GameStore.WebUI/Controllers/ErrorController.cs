using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult Http404()
		{
			return View();
		}

		public ActionResult Http500()
		{
			return View();
		}
	}
}
