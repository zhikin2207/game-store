using System;
using GameStore.WebUI.ViewModels.Order;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
	public interface IOrderViewModelBuilder
	{
		/// <summary>
		/// Build BasketViewModel by guid of current user.
		/// </summary>
		/// <param name="currentUser">Guid of current user</param>
		BasketViewModel BuildBasketViewModel(Guid currentUser);

		/// <summary>
		/// Build order view model.
		/// </summary>
		/// <param name="currentUser">Current user guid</param>
		/// <returns>Order view model</returns>
		OrderViewModel BuildOrderViewModel(Guid currentUser);

		/// <summary>
		/// Build IBox terminal payment view model.
		/// </summary>
		/// <param name="currentUser">Current user guid</param>
		/// <returns>IBox terminal payment view model</returns>
		IBoxTerminalPaymentViewModel BuildIBoxTerminalPaymentViewModel(Guid currentUser);

		AllShippersViewModel BuildAllShippersViewModel(Guid currentUser);
		void RebuildOrdersHistoryViewModel(OrdersHistoryViewModel ordersHistoryViewModel);
		void RebuildOrdersViewModel(OrdersHistoryViewModel ordersHistoryViewModel);
	}
}