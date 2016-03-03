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
	public class GameRepositoryDecorator : GameRepository
	{
		private readonly BaseDataContext _gameStoreContext;
		private readonly BaseDataContext _northwindContext;

		private readonly ProductRepository _productRepository;

		public GameRepositoryDecorator(BaseDataContext gameStoreContext, BaseDataContext northwindContext)
			: base(gameStoreContext)
		{
			_gameStoreContext = gameStoreContext;
			_northwindContext = northwindContext;

			_productRepository = new ProductRepository(_northwindContext);
		}

		public override int Count()
		{
			Sync();

			return base.Count(g => g.Database == DatabaseName.GameStore) + _productRepository.Count();
		}

		public override int Count(IEnumerable<Func<Game, bool>> conditions)
		{
			Sync();

			ICollection<Func<Game, bool>> gameConditions = conditions.ToList();
			gameConditions.Add(g => g.Database == DatabaseName.GameStore);
			int gamesCount = base.Count(gameConditions);

			int productsCount = _productRepository.Count(GetProductConditions(conditions));

			return gamesCount + productsCount;
		}

		public override IEnumerable<Game> GetList()
		{
			Sync();

			IEnumerable<Game> gameStoreGames = base.GetList();
			IEnumerable<Game> northwindGames = GetNorthwindGames();

			gameStoreGames = gameStoreGames.Union(northwindGames, new GameComparer());

			return gameStoreGames;
		}

		public override IEnumerable<Game> GetList(Func<Game, bool> condition)
		{
			Sync();

			IEnumerable<Game> gameStoreGames = base.GetList(condition);
			IEnumerable<Game> northwindGames = GetNorthwindGames(condition);

			gameStoreGames = gameStoreGames.Union(northwindGames, new GameComparer());
			return gameStoreGames;
		}

		public override IEnumerable<Game> GetList(IEnumerable<Func<Game, bool>> conditions, Func<Game, object> sortField, bool isAscending)
		{
			Sync();

			IEnumerable<Game> gameStoreGames = base.GetList(conditions, sortField, isAscending);
			IEnumerable<Game> northwindGames = GetNorthwindGames(conditions, sortField, isAscending);

			gameStoreGames = gameStoreGames.Union(northwindGames, new GameComparer());
			gameStoreGames = isAscending ? gameStoreGames.OrderBy(sortField) : gameStoreGames.OrderByDescending(sortField);

			return gameStoreGames;
		}

		public override IEnumerable<Game> GetList(IEnumerable<Func<Game, bool>> conditions, Func<Game, object> sortField, bool isAscending, int currentPage, int itemsPerPage)
		{
			Sync();

			IEnumerable<Game> gameStoreGames = GetGames(conditions);
			IEnumerable<Game> northwindGames = GetNorthwindGames(conditions);

			gameStoreGames = gameStoreGames.Union(northwindGames, new GameComparer());
			gameStoreGames = isAscending ? gameStoreGames.OrderBy(sortField) : gameStoreGames.OrderByDescending(sortField);
			gameStoreGames = gameStoreGames.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);
			gameStoreGames = gameStoreGames.ToList();

			return gameStoreGames;
		}

		public override bool IsExist(string key)
		{
			Sync();

			if (base.IsExist(key))
			{
				return true;
			}

			int productId;
			bool isExist = int.TryParse(key, out productId) && _productRepository.IsExist(p => p.ProductID == productId);

			return isExist;
		}

		public override bool IsExist(Func<Game, bool> condition)
		{
			Sync();

			if (base.IsExist(condition))
			{
				return true;
			}

			Func<Product, bool> productCondition = p => condition(Mapper.Map<Game>(p));
			return _productRepository.IsExist(productCondition);
		}

		public override Game Get(string key)
		{
			Sync();

		    Game game;
		    
			try
		    {
		        game = base.Get(key);
		    }
		    catch (InvalidOperationException)
		    {
		        Product product = _productRepository.GetProduct(key);
		        game = Mapper.Map<Game>(product);

		        game.Publisher = GetGamePublisher(key);
		        game.Genres = GetGameGenres(key).ToList();
		    }

		    return game;
		}

		public override Game Get(Func<Game, bool> condition)
		{
			Sync();

			Game game;
			
			try
			{
				game = base.Get(condition);
			}
			catch (InvalidOperationException)
			{
				Func<Product, bool> productCondition = p => condition(Mapper.Map<Game>(p));
				Product product = _productRepository.Get(productCondition);
				game = Mapper.Map<Game>(product);

				game.Publisher = GetGamePublisher(game.Key);
				game.Genres = GetGameGenres(game.Key).ToList();
			}

			return game;
		}

		public override IEnumerable<Genre> GetGameGenres(string key)
		{
			Game game = _gameStoreContext.GetEntity<Game>().Find(key);

			if (game != null)
			{
				return game.Genres;
			}

			string genreName = _productRepository.GetProduct(key).Category.CategoryName;
			Genre genre = _gameStoreContext.GetEntity<Genre>().FirstOrDefault(g => g.Name == genreName);

			if (genre == null)
			{
				Category category = _productRepository.GetProductCategory(key);
				genre = Mapper.Map<Genre>(category);
			}

			return new[] { genre };
		}

		public override IEnumerable<PlatformType> GetGamePlatformTypes(string key)
		{
			Game game = _gameStoreContext.GetEntity<Game>().Find(key);

			if (game != null)
			{
				return game.PlatformTypes;
			}

			return Enumerable.Empty<PlatformType>();
		}

		public override Publisher GetGamePublisher(string key)
		{
			Game game = _gameStoreContext.GetEntity<Game>().Find(key);

			if (game != null)
			{
				return game.Publisher;
			}

			string companyName = _productRepository.GetProduct(key).Supplier.CompanyName;
			Publisher publisher = _gameStoreContext.GetEntity<Publisher>().FirstOrDefault(p => p.CompanyName == companyName);

			if (publisher == null)
			{
				Supplier supplier = _productRepository.GetProductSupplier(key);
				publisher = Mapper.Map<Publisher>(supplier);
			}

			return publisher;
		}

		private void Sync()
		{
			IEnumerable<Game> gameStoreGames = base.GetList(g => g.Database == DatabaseName.Northwind);
			IEnumerable<Game> northwindGames = GetNorthwindGames();

			IEnumerable<Game> redundantGames = gameStoreGames.Except(northwindGames, new GameComparer());

			if (redundantGames.Any())
			{
				foreach (Game game in redundantGames)
				{
					Delete(game);
				}

				_gameStoreContext.SaveChanges();
			}
		}

		private IEnumerable<Game> GetNorthwindGames()
		{
			IEnumerable<Product> products = _productRepository.GetList();
			var mappedGames = Mapper.Map<IEnumerable<Game>>(products);
			return mappedGames;
		}

		private IEnumerable<Game> GetNorthwindGames(IEnumerable<Func<Game, bool>> gameConditions, Func<Game, object> gameSortField, bool isAscending)
		{
			IEnumerable<Func<Product, bool>> productConditions = GetProductConditions(gameConditions);
			Func<Product, object> productSortField = product => gameSortField(Mapper.Map<Game>(product));

			IEnumerable<Product> products = _productRepository.GetList(productConditions, productSortField, isAscending);
			var mappedGames = Mapper.Map<IEnumerable<Game>>(products);

			return mappedGames;
		}

		private IEnumerable<Game> GetNorthwindGames(IEnumerable<Func<Game, bool>> gameConditions)
		{
			IEnumerable<Func<Product, bool>> productConditions = GetProductConditions(gameConditions);

			IEnumerable<Product> products = _northwindContext.GetEntity<Product>();

			foreach (Func<Product, bool> condition in productConditions)
			{
				products = products.Where(condition);
			}

			var mappedGames = Mapper.Map<IEnumerable<Game>>(products);

			return mappedGames;
		}

		private IEnumerable<Game> GetNorthwindGames(Func<Game, bool> gameCondition)
		{
			Func<Product, bool> productCondition = product => gameCondition(Mapper.Map<Game>(product));

			IEnumerable<Product> products = _northwindContext.GetEntity<Product>();

			products = products.Where(productCondition);

			var mappedGames = Mapper.Map<IEnumerable<Game>>(products);

			return mappedGames;
		}

		private IEnumerable<Func<Product, bool>> GetProductConditions(IEnumerable<Func<Game, bool>> gameConditions)
		{
			var productConditions = new List<Func<Product, bool>>();

			foreach (Func<Game, bool> condition in gameConditions)
			{
				Func<Game, bool> currentCondition = condition;
				productConditions.Add(product => currentCondition(Mapper.Map<Game>(product)));
			}

			return productConditions;
		}

		private IEnumerable<Game> GetGames(IEnumerable<Func<Game, bool>> conditions)
		{
			IEnumerable<Game> games = _gameStoreContext.GetEntity<Game>();

			foreach (Func<Game, bool> condition in conditions)
			{
				games = games.Where(condition);
			}

			return games;
		}
	}
}