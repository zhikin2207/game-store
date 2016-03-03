using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GameStore.Domain
{
	public abstract class BaseDataContext : DbContext
	{
		public BaseDataContext() : this("Default")
		{
		}

		public BaseDataContext(string connection) : base(connection)
		{
		}

		public virtual IDbSet<TEntity> GetEntity<TEntity>() where TEntity : class
		{
			return Set<TEntity>();
		}

		public virtual DbEntityEntry GetEntry(object item)
		{
			return Entry(item);
		}

		public virtual void SetModified<TEntity>(TEntity entity) where TEntity : class
		{
			Entry(entity).State = EntityState.Modified;
		}

		public virtual void SetDeleted<TEntity>(TEntity entity) where TEntity : class
		{
			Entry(entity).State = EntityState.Deleted;
		}

		public virtual void SetAdded<TEntity>(TEntity entity) where TEntity : class
		{
			Entry(entity).State = EntityState.Added;
		}
	}
}
