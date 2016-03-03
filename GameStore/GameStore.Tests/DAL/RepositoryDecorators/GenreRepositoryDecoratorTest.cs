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
	public class GenreRepositoryDecoratorTest
	{
		private GenreRepositoryDecorator _genreRepositoryDecorator;
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

			ICollection<Genre> genres = GetGenres().ToList();
			IEnumerable<Category> categories = GetCategories();

			_mockGameStoreContext.Setup(m => m.GetEntity<Genre>()).Returns(GetGenresMock(genres).Object);
			_mockGameStoreContext.Setup(m => m.SetDeleted(It.IsAny<Genre>())).Callback<Genre>(item => genres.Remove(item));

			_mockNorthwindContext.Setup(m => m.GetEntity<Category>()).Returns(GetCategoriesMock(categories).Object);

			_genreRepositoryDecorator = new GenreRepositoryDecorator(
				_mockGameStoreContext.Object,
				_mockNorthwindContext.Object);
		}

		#region GetGenre method

		[TestMethod]
		public void GetGenre_Gets_Genre_From_GameStore()
		{
			Genre genre = _genreRepositoryDecorator.GetGenre("A");

			Assert.AreEqual("A", genre.Name);
		}

		[TestMethod]
		public void GetGenre_Gets_Genre_From_Northwind()
		{
			Genre genre = _genreRepositoryDecorator.GetGenre("G");

			Assert.AreEqual("G", genre.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetGenre_Throws_Exception_When_Genre_Deleted_From_Northwind()
		{
			_genreRepositoryDecorator.GetGenre("B");
		}

		#endregion

		#region GetList method

		[TestMethod]
		public void GetList_Synchronizes_Both_Databases()
		{
			IEnumerable<Genre> genres = _genreRepositoryDecorator.GetList();

			Assert.AreEqual(6, genres.Count());
		}

		[TestMethod]
		public void GetList_Deletes_Genres_From_Gamestore_Which_Present_In_Northwind()
		{
			_genreRepositoryDecorator.GetList();

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Genre>()), Times.Once);
		}

		#endregion

		#region IsExist method

		[TestMethod]
		public void IsExist_Returns_True_When_Genre_From_GameStore()
		{
			bool isExist = _genreRepositoryDecorator.IsExist("A");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_True_When_Genre_From_Northwind()
		{
			bool isExist = _genreRepositoryDecorator.IsExist("F");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_False_When_Genre_Absent_In_Both_Databases()
		{
			bool isExist = _genreRepositoryDecorator.IsExist("K");

			Assert.IsFalse(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_False_When_Genre_Absent_In_Northwind_And_Present_In_GameStore()
		{
			bool isExist = _genreRepositoryDecorator.IsExist("B");

			Assert.IsFalse(isExist);
		}

		[TestMethod]
		public void IsExist_Calls_Delete_When_Genre_Absent_In_Northwind_And_Present_In_GameStore()
		{
			_genreRepositoryDecorator.IsExist("B");

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Genre>()), Times.Once);
		}

		#endregion

		#region Test helpers

		private Mock<IDbSet<Genre>> GetGenresMock(IEnumerable<Genre> genres)
		{
			return DbSetMockBuilder.BuildDbSetMock(genres);
		}

		private Mock<IDbSet<Category>> GetCategoriesMock(IEnumerable<Category> categories)
		{
			return DbSetMockBuilder.BuildDbSetMock(categories);
		}

		private IEnumerable<Genre> GetGenres()
		{
			return new List<Genre>
			{
				new Genre { GenreId = 1, Database = DatabaseName.GameStore, Name = "A" },
				new Genre { GenreId = 2, Database = DatabaseName.Northwind, Name = "B" },
				new Genre { GenreId = 3, Database = DatabaseName.GameStore, Name = "C" },
				new Genre { GenreId = 4, Database = DatabaseName.Northwind, Name = "D" }
			};
		}

		private IEnumerable<Category> GetCategories()
		{
			return new List<Category>
			{
				new Category { CategoryID = 1, CategoryName = "E" },
				new Category { CategoryID = 2, CategoryName = "F" },
				new Category { CategoryID = 3, CategoryName = "G" },
				new Category { CategoryID = 4, CategoryName = "D" }
			};
		}

		#endregion
	}
}
