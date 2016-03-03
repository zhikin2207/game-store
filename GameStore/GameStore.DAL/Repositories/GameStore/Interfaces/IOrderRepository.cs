using System;
using System.Collections.Generic;
using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.Repositories.GameStore.Interfaces
{
	public interface IOrderRepository : IGenericRepository<Order, int>
	{
		/// <summary>
		/// Check if user has basket (order with 'NonPaid' status)
		/// </summary>
		/// <param name="userGuid">User guid</param>
		/// <returns>True when user has basket</returns>
		bool HasBasket(Guid userGuid);

		/// <summary>
		/// Get user basket.
		/// </summary>
		/// <param name="userGuid">User guid</param>
		/// <returns>User basket</returns>
		Order GetUserBasket(Guid userGuid);

		IEnumerable<Order> GetOrdersHistory(DateTime startDate, DateTime endDate);
		IEnumerable<Order> GetOrdersHistory(Guid? userGuid, DateTime? startDate, DateTime? endDate);
	}
}
