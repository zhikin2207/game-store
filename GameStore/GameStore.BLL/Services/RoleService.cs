using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Services
{
	public class RoleService : IRoleService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public RoleService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<RoleDto> GetRoles()
		{
			IEnumerable<Role> roles = _unitOfWork.RoleRepository.GetList();
			var mappedRoles = Mapper.Map<IEnumerable<RoleDto>>(roles);
			return mappedRoles;
		}

		public RoleDto GetRole(int roleId)
		{
			Role role = _unitOfWork.RoleRepository.Get(roleId);
			var mappedRole = Mapper.Map<RoleDto>(role);
			return mappedRole;
		}

		public void Create(RoleDto roleDto)
		{
			var role = Mapper.Map<Role>(roleDto);

			_unitOfWork.RoleRepository.Add(role);
			_unitOfWork.Save();
		}

		public void Update(RoleDto roleDto)
		{
			Role role = _unitOfWork.RoleRepository.Get(roleDto.RoleId);

			role.Name = roleDto.Name;

			_unitOfWork.RoleRepository.Update(role);
			_unitOfWork.Save();
		}

		public void Delete(int roleId)
		{
			Role role = _unitOfWork.RoleRepository.Get(roleId);

			_unitOfWork.RoleRepository.Delete(role);
			_unitOfWork.Save();
		}
	}
}