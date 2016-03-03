using System;
using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.User;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
    public interface IUserViewModelBuilder
    {
		BanUserViewModel BuildBanUserViewModel(Guid userGuid);

        IEnumerable<UserViewModel> BuildUsersViewModel();
        
		UserViewModel BuildUserViewModel(Guid userGuid);
	    
		ChangeRoleViewModel BuildChangeRoleViewModel(Guid userGuid);
	    UserPublisherViewModel BuildUserPublisherViewModel(Guid userGuid);
    }
}