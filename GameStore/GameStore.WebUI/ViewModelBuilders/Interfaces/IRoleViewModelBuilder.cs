using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
	public interface IRoleViewModelBuilder
	{
		IEnumerable<RoleViewModel> BuildAllRolesViewModel();

		RoleViewModel BuildRoleViewModel(int roleID);
	}
}