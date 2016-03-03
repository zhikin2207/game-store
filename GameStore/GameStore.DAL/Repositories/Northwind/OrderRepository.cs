using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Repositories.Northwind.Interfaces;
using GameStore.Domain;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind
{
	public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
	{
		public OrderRepository(BaseDataContext context)
			: base(context)
		{
		}

		public IEnumerable<Order> GetOrdersHistory(DateTime startDate, DateTime endDate)
		{
			return GetList(order => order.OrderDate.HasValue && order.OrderDate >= startDate && order.OrderDate <= endDate);
		}

		public virtual IEnumerable<Order> GetOrdersHistory(DateTime? startDate, DateTime? endDate)
		{
			IEnumerable<Order> orders = GetList(order => order.OrderDate.HasValue);

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