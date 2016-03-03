using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using Resources;

namespace GameStore.WebUI.ViewModels.Account
{
    public class RegisterViewModel
    {
		[Required]
		[StringLength(20, MinimumLength = 5)]
		[Display(ResourceType = typeof(Resource), Name = "RegisterViewModel_Name")]
		public string Name { get; set; }
 
        [Required]
		[StringLength(50)]
		[DataType(DataType.EmailAddress)]
		[Display(ResourceType = typeof(Resource), Name = "RegisterViewModel_Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resource), Name = "RegisterViewModel_ConfirmPassword")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resource), Name = "RegisterViewModel_ConfirmPassword")]
		[Compare("Password", ErrorMessageResourceType= typeof(GlobalResource), ErrorMessageResourceName = "ConfirmationAttribute_PasswordValidation")]
        public string ConfirmPassword { get; set; }
    }
}