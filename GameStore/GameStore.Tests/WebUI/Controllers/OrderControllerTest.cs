using System;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.Payments.Interfaces;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Order;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class OrderControllerTest
	{
		private Mock<IOrderService> _mockOrderService;
		private Mock<ILoggingService> _mockLogger;
		private Mock<IOrderViewModelBuilder> _mockViewModelBuilder;
		private OrderController _orderController;

		[TestInitialize]
		public void InitializeTest()
		{
			_mockLogger = new Mock<ILoggingService>();
			_mockOrderService = new Mock<IOrderService>();
			_mockViewModelBuilder = new Mock<IOrderViewModelBuilder>();
			_orderController = new OrderController(_mockOrderService.Object, _mockViewModelBuilder.Object, _mockLogger.Object);

			// Stub for cookies
			var context = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Guest).Get());
			context.SetupGet(r => r.HttpContext.Request.Cookies).Returns(new HttpCookieCollection());
			context.SetupGet(r => r.HttpContext.Response.Cookies).Returns(new HttpCookieCollection());
			_orderController.ControllerContext = context.Object;
		}

		#region Basket action

		[TestMethod]
		public void UserBasket_Action_Does_Not_Return_Null()
		{
			_mockViewModelBuilder.Setup(b => b.BuildBasketViewModel(It.IsAny<Guid>())).Returns(new BasketViewModel());

			var result = _orderController.Basket() as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void UserBasket_Sets_Cookie_For_New_User()
		{
			int initialCount = _orderController.ControllerContext.HttpContext.Request.Cookies.Count;

			_orderController.Basket();
			int count = _orderController.ControllerContext.HttpContext.Response.Cookies.Count;

			Assert.IsTrue(count > initialCount);
		}

		[TestMethod]
		public void UserBasket_Gets_Existing_Cookie()
		{
			var guid = Guid.NewGuid();
			_orderController.ControllerContext.HttpContext.Request.Cookies.Add(new HttpCookie("__USER_GUID", guid.ToString()));

			Guid userGuid = Guid.Empty;
			_mockViewModelBuilder.Setup(b => b.BuildBasketViewModel(It.IsAny<Guid>())).Callback<Guid>(item => userGuid = item);

			_orderController.Basket();

			Assert.AreEqual(guid, userGuid);
		}

		#endregion

		#region Details action

		[TestMethod]
		public void Details_Action_Does_Not_Return_Null()
		{
			_mockViewModelBuilder.Setup(b => b.BuildBasketViewModel(It.IsAny<Guid>())).Returns(new BasketViewModel());

			var result = _orderController.Details() as ViewResult;

			Assert.IsNotNull(result);
		}

		#endregion

		#region Buy action

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Buy_Action_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockOrderService
				.Setup(s => s.AddItemToBasket(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()))
				.Throws<InvalidOperationException>();

			_orderController.Buy("game-key");
		}

		[TestMethod]
		public void Buy_Action_Redirects_To_AllGames_Action_After_Successfull_Buying()
		{
			var result = _orderController.Buy("game-key") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("AllGames", result.RouteValues["action"].ToString());
		}

		#endregion

		#region PayWithBank action

		//[TestMethod]
		//public void PayWithBank_Action_Returns_FileResult_After_Successful_Operation()
		//{
		//	_mockOrderService.Setup(m => m.Pay(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IPaymentMethod>())).Returns(true);

		//	ActionResult result = _orderController.PayWithBank();

		//	Assert.IsInstanceOfType(result, typeof(FileResult));
		//}

		//[TestMethod]
		//public void PayWithBank_Action_Redirects_To_Basket_After_Unsuccessful_Operation()
		//{
		//	_mockOrderService.Setup(m => m.Pay(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IPaymentMethod>())).Returns(false);

		//	var result = _orderController.PayWithBank() as RedirectToRouteResult;

		//	Assert.IsNotNull(result);
		//	Assert.AreEqual("AllGames", result.RouteValues["Action"].ToString());
		//}

		#endregion

		#region PayWithIBoxTerminal action

		[TestMethod]
		public void PayWithIBoxTerminal_Action_Does_Not_Return_Null()
		{
			_mockViewModelBuilder
				.Setup(m => m.BuildIBoxTerminalPaymentViewModel(It.IsAny<Guid>()))
				.Returns(new IBoxTerminalPaymentViewModel());

			ActionResult result = _orderController.PayWithIBoxTerminal();

			Assert.IsNotNull(result);
		}

		//[TestMethod]
		//public void PayWithIBoxTerminal_Action_Redirects_To_AllGames_After_Successful_Operation()
		//{
		//	_mockOrderService.Setup(m => m.Pay(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IPaymentMethod>())).Returns(true);

		//	var result = _orderController.PayWithIBoxTerminal(new IBoxTerminalPaymentViewModel
		//	{
		//		InvoiceNumber = 0
		//	}) as RedirectToRouteResult;

		//	Assert.IsNotNull(result);
		//	Assert.AreEqual("AllGames", result.RouteValues["Action"].ToString());
		//}

		//[TestMethod]
		//public void PayWithIBoxTerminal_Action_Redirects_To_Basket_After_Unsuccessful_Operation()
		//{
		//	_mockOrderService.Setup(m => m.Pay(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IPaymentMethod>())).Returns(false);

		//	var result = _orderController.PayWithIBoxTerminal(new IBoxTerminalPaymentViewModel
		//	{
		//		InvoiceNumber = 0
		//	}) as RedirectToRouteResult;

		//	Assert.IsNotNull(result);
		//	Assert.AreEqual("AllGames", result.RouteValues["Action"].ToString());
		//}

		#endregion

		#region PayWithVisa action

		[TestMethod]
		public void PayWithVisa_Action_Does_Not_Return_Null()
		{
			ActionResult result = _orderController.PayWithVisa();

			Assert.IsNotNull(result);
		}

		//[TestMethod]
		//public void PayWithVisa_Action_Redirects_To_AllGames_After_Successful_Operation()
		//{
		//	_mockOrderService.Setup(m => m.Pay(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IPaymentMethod>())).Returns(true);

		//	var result = _orderController.PayWithVisa(new VisaPaymentViewModel
		//	{
		//		CVV2 = "cvv2",
		//		CardHolderName = "card-holder-name",
		//		CardNumber = "card-number",
		//		ExpirationMonth = DateTime.UtcNow.Month,
		//		ExpirationYear = DateTime.UtcNow.Year
		//	}) as RedirectToRouteResult;

		//	Assert.IsNotNull(result);
		//	Assert.AreEqual("AllGames", result.RouteValues["Action"].ToString());
		//}

		//[TestMethod]
		//public void PayWithVisa_Action_Redirects_To_Basket_After_Unsuccessful_Operation()
		//{
		//	_mockOrderService.Setup(m => m.Pay(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IPaymentMethod>())).Returns(false);

		//	var result = _orderController.PayWithVisa(new VisaPaymentViewModel
		//	{
		//		CVV2 = "cvv2",
		//		CardHolderName = "card-holder-name",
		//		CardNumber = "card-number",
		//		ExpirationMonth = DateTime.UtcNow.Month,
		//		ExpirationYear = DateTime.UtcNow.Year
		//	}) as RedirectToRouteResult;

		//	Assert.IsNotNull(result);
		//	Assert.AreEqual("AllGames", result.RouteValues["Action"].ToString());
		//}

		#endregion

		#region Shipper action

		[TestMethod]
		public void ChangeShipper_Action_By_Get_Method_Does_Not_Return_Null()
		{
			_mockViewModelBuilder
				.Setup(b => b.BuildAllShippersViewModel(It.IsAny<Guid>()))
				.Returns(new AllShippersViewModel());

			ActionResult result = _orderController.Shipper();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ChangeShipper_Action_By_Post_Method_Redirects_To_Details_Action()
		{
			var result = _orderController.Shipper(new AllShippersViewModel()) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"].ToString());
		}

		#endregion

		#region History action

		[TestMethod]
		public void History_Action_Does_Not_Return_Null()
		{
			ActionResult result = _orderController.History(new OrdersHistoryViewModel());

			Assert.IsNotNull(result);
		}

		#endregion
	}
}