using System;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Order
{
	public class IBoxTerminalPaymentViewModel
	{
		[Display(ResourceType = typeof(Resource), Name = "IBoxTerminalPaymentViewModel_Invoice")]
		public int InvoiceNumber { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "IBoxTerminalPaymentViewModel_Guid")]
		public Guid BasketGuid { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "IBoxTerminalPaymentViewModel_Sum")]
		public decimal Sum { get; set; }
	}
}