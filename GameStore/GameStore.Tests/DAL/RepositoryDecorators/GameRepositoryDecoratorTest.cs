using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Mappings;
using GameStore.DAL.RepositoryDecorators;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.NorthwindDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.DAL.RepositoryDecorators
{
	[TestClass]
	public class GameRepositoryDecoratorTest
	{
		private GameRepositoryDecorator _gameRepositoryDecorator;
		private Mock<BaseDataContext> _mockGameStoreContext;
		private Mock<BaseDataContext> _mockNorthwindContext;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingDalProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockGameStoreContext = new Mock<BaseDataContext>();
			_mockNorthwindContext = new Mock<BaseDataContext>();

			ICollection<Game> games = GetGames().ToList();
			IEnumerable<Product> products = GetProducts();
			IEnumerable<Publisher> publishers = GetPublishers();
			IEnumerable<Genre> genres = GetGenres();

			_mockGameStoreContext.Setup(m => m.GetEntity<Game>()).Returns(GetGamesMock(games).Object);
			_mockGameStoreContext.Setup(m => m.GetEntity<Publisher>()).Returns(GetPublishersMock(publishers).Object);
			_mockGameStoreContext.Setup(m => m.GetEntity<Genre>()).Returns(GetGenresMock(genres).Object);
			_mockGameStoreContext.Setup(m => m.SetDeleted(It.IsAny<Game>())).Callback<Game>(item => games.Remove(item));

			_mockNorthwindContext.Setup(m => m.GetEntity<Product>()).Returns(GetProductsMock(products).Object);

			_gameRepositoryDecorator = new GameRepositoryDecorator(
				_mockGameStoreContext.Object,
				_mockNorthwindContext.Object);
		}

		#region Count method

		[TestMethod]
		public void Count_Synchronizes_Both_Databases()
		{
			int count = _gameRepositoryDecorator.Count();

			Assert.AreEqual(6, count);
		}

		[TestMethod]
		public void Count_Calls_Delete_When_Game_Present_In_GameStore_And_Absent_In_Northwind()
		{
			_gameRepositoryDecorator.Count();

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Game>()), Times.Once);
		}

		[TestMethod]
		public void Count_With_Filters_Synchronizes_Both_Databases()
		{
			int count = _gameRepositoryDecorator.Count(new Func<Game, bool>[] { game => game.UnitsInStock == 1 });

			Assert.AreEqual(3, count);
		}

		#endregion

		#region GetList method

		[TestMethod]
		public void GetList_Synchronizes_Both_Databases()
		{
			IEnumerable<Game> games = _gameRepositoryDecorator.GetList();

			Assert.AreEqual(6, games.Count());
		}

		[TestMethod]
		public void GetList_Calls_Delete_When_Game_Present_In_GameStore_And_Absent_In_Northwind()
		{
			_gameRepositoryDecorator.GetList();

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Game>()), Times.Once);
		}

		[TestMethod]
		public void GetList_Calls_Delete_When_Game_In_Northwind_Updated()
		{
			ICollection<Game> games = GetGames().ToList();
			games.Add(new Game
			{
				Key = "4",
				Database = DatabaseName.Northwind,
				Name = "product-4",
				Price = 400,
				UnitsInStock = 4
			});
			_mockGameStoreContext.Setup(m => m.GetEntity<Game>()).Returns(GetGamesMock(games).Object);

			_gameRepositoryDecorator = new GameRepositoryDecorator(
				_mockGameStoreContext.Object,
				_mockNorthwindContext.Object);

			_gameRepositoryDecorator.GetList();

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Game>()), Times.Exactly(2));
		}

		[TestMethod]
		public void GetList_With_Filters_Calls_Delete_When_Game_Present_In_GameStore_And_Absent_In_Northwind()
		{
			_gameRepositoryDecorator.GetList(game => game.UnitsInStock == 1);

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Game>()), Times.Once);
		}

		[TestMethod]
		public void GetList_With_Filters_Calls_Delete_When_Game_In_Northwind_Updated()
		{
			ICollection<Game> games = GetGames().ToList();
			games.Add(new Game
			{
				Key = "4",
				Database = DatabaseName.Northwind,
				Name = "product-4",
				Price = 400,
				UnitsInStock = 4
			});
			_mockGameStoreContext.Setup(m => m.GetEntity<Game>()).Returns(GetGamesMock(games).Object);

			_gameRepositoryDecorator = new GameRepositoryDecorator(
				_mockGameStoreContext.Object,
				_mockNorthwindContext.Object);

			_gameRepositoryDecorator.GetList(game => game.UnitsInStock == 4);

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Game>()), Times.Exactly(2));
		}

		[TestMethod]
		public void GetList_Synchronizes_Both_Databases_With_Sorting_By_Name()
		{
			IEnumerable<Game> games = _gameRepositoryDecorator.GetList(
				new Func<Game, bool>[] { game => true },
				game => game.Name,
				true);

			Assert.AreEqual(6, games.Count());

			for (int i = 0; i < games.Count() - 1; i++)
			{
				Assert.IsTrue(string.Compare(games.ElementAt(i).Name, games.ElementAt(i + 1).Name, StringComparison.Ordinal) < 0);
			}
		}

		[TestMethod]
		public void GetList_Synchronizes_Both_Databases_With_Sorting_By_Name_And_Paging()
		{
			IEnumerable<Game> games = _gameRepositoryDecorator.GetList(
				new Func<Game, bool>[] { game => true },
				game => game.Name,
				true,
				1,
				5);

			Assert.AreEqual(5, games.Count());

			for (int i = 0; i < games.Count() - 1; i++)
			{
				Assert.IsTrue(string.Compare(games.ElementAt(i).Name, games.ElementAt(i + 1).Name, StringComparison.Ordinal) < 0);
			}
		}

		#endregion

		#region IsExist method

		[TestMethod]
		public void IsExist_Returns_True_When_Game_From_GameStore()
		{
			bool isExist = _gameRepositoryDecorator.IsExist("game-1");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_True_When_Game_From_Northwind()
		{
			bool isExist = _gameRepositoryDecorator.IsExist("2");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_False_When_Game_Absent_In_Both_Databases()
		{
			bool isExist = _gameRepositoryDecorator.IsExist("100");

			Assert.IsFalse(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_False_When_Game_Absent_In_Northwind_And_Present_In_GameStore()
		{
			bool isExist = _gameRepositoryDecorator.IsExist("5");

			Assert.IsFalse(isExist);
		}

		[TestMethod]
		public void IsExist_Calls_Delete_When_Game_Absent_In_Northwind_And_Present_In_GameStore()
		{
			_gameRepositoryDecorator.IsExist("4");

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Game>()), Times.Once);
		}

		[TestMethod]
		public void IsExist_Returns_True_When_Game_From_GameStore_With_Filter()
		{
			bool isExist = _gameRepositoryDecorator.IsExist(game => game.Key == "game-1");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_True_When_Game_From_Northwind_With_Filter()
		{
			bool isExist = _gameRepositoryDecorator.IsExist(game => game.Key == "2");

			Assert.IsTrue(isExist);
		}

		#endregion

		#region Get method

		[TestMethod]
		public void Get_Returns_Game_From_GameStore()
		{
			Game game = _gameRepositoryDecorator.Get("game-3");

			Assert.AreEqual("game-3", game.Name);
			Assert.AreEqual(300, game.Price);
			Assert.AreEqual(3, game.UnitsInStock);
			Assert.AreEqual(DatabaseName.GameStore, game.Database);
		}

		[TestMethod]
		public void Get_Returns_Game_From_Northwind()
		{
			Game game = _gameRepositoryDecorator.Get("4");

			Assert.AreEqual("product-4", game.Name);
			Assert.AreEqual(400, game.Price);
			Assert.AreEqual(1, game.UnitsInStock);
			Assert.AreEqual(DatabaseName.Northwind, game.Database);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Get_Throws_Exception_When_Game_Deleted_From_Northwind()
		{
			_gameRepositoryDecorator.Get("5");
		}

		[TestMethod]
		public void Get_Returns_Game_From_GameStore_With_Filter()
		{
			Game game = _gameRepositoryDecorator.Get(g => g.Key == "game-3");

			Assert.AreEqual("game-3", game.Name);
			Assert.AreEqual(300, game.Price);
			Assert.AreEqual(3, game.UnitsInStock);
			Assert.AreEqual(DatabaseName.GameStore, game.Database);
		}

		[TestMethod]
		public void Get_Returns_Game_From_Northwind_With_Filter()
		{
			Game game = _gameRepositoryDecorator.Get(g => g.Key == "4");

			Assert.AreEqual("product-4", game.Name);
			Assert.AreEqual(400, game.Price);
			Assert.AreEqual(1, game.UnitsInStock);
			Assert.AreEqual(DatabaseName.Northwind, game.Database);
		}

		#endregion

		#region GetGameGenres method

		[TestMethod]
		public void GetGameGenres_Returns_Genres_From_GameStore()
		{
			IEnumerable<Genre> genres = _gameRepositoryDecorator.GetGameGenres("game-3");

			Assert.AreEqual(3, genres.Count());
			Assert.AreEqual("genre-1", genres.ElementAt(0).Name);
			Assert.AreEqual("genre-2", genres.ElementAt(1).Name);
			Assert.AreEqual("genre-3", genres.ElementAt(2).Name);
		}

		[TestMethod]
		public void GetGameGenres_Returns_Genres_From_Northwind()
		{
			IEnumerable<Genre> genres = _gameRepositoryDecorator.GetGameGenres("4");

			Assert.AreEqual(1, genres.Count());
			Assert.AreEqual("category-4", genres.ElementAt(0).Name);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetGameGenres_Throws_Exception_When_Genres_Not_Found()
		{
			_gameRepositoryDecorator.GetGameGenres("game-5");
		}

		#endregion

		#region GetGamePlatformTypes method

		[TestMethod]
		public void GetGamePlatformTypes_Returns_Platforms_From_GameStore()
		{
			IEnumerable<PlatformType> platformTypes = _gameRepositoryDecorator.GetGamePlatformTypes("game-3");

			Assert.AreEqual(3, platformTypes.Count());
		}

		[TestMethod]
		public void GetGamePlatformTypes_Returns_Empty_List_When_Game_From_Northwind()
		{
			IEnumerable<PlatformType> platformTypes = _gameRepositoryDecorator.GetGamePlatformTypes("4");

			Assert.AreEqual(0, platformTypes.Count());
		}

		#endregion

		#region GetGamePublisher method

		[TestMethod]
		public void GetGamePublisher_Returns_Publisher_From_GameStore()
		{
			Publisher publisher = _gameRepositoryDecorator.GetGamePublisher("game-1");

			Assert.AreEqual("publisher-1", publisher.CompanyName);
		}

		[TestMethod]
		public void GetGamePublisher_Returns_Publisher_From_Northwind()
		{
			Publisher publisher = _gameRepositoryDecorator.GetGamePublisher("4");

			Assert.AreEqual("supplier-4", publisher.CompanyName);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetGamePublisher_Throws_Exception_When_Publisher_Not_Found()
		{
			_gameRepositoryDecorator.GetGamePublisher("game-5");
		}

		#endregion

		#region Test helpers

		private Mock<IDbSet<Game>> GetGamesMock(IEnumerable<Game> games)
		{
			Mock<IDbSet<Game>> dbSet = DbSetMockBuilder.BuildDbSetMock(games);

			dbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(keys => games.FirstOrDefault(g => g.Key == (string)keys[0]));

			return dbSet;
		}

		private Mock<IDbSet<Product>> GetProductsMock(IEnumerable<Product> products)
		{
			Mock<IDbSet<Product>> dbSet = DbSetMockBuilder.BuildDbSetMock(products);

			dbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(keys => products.FirstOrDefault(g => g.ProductID == (int)keys[0]));

			return dbSet;
		}

		private Mock<IDbSet<Publisher>> GetPublishersMock(IEnumerable<Publisher> publishers)
		{
			Mock<IDbSet<Publisher>> dbSet = DbSetMockBuilder.BuildDbSetMock(publishers);

			dbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(keys => publishers.FirstOrDefault(g => g.PublisherId == (int)keys[0]));

			return dbSet;
		}

		private Mock<IDbSet<Genre>> GetGenresMock(IEnumerable<Genre> genres)
		{
			Mock<IDbSet<Genre>> dbSet = DbSetMockBuilder.BuildDbSetMock(genres);

			dbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(keys => genres.FirstOrDefault(g => g.GenreId == (int)keys[0]));

			return dbSet;
		}

		private IEnumerable<Game> GetGames()
		{
			return new List<Game>
			{
				new Game
				{
					Key = "game-1",
					Database = DatabaseName.GameStore,
					Name = "game-1",
					Price = 100,
					UnitsInStock = 1,
					Publisher = new Publisher
					{
						CompanyName = "publisher-1"
					},
					PlatformTypes = new[]
					{
						new PlatformType()
					},
					Genres = new[]
					{
						new Genre { Name = "genre-1" }
					}
				},
				new Game
				{
					Key = "2",
					Database = DatabaseName.Northwind,
					Name = "product-2",
					Price = 200,
					UnitsInStock = 2,
					Publisher = new Publisher
					{
						CompanyName = "publisher-2"
					},
					Genres = new[]
					{
						new Genre { Name = "genre-1" },
						new Genre { Name = "genre-2" }
					}
				},
				new Game
				{
					Key = "game-3",
					Database = DatabaseName.GameStore,
					Name = "game-3",
					Price = 300,
					UnitsInStock = 3,
					Publisher = new Publisher
					{
						CompanyName = "publisher-3"
					},
					PlatformTypes = new[]
					{
						new PlatformType(),
						new PlatformType(),
						new PlatformType()
					},
					Genres = new[]
					{
						new Genre { Name = "genre-1" },
						new Genre { Name = "genre-2" },
						new Genre { Name = "genre-3" }
					}
				},
				new Game
				{
					Key = "5",
					Database = DatabaseName.Northwind,
					Name = "product-5",
					Price = 500,
					UnitsInStock = 5,
					Publisher = new Publisher
					{
						CompanyName = "publisher-5"
					},
					Genres = new[]
					{
						new Genre { Name = "genre-1" }
					}
				},
			};
		}

		private IEnumerable<Product> GetProducts()
		{
			return new List<Product>
			{
				new Product
				{
					ProductID = 1,
					ProductName = "product-1",
					UnitPrice = 100,
					UnitsInStock = 1,
					Supplier = new Supplier { CompanyName = "supplier-1" },
					Category = new Category { CategoryName = "category-1" }
				},
				new Product
				{
					ProductID = 2,
					ProductName = "product-2",
					UnitPrice = 200,
					UnitsInStock = 2,
					Supplier = new Supplier { CompanyName = "supplier-2" },
					Category = new Category { CategoryName = "category-2" }
				},
				new Product
				{
					ProductID = 3,
					ProductName = "product-3",
					UnitPrice = 300,
					UnitsInStock = 3,
					Supplier = new Supplier { CompanyName = "supplier-3" },
					Category = new Category { CategoryName = "category-3" }
				},
				new Product
				{
					ProductID = 4,
					ProductName = "product-4",
					UnitPrice = 400,
					UnitsInStock = 1,
					Supplier = new Supplier { CompanyName = "supplier-4" },
					Category = new Category { CategoryName = "category-4" }
				}
			};
		}

		private IEnumerable<Publisher> GetPublishers()
		{
			return new List<Publisher>
			{
				new Publisher { CompanyName = "publisher-1" },
				new Publisher { CompanyName = "publisher-2" },
				new Publisher { CompanyName = "publisher-3" }
			};
		}

		private IEnumerable<Genre> GetGenres()
		{
			return new List<Genre>
			{
				new Genre { Name = "genre-1" },
				new Genre { Name = "genre-2" },
				new Genre { Name = "genre-3" }
			};
		}

		#endregion
	}
}
