using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.User;

namespace GameStore.WebUI.Controllers
{
	[AuthorizeUser(UserRole = UserRole.Administrator)]
	public class UserController : BaseController
	{
		private readonly IUserService _userService;
		private readonly IUserViewModelBuilder _userViewModelBuilder;

		public UserController(
			IUserService userService, 
			IUserViewModelBuilder userViewModelBuilder,
			ILoggingService loggingService)
			: base(loggingService)
		{
			_userService = userService;
			_userViewModelBuilder = userViewModelBuilder;
		}

		[HttpGet]
		public ActionResult Users()
		{
			IEnumerable<UserViewModel> users = _userViewModelBuilder.BuildUsersViewModel();
			return View(users);
		}

		[HttpGet]
		public ActionResult Details(Guid userGuid)
		{
			UserViewModel userViewModel = _userViewModelBuilder.BuildUserViewModel(userGuid);
			return View(userViewModel);
		}

		[HttpGet]
		public ActionResult ChangeRole(Guid userGuid)
		{
			ChangeRoleViewModel changeRoleViewModel = _userViewModelBuilder.BuildChangeRoleViewModel(userGuid);
			return View(changeRoleViewModel);
		}

		[HttpPost]
		public ActionResult ChangeRole(ChangeRoleViewModel changeRoleViewModel)
		{
			_userService.ChangeUserRole(changeRoleViewModel.User.UserGuid, changeRoleViewModel.SelectedRoleId);

			return RedirectToAction("Details", new { changeRoleViewModel.User.UserGuid });
		}

		[HttpPost]
		public ActionResult Delete(Guid userGuid)
		{
			_userService.Delete(userGuid);
			return RedirectToAction("Users");
		}

		[HttpGet]
		[AuthorizeUser(UserRole = UserRole.Administrator | UserRole.Moderator)]
		public ActionResult Ban(Guid userGuid)
		{
			BanUserViewModel viewModel = _userViewModelBuilder.BuildBanUserViewModel(userGuid);
			return View(viewModel);
		}

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.Administrator | UserRole.Moderator)]
		public ActionResult Ban(BanUserViewModel viewModel)
		{
			_userService.Ban(viewModel.User.UserGuid, viewModel.SelectedBanOption);
			return RedirectToAction("Users", "User");
		}

		[HttpPost]
		public ActionResult Unban(Guid userGuid)
		{
			_userService.Unban(userGuid);
			return RedirectToAction("Details", new { userGuid });
		}

		[HttpGet]
		public ActionResult ChangePublisher(Guid userGuid)
		{
			UserPublisherViewModel userPublisherViewModel = _userViewModelBuilder.BuildUserPublisherViewModel(userGuid);
			return View(userPublisherViewModel);
		}

		[HttpPost]
		public ActionResult ChangePublisher(UserPublisherViewModel userPublisherViewModel)
		{
			_userService.ChangeUserPublisher(userPublisherViewModel.User.UserGuid, userPublisherViewModel.CurrentCompanyName);
			return RedirectToAction("Users");
		}
	}
}
