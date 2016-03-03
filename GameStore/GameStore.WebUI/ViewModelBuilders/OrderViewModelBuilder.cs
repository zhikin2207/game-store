using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Order;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class OrderViewModelBuilder : IOrderViewModelBuilder
	{
		private const int OrderHistoryDefaultDaysCount = 31;

		private readonly IOrderService _orderService;

		public OrderViewModelBuilder(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public BasketViewModel BuildBasketViewModel(Guid currentUser)
		{
			OrderDto basket = _orderService.GetUserBasket(currentUser);
			var orderDetails = Mapper.Map<IEnumerable<OrderDetailsViewModel>>(basket.OrderDetails);

			return new BasketViewModel
			{
				OrderDetails = orderDetails,
				TotalPrice = orderDetails.Sum(d => d.Price * d.Quantity)
			};
		}

		public OrderViewModel BuildOrderViewModel(Guid currentUser)
		{
			OrderDto basket = _orderService.GetUserBasket(currentUser);
			var orderDetails = Mapper.Map<IEnumerable<OrderDetailsViewModel>>(basket.OrderDetails);
			var shipper = _orderService.GetOrderShipper(basket.OrderId);

			return new OrderViewModel
			{
				OrderDetails = orderDetails,
				TotalPrice = orderDetails.Sum(d => d.Price * d.Quantity),
				ShipperName = shipper == null ? string.Empty : shipper.CompanyName
			};
		}

		public IBoxTerminalPaymentViewModel BuildIBoxTerminalPaymentViewModel(Guid basketGuid)
		{
			OrderDto basket = _orderService.GetUserBasket(basketGuid);

			return new IBoxTerminalPaymentViewModel
			{
				BasketGuid = basketGuid,
				InvoiceNumber = basket.OrderId,
				Sum = basket.OrderDetails.Sum(d => d.Price * d.Quantity)
			};
		}

		public AllShippersViewModel BuildAllShippersViewModel(Guid currentUser)
		{
			OrderDto basket = _orderService.GetUserBasket(currentUser);

			IEnumerable<ShipperDto> shipers = _orderService.GetShippers();
			
			return new AllShippersViewModel
			{
				Shippers = Mapper.Map<IEnumerable<ShipperViewModel>>(shipers),
				SelectedShipperId = basket.ShipperId.HasValue ? basket.ShipperId.Value : shipers.First().ShipperId
			};
		}

		public void RebuildOrdersHistoryViewModel(OrdersHistoryViewModel ordersHistoryViewModel)
		{
			if (ordersHistoryViewModel == null)
			{
				throw new ArgumentNullException("ordersHistoryViewModel");
			}

			if (!ordersHistoryViewModel.StartDate.HasValue && !ordersHistoryViewModel.EndDate.HasValue)
			{
				ordersHistoryViewModel.EndDate = DateTime.UtcNow.AddDays(-(OrderHistoryDefaultDaysCount + 1));
			}

			IEnumerable<OrderDto> orderDTOs = _orderService.GetOrders(
				ordersHistoryViewModel.UserGuid,
				ordersHistoryViewModel.StartDate, 
				ordersHistoryViewModel.EndDate);

			var orderViewModels = Mapper.Map<IEnumerable<OrderViewModel>>(orderDTOs);

			ordersHistoryViewModel.Orders = orderViewModels;

			foreach (OrderViewModel order in orderViewModels)
			{
				order.TotalPrice = order.OrderDetails.Sum(details => details.Price * details.Quantity);
			}
		}

		public void RebuildOrdersViewModel(OrdersHistoryViewModel ordersHistoryViewModel)
		{
			if (ordersHistoryViewModel == null)
			{
				throw new ArgumentNullException("ordersHistoryViewModel");
			}

			if (!ordersHistoryViewModel.StartDate.HasValue && !ordersHistoryViewModel.EndDate.HasValue)
			{
				ordersHistoryViewModel.StartDate = DateTime.UtcNow.AddDays(-(OrderHistoryDefaultDaysCount + 1));
				ordersHistoryViewModel.EndDate = DateTime.UtcNow;
			}

			IEnumerable<OrderDto> orderDTOs = _orderService.GetOrders(
				ordersHistoryViewModel.StartDate,
				ordersHistoryViewModel.EndDate);

			var orderViewModels = Mapper.Map<IEnumerable<OrderViewModel>>(orderDTOs);

			ordersHistoryViewModel.Orders = orderViewModels;

			foreach (OrderViewModel order in orderViewModels)
			{
				order.TotalPrice = order.OrderDetails.Sum(details => details.Price * details.Quantity);
			}
		}
	}
}