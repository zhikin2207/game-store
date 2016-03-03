using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.ViewModelBuilders;
using GameStore.WebUI.ViewModels.Order;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.ViewModelBuilders
{
	[TestClass]
	public class OrderViewModelBuilderTest
	{
		private Mock<IOrderService> _mockOrderService;
		private OrderViewModelBuilder _orderViewModelBuilder;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockOrderService = new Mock<IOrderService>();

			PopulateOrderServiceMock();

			_orderViewModelBuilder = new OrderViewModelBuilder(_mockOrderService.Object);
		}

		#region BuildBasketViewModel method

		[TestMethod]
		public void BuildBasketViewModel_Does_Not_Return_Null()
		{
			BasketViewModel viewModel = _orderViewModelBuilder.BuildBasketViewModel(Guid.NewGuid());

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		public void BuildBasketViewModel_Maps_Order_Details_Appropriately()
		{
			BasketViewModel viewModel = _orderViewModelBuilder.BuildBasketViewModel(Guid.NewGuid());

			Assert.AreEqual("game-key-0", viewModel.OrderDetails.ElementAt(0).GameKey);
			Assert.AreEqual(0, viewModel.OrderDetails.ElementAt(0).OrderDetailsId);
			Assert.AreEqual(100, viewModel.OrderDetails.ElementAt(0).Price);
			Assert.AreEqual(10, viewModel.OrderDetails.ElementAt(0).Quantity);

			Assert.AreEqual("game-key-1", viewModel.OrderDetails.ElementAt(1).GameKey);
			Assert.AreEqual(1, viewModel.OrderDetails.ElementAt(1).OrderDetailsId);
			Assert.AreEqual(10, viewModel.OrderDetails.ElementAt(1).Price);
			Assert.AreEqual(2, viewModel.OrderDetails.ElementAt(1).Quantity);
		}

		[TestMethod]
		public void BuildBasketViewModel_Counts_Total_Price_Appropriately()
		{
			BasketViewModel viewModel = _orderViewModelBuilder.BuildBasketViewModel(Guid.NewGuid());

			Assert.AreEqual(1020, viewModel.TotalPrice);
		}

		#endregion

		#region BuildOrderViewModel method

		[TestMethod]
		public void BuildOrderViewModel_Does_Not_Return_Null()
		{
			OrderViewModel viewModel = _orderViewModelBuilder.BuildOrderViewModel(Guid.NewGuid());

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		public void BuildOrderViewModel_Maps_Order_Details_Appropriately()
		{
			OrderViewModel viewModel = _orderViewModelBuilder.BuildOrderViewModel(Guid.NewGuid());

			Assert.AreEqual("game-key-0", viewModel.OrderDetails.ElementAt(0).GameKey);
			Assert.AreEqual(0, viewModel.OrderDetails.ElementAt(0).OrderDetailsId);
			Assert.AreEqual(100, viewModel.OrderDetails.ElementAt(0).Price);
			Assert.AreEqual(10, viewModel.OrderDetails.ElementAt(0).Quantity);

			Assert.AreEqual("game-key-1", viewModel.OrderDetails.ElementAt(1).GameKey);
			Assert.AreEqual(1, viewModel.OrderDetails.ElementAt(1).OrderDetailsId);
			Assert.AreEqual(10, viewModel.OrderDetails.ElementAt(1).Price);
			Assert.AreEqual(2, viewModel.OrderDetails.ElementAt(1).Quantity);
		}

		[TestMethod]
		public void BuildOrderViewModel_Counts_Total_Price_Appropriately()
		{
			OrderViewModel viewModel = _orderViewModelBuilder.BuildOrderViewModel(Guid.NewGuid());

			Assert.AreEqual(1020, viewModel.TotalPrice);
		}

		#endregion

		#region BuildIBoxTerminalPaymentViewModel method

		[TestMethod]
		public void BuildIBoxTerminalPaymentViewModel_Does_Not_Return_Null()
		{
			IBoxTerminalPaymentViewModel viewModel = _orderViewModelBuilder.BuildIBoxTerminalPaymentViewModel(Guid.NewGuid());

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		public void BuildIBoxTerminalPaymentViewModel_Maps_Order_Appropriately()
		{
			IBoxTerminalPaymentViewModel viewModel = _orderViewModelBuilder.BuildIBoxTerminalPaymentViewModel(Guid.Empty);

			Assert.AreEqual(0, viewModel.InvoiceNumber);
			Assert.AreEqual(1020, viewModel.Sum);
			Assert.AreEqual(Guid.Empty, viewModel.BasketGuid);
		}

		[TestMethod]
		public void BuildIBoxTerminalPaymentViewModel_Counts_Sum_Appropriately()
		{
			IBoxTerminalPaymentViewModel viewModel = _orderViewModelBuilder.BuildIBoxTerminalPaymentViewModel(Guid.NewGuid());

			Assert.AreEqual(1020, viewModel.Sum);
		}

		#endregion

		#region BuildAllShippersViewModel method

		[TestMethod]
		public void BuildShippersViewModel_Does_Not_Return_Null()
		{
			_mockOrderService.Setup(m => m.GetShippers()).Returns(new[] { new ShipperDto() });

			AllShippersViewModel viewModel = _orderViewModelBuilder.BuildAllShippersViewModel(Guid.NewGuid());

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		public void BuildShippersViewModel_Maps_Shipers_Appropriately()
		{
			_mockOrderService.Setup(m => m.GetShippers()).Returns(new[]
			{
				new ShipperDto { CompanyName = "shipper-name", Phone = "shipper-phone", ShipperId = 200 }
			});

			AllShippersViewModel viewModel = _orderViewModelBuilder.BuildAllShippersViewModel(Guid.NewGuid());

			Assert.AreEqual("shipper-name", viewModel.Shippers.ElementAt(0).CompanyName);
			Assert.AreEqual("shipper-phone", viewModel.Shippers.ElementAt(0).Phone);
			Assert.AreEqual(200, viewModel.Shippers.ElementAt(0).ShipperId);
		}

		[TestMethod]
		public void BuildShippersViewModel_Maps_Selected_ShipperId_Appropriately_When_It_Is_Null()
		{
			_mockOrderService.Setup(m => m.GetShippers()).Returns(new[] { new ShipperDto { ShipperId = 200 } });
			_mockOrderService.Setup(m => m.GetUserBasket(It.IsAny<Guid>())).Returns(new OrderDto { ShipperId = null });

			AllShippersViewModel viewModel = _orderViewModelBuilder.BuildAllShippersViewModel(Guid.NewGuid());

			Assert.AreEqual(200, viewModel.SelectedShipperId);
		}

		[TestMethod]
		public void BuildShippersViewModel_Maps_Selected_ShipperId_Appropriately_When_It_Is_Not_Null()
		{
			_mockOrderService.Setup(m => m.GetShippers()).Returns(new[] { new ShipperDto { ShipperId = 200 } });
			_mockOrderService.Setup(m => m.GetUserBasket(It.IsAny<Guid>())).Returns(new OrderDto { ShipperId = 1 });

			AllShippersViewModel viewModel = _orderViewModelBuilder.BuildAllShippersViewModel(Guid.NewGuid());

			Assert.AreEqual(1, viewModel.SelectedShipperId);
		}

		#endregion

		#region RebuildOrdersHistoryViewModel method

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RebuildOrdersHistoryViewModel_Throws_ArgumentNullException_When_ViewModel_Is_Null()
		{
			_orderViewModelBuilder.RebuildOrdersHistoryViewModel(null);
		}

		[TestMethod]
		public void RebuildOrderHistoryViewModel_Maps_Orders_Appropriately()
		{
			PopulateOrders();

			var viewModel = new OrdersHistoryViewModel();
			_orderViewModelBuilder.RebuildOrdersHistoryViewModel(viewModel);

			Assert.IsNotNull(viewModel.Orders);
			Assert.AreEqual(1, viewModel.Orders.Count());
			Assert.AreEqual(1, viewModel.Orders.ElementAt(0).OrderId);
			Assert.AreEqual(3, viewModel.Orders.ElementAt(0).OrderDetails.Count());
		}

		[TestMethod]
		public void RebuildOrderHistoryViewModel_Counts_Orders_Total_Price_Correctly()
		{
			PopulateOrders();

			var viewModel = new OrdersHistoryViewModel();
			_orderViewModelBuilder.RebuildOrdersHistoryViewModel(viewModel);

			Assert.IsNotNull(viewModel.Orders);
			Assert.AreEqual(1, viewModel.Orders.Count());
			Assert.AreEqual(160, viewModel.Orders.ElementAt(0).TotalPrice);
		}

		#endregion

		private void PopulateOrderServiceMock()
		{
			_mockOrderService.Setup(m => m.GetUserBasket(It.IsAny<Guid>())).Returns(new OrderDto
			{
				BasketGuid = Guid.Empty,
				OrderId = 0,
				OrderDetails = new[]
				{
					new OrderDetailsDto { GameKey = "game-key-0", OrderDetailsId = 0, Price = 100, Quantity = 10 },
					new OrderDetailsDto { GameKey = "game-key-1", OrderDetailsId = 1, Price = 10, Quantity = 2 }
				}
			});
		}

		private void PopulateOrders()
		{
			_mockOrderService.Setup(m => m.GetOrders(It.IsAny<Guid?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns(new List<OrderDto>
			{
				new OrderDto
				{
					OrderId = 1,
					OrderDetails = new List<OrderDetailsDto>
					{
						new OrderDetailsDto { Price = 10, Quantity = 2 },
						new OrderDetailsDto { Price = 15, Quantity = 1 },
						new OrderDetailsDto { Price = 25, Quantity = 5 }
					}
				}
			});
		}
	}
}