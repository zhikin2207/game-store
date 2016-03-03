using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class RoleViewModelBuilder : IRoleViewModelBuilder
	{
		private readonly IRoleService _roleService;

		public RoleViewModelBuilder(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public IEnumerable<RoleViewModel> BuildAllRolesViewModel()
		{
			IEnumerable<RoleDto> roles = _roleService.GetRoles();
			var viewModels = Mapper.Map<IEnumerable<RoleViewModel>>(roles);

			return viewModels;
		}

		public RoleViewModel BuildRoleViewModel(int roleID)
		{
			RoleDto role = _roleService.GetRole(roleID);
			var viewModel = Mapper.Map<RoleViewModel>(role);

			return viewModel;
		}
	}
}