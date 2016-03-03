using System.Data.Entity;
using GameStore.Domain.DbInitializers;
using GameStore.Domain.Entities;

namespace GameStore.Domain
{
	public class GameStoreContext : DbContext
	{
		public GameStoreContext()
			: base("GameStoreContext")
		{
			Database.SetInitializer(new GameStoreDbAutomatedInitializer());
		}

        public virtual IDbSet<Game> Games { get; set; }

		public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Genre> Genres { get; set; }

        public virtual IDbSet<PlatformType> PlatformTypes { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<OrderDetails> OrderDetails { get; set; }

        public virtual IDbSet<Publisher> Publishers { get; set; }

        public virtual IDbSet<OperationHistory> OperationHistory { get; set; }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }

	    public virtual IDbSet<TEntity> GetEntity<TEntity>() where TEntity : class
	    {
	        return Set<TEntity>();
	    }
	}
}
