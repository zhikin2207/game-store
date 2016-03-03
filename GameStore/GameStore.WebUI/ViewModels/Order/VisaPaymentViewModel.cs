using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Order
{
	public class VisaPaymentViewModel
	{
        [Required]
		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_CardHolder")]
		public string CardHolderName { get; set; }

        [Required]
		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_CardNumber")]
		[DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        
        [Required]
		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_CardMonth")]
		[DataType(DataType.Date)]
		[Range(1, 12)]
        public int ExpirationMonth { get; set; }

		[Required]
		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_CardYear")]
		[DataType(DataType.Date)]
		[Range(2000, 2030)]
		public int ExpirationYear { get; set; }

        [Required]
		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_CVV2")]
        public string CVV2 { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_Email")]
		public string Email { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "VisaPaymentViewModel_Phone")]
		public string Phone { get; set; }
	}
}