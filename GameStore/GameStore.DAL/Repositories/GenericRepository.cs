using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GameStore.DAL.Repositories.Interfaces;
using GameStore.Domain;

namespace GameStore.DAL.Repositories
{
	public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
	{
		private readonly BaseDataContext _context;
		private readonly IDbSet<TEntity> _dbSet;

		protected GenericRepository(BaseDataContext context)
		{
			_context = context;

			_dbSet = _context.GetEntity<TEntity>();
		}

		public virtual int Count()
		{
			return _dbSet.Count();
		}

		public virtual int Count(Func<TEntity, bool> condition)
		{
			return _dbSet.Count(condition);
		}

		public virtual int Count(IEnumerable<Func<TEntity, bool>> conditions)
		{
			IEnumerable<TEntity> records = _dbSet;
			foreach (Func<TEntity, bool> condition in conditions)
			{
				records = records.Where(condition);
			}

			return records.Count();
		}

		public virtual IEnumerable<TEntity> GetList()
		{
			return _dbSet.ToList();
		}

		public virtual IEnumerable<TEntity> GetList(Func<TEntity, bool> condition)
		{
			return _dbSet.Where(condition).ToList();
		}

		public virtual IEnumerable<TEntity> GetList(IEnumerable<Func<TEntity, bool>> conditions, Func<TEntity, object> sortField, bool sortOrder)
		{
			IEnumerable<TEntity> records = _dbSet;

			foreach (Func<TEntity, bool> condition in conditions)
			{
				records = records.Where(condition);
			}

			records = sortOrder ? records.OrderBy(sortField) : records.OrderByDescending(sortField);

			return records.ToList();
		}

		public virtual IEnumerable<TEntity> GetList(IEnumerable<Func<TEntity, bool>> conditions, Func<TEntity, object> sortField, bool sortOrder, int currentPage, int itemsPerPage)
		{
			IEnumerable<TEntity> records = _dbSet;

			foreach (Func<TEntity, bool> condition in conditions)
			{
				records = records.Where(condition);
			}

			records = sortOrder ? records.OrderBy(sortField) : records.OrderByDescending(sortField);

			records = records.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

			return records.ToList();
		}

		public virtual bool IsExist(TKey key)
		{
			return _dbSet.Find(key) != null;
		}

		public virtual bool IsExist(Func<TEntity, bool> condition)
		{
			return _dbSet.FirstOrDefault(condition) != null;
		}

		public virtual TEntity Get(Func<TEntity, bool> condition)
		{
			return _dbSet.First(condition);
		}

		public virtual TEntity Get(TKey key)
		{
			TEntity entity = _dbSet.Find(key);

			if (entity == null)
			{
				throw new InvalidOperationException(string.Format("Entity with key {0} does not exist", key));
			}

			return entity;
		}

		public virtual void Add(TEntity item)
		{
			_context.SetAdded(item);
		}

		public virtual void Update(TEntity item)
		{
			_context.SetModified(item);
		}

		public virtual void Delete(TEntity item)
		{
			_context.SetDeleted(item);
		}
	}
}