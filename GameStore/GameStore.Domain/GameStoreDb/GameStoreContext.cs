using System.Data.Entity;
using GameStore.Domain.GameStoreDb.DbInitializers;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.Domain.GameStoreDb
{
	public class GameStoreContext : BaseDataContext
	{
		public GameStoreContext() : base("GameStoreContext")
		{
			Database.SetInitializer(new GameStoreDbAutomatedInitializer());
		}

        public virtual IDbSet<Game> Games { get; set; }

		public virtual IDbSet<Comment> Comments { get; set; }

		public virtual IDbSet<Genre> Genres { get; set; }

		public virtual IDbSet<GenreLocalization> GenreLocalizations { get; set; }

        public virtual IDbSet<PlatformType> PlatformTypes { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<OrderDetails> OrderDetails { get; set; }

        public virtual IDbSet<Publisher> Publishers { get; set; }

        public virtual IDbSet<OperationHistory> OperationHistory { get; set; }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }
	}
}
