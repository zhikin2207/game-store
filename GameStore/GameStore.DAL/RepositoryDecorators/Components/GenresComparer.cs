using System.Collections.Generic;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.RepositoryDecorators.Components
{
	public class GenresComparer : IEqualityComparer<Genre>
	{
		public bool Equals(Genre x, Genre y)
		{
			return x.Name == y.Name && x.Database == y.Database;
		}

		public int GetHashCode(Genre obj)
		{
			return obj.Name.GetHashCode();
		}
	}
}