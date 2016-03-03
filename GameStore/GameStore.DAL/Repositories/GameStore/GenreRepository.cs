using System.Linq;
using GameStore.DAL.Repositories.GameStore.Interfaces;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.EntitiesLocalization;

namespace GameStore.DAL.Repositories.GameStore
{
	public class GenreRepository : GenericRepository<Genre, int>, IGenreRepository
	{
		public GenreRepository(BaseDataContext context)
			: base(context)
		{
		}

		public virtual Genre GetGenre(string name)
		{
			return Get(g => g.Name == name);
		}

		public virtual bool IsExist(string name)
		{
			return IsExist(g => g.Name == name);
		}

		public override void Delete(Genre item)
		{
			DeleteGenreRecursively(item);
		}

		private void DeleteGenreRecursively(Genre genre)
		{
			if (genre.Children != null)
			{
				while (genre.Children.Any())
				{
					Genre childGenre = genre.Children.First();
					genre.Children.Remove(childGenre);
				}
			}

			if (genre.GenreLocalizations != null)
			{
				while (genre.GenreLocalizations.Any())
				{
					GenreLocalization genreLocalization = genre.GenreLocalizations.First();
					genre.GenreLocalizations.Remove(genreLocalization);
				}
			}

			base.Delete(genre);
		}
	}
}