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
	public class PublisherRepositoryDecoratorTest
	{
		private PublisherRepositoryDecorator _publisherRepositoryDecorator;
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

			ICollection<Publisher> publishers = GetPublishers().ToList();
			IEnumerable<Supplier> suppliers = GetSuppliers();

			_mockGameStoreContext.Setup(m => m.GetEntity<Publisher>()).Returns(GetPublishersMock(publishers).Object);
			_mockGameStoreContext.Setup(m => m.SetDeleted(It.IsAny<Publisher>())).Callback<Publisher>(item => publishers.Remove(item));

			_mockNorthwindContext.Setup(m => m.GetEntity<Supplier>()).Returns(GetSuppliersMock(suppliers).Object);

			_publisherRepositoryDecorator = new PublisherRepositoryDecorator(_mockGameStoreContext.Object, _mockNorthwindContext.Object);
		}

		#region GetList method

		[TestMethod]
		public void GetList_Synchronizes_Both_Databases()
		{
			IEnumerable<Publisher> genres = _publisherRepositoryDecorator.GetList();

			Assert.AreEqual(6, genres.Count());
		}

		[TestMethod]
		public void GetList_Deletes_Genres_From_Gamestore_Which_Present_In_Northwind()
		{
			_publisherRepositoryDecorator.GetList();

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Publisher>()), Times.Once);
		}

		#endregion

		#region GetPublisher method

		[TestMethod]
		public void GetPublisher_Gets_Genre_From_GameStore()
		{
			Publisher publisher = _publisherRepositoryDecorator.GetPublisher("A");

			Assert.AreEqual("A", publisher.CompanyName);
		}

		[TestMethod]
		public void GetPublisher_Gets_Publisher_From_Northwind()
		{
			Publisher publisher = _publisherRepositoryDecorator.GetPublisher("G");

			Assert.AreEqual("G", publisher.CompanyName);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetPublisher_Throws_Exception_When_Publisher_Deleted_From_Northwind()
		{
			Publisher publisher = _publisherRepositoryDecorator.GetPublisher("B");

			Assert.AreEqual("B", publisher.CompanyName);
		}

		#endregion

		#region IsExist method

		[TestMethod]
		public void IsExist_Returns_True_When_Publisher_From_GameStore()
		{
			bool isExist = _publisherRepositoryDecorator.IsExist("A");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_True_When_Publisher_From_Northwind()
		{
			bool isExist = _publisherRepositoryDecorator.IsExist("F");

			Assert.IsTrue(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_False_When_Publisher_Absent_In_Both_Databases()
		{
			bool isExist = _publisherRepositoryDecorator.IsExist("K");

			Assert.IsFalse(isExist);
		}

		[TestMethod]
		public void IsExist_Returns_False_When_Publisher_Absent_In_Northwind_And_Present_In_GameStore()
		{
			bool isExist = _publisherRepositoryDecorator.IsExist("B");

			Assert.IsFalse(isExist);
		}

		[TestMethod]
		public void IsExist_Calls_Delete_When_Publisher_Absent_In_Northwind_And_Present_In_GameStore()
		{
			_publisherRepositoryDecorator.IsExist("B");

			_mockGameStoreContext.Verify(m => m.SetDeleted(It.IsAny<Publisher>()), Times.Once);
		}

		#endregion

		#region Test helpers

		private Mock<IDbSet<Publisher>> GetPublishersMock(IEnumerable<Publisher> publishers)
		{
			return DbSetMockBuilder.BuildDbSetMock(publishers);
		}

		private Mock<IDbSet<Supplier>> GetSuppliersMock(IEnumerable<Supplier> suppliers)
		{
			return DbSetMockBuilder.BuildDbSetMock(suppliers);
		}

		private IEnumerable<Publisher> GetPublishers()
		{
			return new List<Publisher>
			{
				new Publisher { PublisherId = 1, Database = DatabaseName.GameStore, CompanyName = "A" },
				new Publisher { PublisherId = 2, Database = DatabaseName.Northwind, CompanyName = "B" },
				new Publisher { PublisherId = 3, Database = DatabaseName.GameStore, CompanyName = "C" },
				new Publisher { PublisherId = 4, Database = DatabaseName.Northwind, CompanyName = "D" }
			};
		}

		private IEnumerable<Supplier> GetSuppliers()
		{
			return new List<Supplier>
			{
				new Supplier { SupplierID = 1, CompanyName = "E" },
				new Supplier { SupplierID = 2, CompanyName = "F" },
				new Supplier { SupplierID = 3, CompanyName = "G" },
				new Supplier { SupplierID = 4, CompanyName = "D" }
			};
		}

		#endregion
	}
}