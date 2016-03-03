using System;
using System.Collections.Generic;
using GameStore.BLL.Banking;
using GameStore.BLL.DTOs;
using GameStore.BLL.Payments.Interfaces;

namespace GameStore.BLL.Services.Interfaces
{
	public interface IOrderService
	{
		/// <summary>
		/// Add game to basket.
		/// </summary>
		/// <param name="userGuid">User guid (to identify current user)</param>
		/// <param name="gameKey">Game key</param>
		/// <param name="quantity">Game quantity</param>
		void AddItemToBasket(Guid userGuid, string gameKey, int quantity);

		/// <summary>
		/// Get order details for current user.
		/// </summary>
		/// <param name="userGuid">User guid (to identify current user)</param>
		/// <returns>Order information</returns>
		OrderDto GetUserBasket(Guid userGuid);

		/// <summary>
		/// Pay order.
		/// </summary>
		/// <param name="basketGuid"></param>
		/// <param name="userGuid">Current user guid</param>
		/// <param name="paymentMethod">Payment method</param>
		/// <returns>True when order was paid</returns>
		PaymentStatus Pay(Guid basketGuid, Guid userGuid, IPaymentMethod paymentMethod);

		IEnumerable<ShipperDto> GetShippers();

		void ChangeOrderShipper(Guid userGuid, int? shipperId);

		ShipperDto GetOrderShipper(int orderId);

		IEnumerable<OrderDto> GetOrders(DateTime? startDate, DateTime? endDate);

		IEnumerable<OrderDto> GetOrders(Guid? userGuid, DateTime? startDate, DateTime? endDate);

		void ShipOrder(int orderId);

		OrderDto Get(int orderId);

		void DeleteItemFromBasket(Guid userGuid, string gameKey);
	}
}