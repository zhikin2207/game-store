using System.Collections.Generic;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.RepositoryDecorators.Components
{
	public class GameComparer : IEqualityComparer<Game>
	{
		public bool Equals(Game x, Game y)
		{
			return x.Key == y.Key &&
			       x.Database == y.Database &&
			       x.Name == y.Name &&
			       x.Price == y.Price &&
			       x.UnitsInStock == y.UnitsInStock;
		}

		public int GetHashCode(Game obj)
		{
			return obj.Key.GetHashCode();
		}
	}
}