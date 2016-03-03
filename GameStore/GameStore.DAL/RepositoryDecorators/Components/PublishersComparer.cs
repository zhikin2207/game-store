using System.Collections.Generic;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.DAL.RepositoryDecorators.Components
{
	public class PublishersComparer : IEqualityComparer<Publisher>
	{
		public bool Equals(Publisher x, Publisher y)
		{
			return x.CompanyName == y.CompanyName && x.Database == y.Database;
		}

		public int GetHashCode(Publisher obj)
		{
			return obj.CompanyName.GetHashCode();
		}
	}
}