using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.DAL.Repositories.GameStore
{
	public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
	{
		public OrderRepository(BaseDataContext context)
			: base(context)
		{
		}

		public bool HasBasket(Guid userGuid)
		{
			return IsExist(order => order.BasketGuid == userGuid && order.Status == OrderStatus.NotPaid);
		}

		public Order GetUserBasket(Guid userGuid)
		{
			return Get(order => order.BasketGuid == userGuid && order.Status == OrderStatus.NotPaid);
		}

		public virtual IEnumerable<Order> GetOrdersHistory(DateTime startDate, DateTime endDate)
		{
			return GetList(order => order.OrderDate.HasValue && order.OrderDate >= startDate && order.OrderDate <= endDate);
		}

		public virtual IEnumerable<Order> GetOrdersHistory(Guid? userGuid, DateTime? startDate, DateTime? endDate)
		{
			IEnumerable<Order> orders = GetList(order => order.OrderDate.HasValue && order.Status != OrderStatus.NotPaid);

			if (userGuid.HasValue)
			{
				orders = orders.Where(order => order.UserGuid == userGuid);
			}

			if (startDate.HasValue)
			{
				orders = orders.Where(order => order.OrderDate >= startDate.Value.Date);
			}

			if (endDate.HasValue)
			{
				orders = orders.Where(order => order.OrderDate <= endDate.Value.Date);
			}

			return orders.ToList();
		}
	}
}
