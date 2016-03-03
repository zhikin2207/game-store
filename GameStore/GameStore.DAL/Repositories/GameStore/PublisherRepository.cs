using System.Linq;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.DAL.Repositories.GameStore
{
	public class PublisherRepository : GenericRepository<Publisher, int>, IPublisherRepository
	{
		public PublisherRepository(BaseDataContext context)
			: base(context)
		{
		}

		public virtual Publisher GetPublisher(string company)
		{
			return Get(p => p.CompanyName == company);
		}

		public virtual bool IsExist(string company)
		{
			return IsExist(p => p.CompanyName == company);
		}

		public override void Delete(Publisher item)
		{
			DeleteGenreRecursively(item);
		}

		private void DeleteGenreRecursively(Publisher publisher)
		{
			if (publisher.PublisherLocalizations != null)
			{
				while (publisher.PublisherLocalizations.Any())
				{
					PublisherLocalization genreLocalization = publisher.PublisherLocalizations.First();
					publisher.PublisherLocalizations.Remove(genreLocalization);
				}
			}

			base.Delete(publisher);
		}
	}
}