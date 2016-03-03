using System.Collections.Generic;
using GameStore.BLL.DTOs;

namespace GameStore.BLL.Services.Interfaces
{
	public interface IRoleService
	{
		IEnumerable<RoleDto> GetRoles();

		RoleDto GetRole(int roleID);

		void Create(RoleDto roleDTO);

		void Update(RoleDto roleDTO);

		void Delete(int roleID);
	}
}