using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Components;
using GameStore.WebUI.ViewModels.Account;
using Resources;

namespace GameStore.WebUI.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IUserService _userService;

		public AccountController(IUserService userService, ILoggingService loggingService)
			: base(loggingService)
		{
			_userService = userService;
		}

		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel viewModel, string returnUrl)
		{
			var webSecurity = new GameStoreWebSecurity(_userService);

			if (ModelState.IsValid && webSecurity.Login(viewModel.Email, viewModel.Password, viewModel.RememberMe))
			{
				RouteData.Values["lang"] = Language.None;
				return RedirectToLocal(returnUrl);
			}

			ModelState.AddModelError(string.Empty, GlobalResource.Login_Validation);
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Logout()
		{
			var webSecurity = new GameStoreWebSecurity(_userService);

			webSecurity.Logout();

			return RedirectToAction("AllGames", "Game");
		}

		[HttpGet]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterViewModel viewModel, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var user = Mapper.Map<UserDto>(viewModel);
				user.Language = ((UserIdentity)CurrentUser.Identity).CurrentUser.Language;

				_userService.Create(user, UserRole.User.ToString());

				return RedirectToLocal(returnUrl);
			}

			ModelState.AddModelError(string.Empty, GlobalResource.Register_Validation);
			return View(viewModel);
		}

		public ActionResult ChangeLanguage(string returnUrl)
		{
			return RedirectToLocal(returnUrl);
		}

		public ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction("AllGames", "Game");
		}
	}
}
