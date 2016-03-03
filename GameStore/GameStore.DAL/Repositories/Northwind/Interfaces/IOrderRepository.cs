using System;
using System.Collections.Generic;
using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.Repositories.Northwind.Interfaces
{
	public interface IOrderRepository : IGenericRepository<Order, int>
	{
		IEnumerable<Order> GetOrdersHistory(DateTime startDate, DateTime endDate);

		IEnumerable<Order> GetOrdersHistory(DateTime? startDate, DateTime? endDate);
	}
}