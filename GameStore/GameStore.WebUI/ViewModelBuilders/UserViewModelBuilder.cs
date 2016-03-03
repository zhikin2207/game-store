using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.User;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class UserViewModelBuilder : IUserViewModelBuilder
	{
		private readonly IUserService _userService;

		public UserViewModelBuilder(IUserService userService)
		{
			_userService = userService;
		}

		public BanUserViewModel BuildBanUserViewModel(Guid userGuid)
		{
			UserDto user = _userService.GetUser(userGuid);

			return new BanUserViewModel
			{
				User = Mapper.Map<UserViewModel>(user)
			};
		}

		public IEnumerable<UserViewModel> BuildUsersViewModel()
		{
			IEnumerable<UserDto> users = _userService.GetUsers();
			return Mapper.Map<IEnumerable<UserViewModel>>(users);
		}

		public UserViewModel BuildUserViewModel(Guid userGuid)
		{
			UserDto user = _userService.GetUser(userGuid);
			return Mapper.Map<UserViewModel>(user);
		}

		public ChangeRoleViewModel BuildChangeRoleViewModel(Guid userGuid)
		{
			UserDto user = _userService.GetUser(userGuid);
			IEnumerable<RoleDto> roles = _userService.GetRoles();

			return new ChangeRoleViewModel
			{
				User = Mapper.Map<UserViewModel>(user),
				Roles = Mapper.Map<IEnumerable<RoleViewModel>>(roles),
				SelectedRoleId = user.Role.RoleId
			};
		}

		public UserPublisherViewModel BuildUserPublisherViewModel(Guid userGuid)
		{
			UserDto user = _userService.GetUser(userGuid);
			IEnumerable<PublisherDto> publishers = _userService.GetPublishers();

			return new UserPublisherViewModel
			{
				User = Mapper.Map<UserViewModel>(user),
				Publishers = Mapper.Map<IEnumerable<PublisherViewModel>>(publishers),
				CurrentCompanyName = user.Publisher != null ? user.Publisher.CompanyName : string.Empty
			};
		}
	}
}