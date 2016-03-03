using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Repositories.GameStore;
using GameStore.DAL.Repositories.Northwind;
using GameStore.DAL.RepositoryDecorators.Components;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.NorthwindDb;

namespace GameStore.DAL.RepositoryDecorators
{
	public class GenreRepositoryDecorator : GenreRepository
	{
		private readonly BaseDataContext _gameStroreContext;

		private readonly CategoryRepository _categoryRepository;

		public GenreRepositoryDecorator(BaseDataContext gameContext, BaseDataContext northwindContext)
			: base(gameContext)
		{
			_gameStroreContext = gameContext;
			_categoryRepository = new CategoryRepository(northwindContext);
		}

		public override Genre GetGenre(string name)
		{
			Sync();

			Genre genre;

			try
			{
				genre = base.GetGenre(name);
			}
			catch (InvalidOperationException)
			{
				Category category = _categoryRepository.GetCategory(name);
				genre = Mapper.Map<Genre>(category);
			}

			return genre;
		}

		public override IEnumerable<Genre> GetList()
		{
			Sync();

			IEnumerable<Genre> gameStoreGenres = base.GetList();
			IEnumerable<Genre> northwindGenres = GetNorthwindGenres();

			return gameStoreGenres.Union(northwindGenres, new GenresComparer()).ToList();
		}

		public override bool IsExist(string name)
		{
			Sync();

			bool isExist = base.IsExist(name);

			if (isExist)
			{
				return true;
			}

			return _categoryRepository.IsExist(name);
		}

		private void Sync()
		{
			IEnumerable<Genre> gameStoreGenres = GetList(g => g.Database == DatabaseName.Northwind);
			IEnumerable<Genre> northwindGenres = GetNorthwindGenres();

			IEnumerable<Genre> redundantGenres = gameStoreGenres.Except(northwindGenres, new GenresComparer());

			if (redundantGenres.Any())
			{
				foreach (Genre genre in redundantGenres)
				{
					Delete(genre);
				}

				_gameStroreContext.SaveChanges();
			}
		}

		private IEnumerable<Genre> GetNorthwindGenres()
		{
			IEnumerable<Category> categories = _categoryRepository.GetList();
			return Mapper.Map<IEnumerable<Genre>>(categories);
		}
	}
}