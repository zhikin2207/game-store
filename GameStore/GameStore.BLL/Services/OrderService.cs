using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.Banking;
using GameStore.BLL.DTOs;
using GameStore.BLL.Payments.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.NorthwindDb;
using Order = GameStore.Domain.GameStoreDb.Entities.Order;

namespace GameStore.BLL.Services
{
	public class OrderService : IOrderService
	{
		private readonly IGameStoreUnitOfWork _unitOfWork;

		public OrderService(IGameStoreUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<OrderDto> GetOrders(DateTime? startDate, DateTime? endDate)
		{
			IEnumerable<Order> orders;

			if (startDate.HasValue && endDate.HasValue)
			{
				orders = _unitOfWork.OrderRepository.GetOrdersHistory(startDate.Value, endDate.Value);
			}
			else
			{
				orders = _unitOfWork.OrderRepository.GetList();
			}

			return Mapper.Map<IEnumerable<OrderDto>>(orders);
		}

		public IEnumerable<OrderDto> GetOrders(Guid? userGuid, DateTime? startDate, DateTime? endDate)
		{
			IEnumerable<Order> orders = _unitOfWork.OrderRepository.GetOrdersHistory(userGuid, startDate, endDate);

			return Mapper.Map<IEnumerable<OrderDto>>(orders);
		}

		public void ShipOrder(int orderId)
		{
			Order order = _unitOfWork.OrderRepository.Get(orderId);

			order.Status = OrderStatus.Shipped;
			order.ShippedDate = DateTime.UtcNow;

			_unitOfWork.OrderRepository.Update(order);
			_unitOfWork.Save();
		}

		public OrderDto Get(int orderId)
		{
			Order order = _unitOfWork.OrderRepository.Get(orderId);
			return Mapper.Map<OrderDto>(order);
		}

		public void AddItemToBasket(Guid userGuid, string gameKey, int quantity)
		{
			Order basket = InitializeUserBasket(userGuid);

			OrderDetails orderItem = basket.OrderDetails.FirstOrDefault(order => order.GameKey == gameKey);

			if (orderItem != null)
			{
				// update quantity when such game alreay is in basket
				orderItem.Quantity += quantity;
			}
			else
			{
				orderItem = GetOrderDetails(gameKey, quantity);
				basket.OrderDetails.Add(orderItem);
			}

			_unitOfWork.OrderRepository.Update(basket);
			_unitOfWork.Save();
		}

		public void DeleteItemFromBasket(Guid userGuid, string gameKey)
		{
			Order basket = InitializeUserBasket(userGuid);

			OrderDetails orderItem = basket.OrderDetails.FirstOrDefault(order => order.GameKey == gameKey);

			if (orderItem != null)
			{
				basket.OrderDetails.Remove(orderItem);

				_unitOfWork.OrderRepository.Update(basket);
				_unitOfWork.Save();
			}
		}

		public OrderDto GetUserBasket(Guid userGuid)
		{
			Order basket = InitializeUserBasket(userGuid);
			return Mapper.Map<OrderDto>(basket);
		}

		public PaymentStatus Pay(Guid basketGuid, Guid userGuid, IPaymentMethod paymentMethod)
		{
			if (_unitOfWork.OrderRepository.HasBasket(basketGuid))
			{
				Order basket = _unitOfWork.OrderRepository.GetUserBasket(basketGuid);
			
				PaymentStatus status = paymentMethod.Pay(basket);
					
				if (status == PaymentStatus.Success)
				{
					basket.Status = OrderStatus.Paid;
					basket.OrderDate = DateTime.UtcNow;
					basket.UserGuid = userGuid;

					_unitOfWork.OrderRepository.Update(basket);
					_unitOfWork.Save();

					return status;
				}
			}

			return PaymentStatus.PaymentFailed;
		}

		public IEnumerable<ShipperDto> GetShippers()
		{
			IEnumerable<Shipper> shippers = _unitOfWork.ShipperRepository.GetList();
			return Mapper.Map<IEnumerable<ShipperDto>>(shippers);
		}

		public void ChangeOrderShipper(Guid userGuid, int? shipperId)
		{
			if (_unitOfWork.OrderRepository.HasBasket(userGuid))
			{
				Order basket = _unitOfWork.OrderRepository.GetUserBasket(userGuid);

				basket.ShipperId = shipperId;
				_unitOfWork.OrderRepository.Update(basket);
				_unitOfWork.Save();
			}
		}

		public ShipperDto GetOrderShipper(int orderId)
		{
			Order basket = _unitOfWork.OrderRepository.Get(orderId);

			if (!basket.ShipperId.HasValue)
			{
				return null;
			}

			Shipper shipper = _unitOfWork.ShipperRepository.Get(basket.ShipperId.Value);
			return Mapper.Map<ShipperDto>(shipper);
		}

		private Order InitializeUserBasket(Guid userGuid)
		{
			try
			{
				return _unitOfWork.OrderRepository.GetUserBasket(userGuid);
			}
			catch (InvalidOperationException)
			{
				var order = new Order
				{
					BasketGuid = userGuid,
					Status = OrderStatus.NotPaid,
					OrderDetails = new List<OrderDetails>()
				};

				_unitOfWork.OrderRepository.Add(order);
				_unitOfWork.Save();

				return order;
			}
		}

		private OrderDetails GetOrderDetails(string key, int quantity)
		{
			Game game = _unitOfWork.GameRepository.Get(key);

			var details = new OrderDetails
			{
				GameKey = key,
				Quantity = quantity,
				Price = game.Price
			};

			return details;
		}
	}
}
