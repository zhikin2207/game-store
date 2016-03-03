using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Mappings;
using GameStore.BLL.Payments;
using GameStore.BLL.Payments.Interfaces;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using GameStore.Domain.NorthwindDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order = GameStore.Domain.GameStoreDb.Entities.Order;

namespace GameStore.Tests.BLL.Services
{
	[TestClass]
	public class OrderServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private IOrderService _orderService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingBllProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_orderService = new OrderService(_mockUnitOfWork.Object);
		}

		#region AddItemToBasket method

		[TestMethod]
		public void AddItemToBasket_Calls_Save_Method_When_Basket_Exists()
		{
			SetupExistingUserBasket();
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>()));

			_orderService.AddItemToBasket(Guid.NewGuid(), "game-key-1", 1);

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		public void AddItemToBasket_Calls_Save_Method_Twice_When_Basket_Is_New()
		{
			SetupNewUserBasket();
			_mockUnitOfWork.Setup(m => m.GameRepository.Get(It.IsAny<string>())).Returns(new Game());
			_mockUnitOfWork.Setup(m => m.OrderRepository.Add(It.IsAny<Order>()));
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>()));

			_orderService.AddItemToBasket(Guid.NewGuid(), "game-key-1", 1);

			_mockUnitOfWork.Verify(m => m.Save(), Times.Exactly(2));
		}

		[TestMethod]
		public void AddItemToBasket_Updates_Quantity_When_Such_Item_Is_Already_In_Basket()
		{
			Order testOrder = null;
			SetupExistingUserBasket();
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>())).Callback<Order>(order =>
			{
				testOrder = order;
			});

			_orderService.AddItemToBasket(Guid.NewGuid(), "game-key-1", 1);

			Assert.IsNotNull(testOrder);
			Assert.AreEqual(2, testOrder.OrderDetails.Count);
			Assert.AreEqual(2, testOrder.OrderDetails.First().Quantity);
		}

		#endregion

		#region GetUserBasket method

		[TestMethod]
		public void GetUserBasket_Does_Not_Return_Null()
		{
			SetupExistingUserBasket();

			OrderDto order = _orderService.GetUserBasket(Guid.NewGuid());

			Assert.IsNotNull(order);
		}

		[TestMethod]
		public void GetUserBasket_Maps_OrderDetails_Appropriately()
		{
			SetupExistingUserBasket();

			OrderDto order = _orderService.GetUserBasket(Guid.NewGuid());

			Assert.AreEqual(2, order.OrderDetails.Count());
			Assert.AreEqual("game-key-1", order.OrderDetails.ElementAt(0).GameKey);
			Assert.AreEqual("game-key-2", order.OrderDetails.ElementAt(1).GameKey);
		}

		#endregion

		#region Pay method

		//[TestMethod]
		//public void Pay_Returns_False_When_User_Does_Not_Have_Basket()
		//{
		//	_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(false);

		//	bool result = _orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), new BankPayment());

		//	Assert.IsFalse(result);
		//}

		//[TestMethod]
		//public void Pay_Returns_False_When_Payment_Method_Is_Not_Set()
		//{
		//	bool result = _orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), null);

		//	Assert.IsFalse(result);
		//}

		//[TestMethod]
		//public void Pay_Returns_False_When_Payment_Method_Filed()
		//{
		//	_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);

		//	_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>()));

		//	var paymentMethod = new Mock<IPaymentMethod>();
		//	paymentMethod.Setup(m => m.Pay()).Returns(false);

		//	bool result = _orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), paymentMethod.Object);

		//	Assert.IsFalse(result);
		//}

		[TestMethod]
		public void Pay_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>())).Returns(new Order());
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>()));

			_orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), new VisaPayment());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		public void Pay_Changes_Status_Into_Paid_Using_Bank_Payment_Method()
		{
			Order testOrder = null;
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>()))
				.Returns(new Order { Status = OrderStatus.NotPaid });
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>())).Callback<Order>(order =>
			{
				testOrder = order;
			});

			_orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), new BankPayment());

			Assert.AreEqual(OrderStatus.Paid, testOrder.Status);
		}

		[TestMethod]
		public void Pay_Changes_Status_Into_Paid_Using_IBox_Payment_Method()
		{
			Order testOrder = null;
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>()))
				.Returns(new Order { Status = OrderStatus.NotPaid });
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>())).Callback<Order>(order =>
			{
				testOrder = order;
			});

			_orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), new IBoxTerminalPayment());

			Assert.AreEqual(OrderStatus.Paid, testOrder.Status);
		}

		[TestMethod]
		public void Pay_Changes_Status_Into_Paid_Using_Visa_Payment_Method()
		{
			Order testOrder = null;
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>()))
				.Returns(new Order { Status = OrderStatus.NotPaid });
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>())).Callback<Order>(order =>
			{
				testOrder = order;
			});

			_orderService.Pay(Guid.NewGuid(), Guid.NewGuid(), new VisaPayment());

			Assert.AreEqual(OrderStatus.Paid, testOrder.Status);
		}

		#endregion

		#region ShipOrder method

		[TestMethod]
		public void ShipOrder_Changes_Order_Status_Into_Shipped()
		{
			Order testOrder = null;
			_mockUnitOfWork.Setup(m => m.OrderRepository.Get(It.IsAny<int>())).Returns(new Order
			{
				Status = OrderStatus.NotPaid
			});

			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>())).Callback<Order>(order =>
			{
				testOrder = order;
			});

			_orderService.ShipOrder(1);

			Assert.AreEqual(OrderStatus.Shipped, testOrder.Status);
		}

		[TestMethod]
		public void ShipOrder_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.Get(It.IsAny<int>())).Returns(new Order());

			_orderService.ShipOrder(1);

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region GetOrders method

		[TestMethod]
		public void GetOrders_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetList()).Returns(Enumerable.Empty<Order>());

			IEnumerable<OrderDto> result = _orderService.GetOrders(null, null);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetOrders_Does_Not_Take_Into_Account_Start_And_End_Dates_When_They_Are_Null()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetList());

			_orderService.GetOrders(null, null);

			_mockUnitOfWork.Verify(m => m.OrderRepository.GetList(), Times.Once);
		}

		[TestMethod]
		public void GetOrders_Takes_Into_Account_Start_And_End_Dates()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetOrdersHistory(It.IsAny<DateTime>(), It.IsAny<DateTime>()));

			_orderService.GetOrders(DateTime.UtcNow, DateTime.UtcNow);

			_mockUnitOfWork.Verify(m => m.OrderRepository.GetOrdersHistory(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
		}

		#endregion

		#region GetShippers method

		[TestMethod]
		public void GetShippers_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.ShipperRepository.GetList()).Returns(Enumerable.Empty<Shipper>());

			IEnumerable<ShipperDto> result = _orderService.GetShippers();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetShippers_Maps_Shippers_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.ShipperRepository.GetList()).Returns(new[]
			{
				new Shipper { CompanyName = "shipper-name", ShipperID = 1, Phone = "shipper-phone" }
			});

			IEnumerable<ShipperDto> result = _orderService.GetShippers();

			Assert.IsNotNull(result);
			Assert.AreEqual("shipper-name", result.ElementAt(0).CompanyName);
			Assert.AreEqual("shipper-phone", result.ElementAt(0).Phone);
		}

		#endregion

		#region ChangeOrderShipper method

		[TestMethod]
		public void ChangeOrderShipper_Does_Nothing_When_Basket_Is_Absent()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(false);

			_orderService.ChangeOrderShipper(Guid.Empty, 0);

			_mockUnitOfWork.Verify(m => m.OrderRepository.Update(It.IsAny<Order>()), Times.Never);
		}

		[TestMethod]
		public void ChangeOrderShipper_Changes_ShipperId()
		{
			Order testOrder = null;
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>())).Returns(new Order { ShipperId = 0 });
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>())).Callback<Order>(order => testOrder = order);

			_orderService.ChangeOrderShipper(Guid.Empty, 200);

			Assert.AreEqual(200, testOrder.ShipperId);
		}

		[TestMethod]
		public void ChangeOrderShipper_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.HasBasket(It.IsAny<Guid>())).Returns(true);
			_mockUnitOfWork.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>())).Returns(new Order());
			_mockUnitOfWork.Setup(m => m.OrderRepository.Update(It.IsAny<Order>()));

			_orderService.ChangeOrderShipper(Guid.Empty, 0);

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region GetOrderShipper method

		[TestMethod]
		public void GetOrderShipper_Returns_Null_When_Shipper_Is_Absent()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.Get(It.IsAny<int>())).Returns(new Order { ShipperId = null });

			ShipperDto result = _orderService.GetOrderShipper(0);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetOrderShipper_Mapps_Shipper_Appropriately()
		{
			_mockUnitOfWork.Setup(m => m.OrderRepository.Get(It.IsAny<int>())).Returns(new Order { ShipperId = 0 });
			_mockUnitOfWork
				.Setup(m => m.ShipperRepository.Get(It.IsAny<int>()))
				.Returns(new Shipper { ShipperID = 0, CompanyName = "shipper-name", Phone = "shipper-phone" });

			ShipperDto result = _orderService.GetOrderShipper(0);

			Assert.AreEqual(0, result.ShipperId);
			Assert.AreEqual("shipper-name", result.CompanyName);
			Assert.AreEqual("shipper-phone", result.Phone);
		}

		#endregion

		#region Test helpers

		private void SetupNewUserBasket()
		{
			_mockUnitOfWork
				.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>()))
				.Throws<InvalidOperationException>();
		}

		private void SetupExistingUserBasket()
		{
			_mockUnitOfWork
				.Setup(m => m.OrderRepository.GetUserBasket(It.IsAny<Guid>()))
				.Returns(new Order
				{
					OrderDetails = new[]
					{
						new OrderDetails { GameKey = "game-key-1", Price = 100, Quantity = 1 },
						new OrderDetails { GameKey = "game-key-2", Price = 200, Quantity = 2 }
					}
				});
		}

		#endregion
	}
}