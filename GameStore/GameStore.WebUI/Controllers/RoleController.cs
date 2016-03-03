using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.Controllers
{
	[AuthorizeUser(UserRole = UserRole.Administrator)]
	public class RoleController : BaseController
	{
		private readonly IRoleService _roleService;
		private readonly IRoleViewModelBuilder _roleViewModelBuilder;

		public RoleController(
			IRoleService roleService, 
			IRoleViewModelBuilder roleViewModelBuilder, 
			ILoggingService loggingService)
			: base(loggingService)
		{
			_roleService = roleService;
			_roleViewModelBuilder = roleViewModelBuilder;
		}

		public ActionResult Roles()
		{
			IEnumerable<RoleViewModel> viewModels = _roleViewModelBuilder.BuildAllRolesViewModel();
			return View(viewModels);
		}

		[HttpGet]
		public ActionResult Details(int roleId)
		{
			RoleViewModel viewModel = _roleViewModelBuilder.BuildRoleViewModel(roleId);
			return View(viewModel);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(RoleViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(viewModel);
			}

			var roleDto = Mapper.Map<RoleDto>(viewModel);
			_roleService.Create(roleDto);

			return RedirectToAction("Roles");
		}

		[HttpGet]
		public ActionResult Update(int roleId)
		{
			RoleViewModel viewModel = _roleViewModelBuilder.BuildRoleViewModel(roleId);
			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(RoleViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(viewModel);
			}

			var roleDTO = Mapper.Map<RoleDto>(viewModel);
			_roleService.Update(roleDTO);

			return RedirectToAction("Details", new { roleId = viewModel.RoleId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int roleId)
		{
			_roleService.Delete(roleId);
			return RedirectToAction("Roles");
		}
	}
}
