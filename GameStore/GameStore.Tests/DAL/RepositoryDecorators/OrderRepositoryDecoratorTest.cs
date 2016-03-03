using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Mappings;
using GameStore.DAL.RepositoryDecorators;
using GameStore.Domain;
using GameStore.Domain.GameStoreDb.Entities.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using GameStoreOrder = GameStore.Domain.GameStoreDb.Entities.Order;
using NorthwindOrder = GameStore.Domain.NorthwindDb.Order;

namespace GameStore.Tests.DAL.RepositoryDecorators
{
	[TestClass]
	public class OrderRepositoryDecoratorTest
	{
		private readonly Guid _user1Guid = Guid.NewGuid();
		private readonly Guid _user2Guid = Guid.NewGuid();

		private OrderRepositoryDecorator _orderRepositoryDecorator;
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

			IEnumerable<GameStoreOrder> gameStoreOrders = GetGameStoreOrders();
			IEnumerable<NorthwindOrder> northwindOrders = GetNorthwindOrders();

			_mockGameStoreContext.Setup(m => m.GetEntity<GameStoreOrder>()).Returns(GetGameStoreOrdersMock(gameStoreOrders).Object);
			_mockNorthwindContext.Setup(m => m.GetEntity<NorthwindOrder>()).Returns(GetNorthwindOrdersMock(northwindOrders).Object);

			_orderRepositoryDecorator = new OrderRepositoryDecorator(_mockGameStoreContext.Object, _mockNorthwindContext.Object);
		}

		#region GetList method

		[TestMethod]
		public void GetList_Unions_Order_From_Both_Databases()
		{
			IEnumerable<GameStoreOrder> orders = _orderRepositoryDecorator.GetList();

			Assert.AreEqual(8, orders.Count());
		}

		#endregion

		#region GetOrdersHistory method

		[TestMethod]
		public void GetOrdersHistory_Gets_Orders_From_Both_Databases_When_User_Id_Not_Set()
		{
			IEnumerable<GameStoreOrder> orders = _orderRepositoryDecorator.GetOrdersHistory(null, null, null);

			Assert.AreEqual(6, orders.Count());
		}

		[TestMethod]
		public void GetOrdersHistory_Filters_Orders_Correctly_When_User_Id_Set()
		{
			IEnumerable<GameStoreOrder> orders = _orderRepositoryDecorator.GetOrdersHistory(_user1Guid, null, null);

			Assert.AreEqual(1, orders.Count());
		}

		[TestMethod]
		public void GetOrdersHistory_Filters_Orders_Correctly_When_Start_Date_Set()
		{
			IEnumerable<GameStoreOrder> orders = _orderRepositoryDecorator.GetOrdersHistory(null, new DateTime(2003, 1, 1), null);

			Assert.AreEqual(3, orders.Count());
		}

		[TestMethod]
		public void GetOrdersHistory_Filters_Orders_Correctly_When_End_Date_Set()
		{
			IEnumerable<GameStoreOrder> orders = _orderRepositoryDecorator.GetOrdersHistory(null, null, new DateTime(2003, 1, 1));

			Assert.AreEqual(4, orders.Count());
		}

		#endregion

		#region Test helpers

		private Mock<IDbSet<GameStoreOrder>> GetGameStoreOrdersMock(IEnumerable<GameStoreOrder> orders)
		{
			return DbSetMockBuilder.BuildDbSetMock(orders);
		}

		private Mock<IDbSet<NorthwindOrder>> GetNorthwindOrdersMock(IEnumerable<NorthwindOrder> orders)
		{
			return DbSetMockBuilder.BuildDbSetMock(orders);
		}

		private IEnumerable<GameStoreOrder> GetGameStoreOrders()
		{
			return new[]
			{
				new GameStoreOrder { OrderId = 1, UserGuid = _user1Guid, OrderDate = new DateTime(2001, 1, 1), Status = OrderStatus.NotPaid },
				new GameStoreOrder { OrderId = 2, UserGuid = _user1Guid, OrderDate = new DateTime(2002, 1, 1), Status = OrderStatus.Paid },
				new GameStoreOrder { OrderId = 3, UserGuid = _user2Guid, OrderDate = new DateTime(2003, 1, 1), Status = OrderStatus.NotPaid },
				new GameStoreOrder { OrderId = 4, UserGuid = _user2Guid, OrderDate = new DateTime(2004, 1, 1), Status = OrderStatus.Paid }
			};
		}

		private IEnumerable<NorthwindOrder> GetNorthwindOrders()
		{
			return new[]
			{
				new NorthwindOrder { OrderID = 1, OrderDate = new DateTime(2001, 1, 1) },
				new NorthwindOrder { OrderID = 2, OrderDate = new DateTime(2002, 1, 1) },
				new NorthwindOrder { OrderID = 3, OrderDate = new DateTime(2003, 1, 1) },
				new NorthwindOrder { OrderID = 4, OrderDate = new DateTime(2004, 1, 1) }
			};
		}

		#endregion
	}
}