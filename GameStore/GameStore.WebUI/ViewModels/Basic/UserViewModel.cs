using System;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
    public class UserViewModel
    {
        public Guid UserGuid { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "UserViewModel_Name")]
        public string Name { get; set; }

        public string Password { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "UserViewModel_Email")]
        public string Email { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "UserViewModel_CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        public bool IsBanned { get; set; }

        public DateTime? BanReleaseAt { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Role")]
        public RoleViewModel Role { get; set; }
    }
}