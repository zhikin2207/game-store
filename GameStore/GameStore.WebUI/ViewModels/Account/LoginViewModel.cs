using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(30)]
		[Display(ResourceType = typeof(Resource), Name = "LoginViewModel_Email")]
		public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resource), Name = "LoginViewModel_Password")]
        public string Password { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "LoginViewModel_RememberMe")]
		public bool RememberMe { get; set; }
    }
}