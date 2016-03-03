using System;
using System.Collections.Generic;

namespace GameStore.DAL.Repositories.Interfaces
{
	public interface IGenericRepository<TEntity, in TKey> where TEntity : class
	{
		/// <summary>
		/// Get all entities.
		/// </summary>
		/// <returns>Entities</returns>
		IEnumerable<TEntity> GetList();

		/// <summary>
		/// Get entities by condition.
		/// </summary>
		/// <param name="condition">Where condition</param>
		/// <returns>Entities</returns>
		IEnumerable<TEntity> GetList(Func<TEntity, bool> condition);

		/// <summary>
		/// Get sorted entities acc. to conditions.
		/// </summary>
		/// <param name="conditions">Where condition</param>
		/// <param name="sortField">Order by condition</param>
		/// <param name="sortOrder">True when ascending</param>
		/// <returns>Entities</returns>
		IEnumerable<TEntity> GetList(IEnumerable<Func<TEntity, bool>> conditions, Func<TEntity, object> sortField, bool sortOrder);

		/// <summary>
		/// Get sorted entities acc. to conditions.
		/// </summary>
		/// <param name="conditions">Where condition</param>
		/// <param name="sortField">Order by condition</param>
		/// <param name="sortOrder">True when ascending</param>
		/// <param name="currentPage">Current page</param>
		/// <param name="itemsPerPage">Items per page</param>
		/// <returns>Entities</returns>
		IEnumerable<TEntity> GetList(IEnumerable<Func<TEntity, bool>> conditions, Func<TEntity, object> sortField, bool sortOrder, int currentPage, int itemsPerPage);

		/// <summary>
		/// Get single entity by condition.
		/// </summary>
		/// <param name="condition">Where condition</param>
		/// <returns>Single entity</returns>
		TEntity Get(Func<TEntity, bool> condition);

		/// <summary>
		/// Add entity.
		/// </summary>
		/// <param name="item">Entity to add</param>
		void Add(TEntity item);

		/// <summary>
		/// Update entity.
		/// </summary>
		/// <param name="item">Entity to update</param>
		void Update(TEntity item);

		/// <summary>
		/// Delete entity.
		/// </summary>
		/// <param name="item">Entity to delete</param>
		void Delete(TEntity item);

		/// <summary>
		/// Get entity by key.
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Entity</returns>
		TEntity Get(TKey key);

		/// <summary>
		/// Check whether entity exists using key.
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>True if entity exists</returns>
		bool IsExist(TKey key);

		/// <summary>
		/// Check whether entity exists using condition.
		/// </summary>
		/// <param name="condition">Conditoin</param>
		/// <returns>True if entity exists</returns>
		bool IsExist(Func<TEntity, bool> condition);

		/// <summary>
		/// Get count of items
		/// </summary>
		/// <returns>Count of entities</returns>
		int Count();

		/// <summary>
		/// Get count of items acc. to condition.
		/// </summary>
		/// <param name="condition">Where condition</param>
		/// <returns>Count of entities</returns>
		int Count(Func<TEntity, bool> condition);

		/// <summary>
		/// Get count of items acc. to conditions.
		/// </summary>
		/// <param name="conditions">Where conditions</param>
		/// <returns>Count of entities</returns>
		int Count(IEnumerable<Func<TEntity, bool>> conditions);
	}
}