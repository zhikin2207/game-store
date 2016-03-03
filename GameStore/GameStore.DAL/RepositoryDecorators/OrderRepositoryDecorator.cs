using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Repositories.GameStore;
using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.RepositoryDecorators
{
	public class OrderRepositoryDecorator : OrderRepository
	{
		private readonly IOrderRepository _northwindOrderRepository;

		public OrderRepositoryDecorator(BaseDataContext gameStoreContext, BaseDataContext northwindContext)
			: base(gameStoreContext)
		{
			_northwindOrderRepository = new Repositories.Northwind.OrderRepository(northwindContext);
		}

		public override IEnumerable<Order> GetList()
		{
			IEnumerable<Order> gameStoreOrders = base.GetList();
			IEnumerable<Domain.NorthwindDb.Order> northwindOrders = _northwindOrderRepository.GetList();

			var mappedOrders = Mapper.Map<IEnumerable<Order>>(northwindOrders);
			gameStoreOrders = gameStoreOrders.Union(mappedOrders);

			return gameStoreOrders;
		}

		public override IEnumerable<Order> GetOrdersHistory(Guid? userGuid, DateTime? startDate, DateTime? endDate)
		{
			IEnumerable<Order> gameStoreOrders = base.GetOrdersHistory(userGuid, startDate, endDate);

			if (!userGuid.HasValue)
			{
				IEnumerable<Domain.NorthwindDb.Order> northwindOrders = _northwindOrderRepository.GetOrdersHistory(startDate, endDate);

				var mappedOrders = Mapper.Map<IEnumerable<Order>>(northwindOrders);
				gameStoreOrders = gameStoreOrders.Union(mappedOrders);
			}

			return gameStoreOrders;
		}
	}
}