using System;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using GameStore.BLL.Banking;
using GameStore.BLL.Payments;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Order;

namespace GameStore.WebUI.Controllers
{
	public class OrderController : BaseController
	{
		protected const string GameDoesNotExistTemplate = "Requested game does not exist: {0}";
		protected const string GameHasEmptyKey = "Requested game has empty key";

		private const string InvoiceFileName = "INVOICE.TXT";

		private readonly IOrderService _orderService;
		private readonly IOrderViewModelBuilder _orderViewModelBuilder;

		public OrderController(
			IOrderService orderService,
			IOrderViewModelBuilder orderViewModelBuilder,
			ILoggingService loggingService) : base(loggingService)
		{
			_orderService = orderService;
			_orderViewModelBuilder = orderViewModelBuilder;
		}

        [HttpGet]
		public ActionResult Basket()
		{
			Guid currentUserGuid = InitializeBasketGuid();

			BasketViewModel basketViewModel = _orderViewModelBuilder.BuildBasketViewModel(currentUserGuid);
			return View(basketViewModel);
		}

        [HttpGet]
		public ActionResult Details()
		{
			Guid currentUserGuid = InitializeBasketGuid();

			OrderViewModel orderViewModel = _orderViewModelBuilder.BuildOrderViewModel(currentUserGuid);
			return View(orderViewModel);
		}

		[HttpGet]
		public ActionResult Shipper()
		{
			Guid currentUserGuid = InitializeBasketGuid();

			AllShippersViewModel allShippersViewModel = _orderViewModelBuilder.BuildAllShippersViewModel(currentUserGuid);
			return View(allShippersViewModel);
		}

		[HttpPost]
		public ActionResult Shipper(AllShippersViewModel viewModel)
		{
			Guid currentUserGuid = InitializeBasketGuid();

			_orderService.ChangeOrderShipper(currentUserGuid, viewModel.SelectedShipperId);

			return RedirectToAction("Details");
		}

		[HttpPost]
		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = GameDoesNotExistTemplate, Parameter = "gameKey")]
		public ActionResult Buy(string gameKey)
		{
			Guid currentUserGuid = InitializeBasketGuid();

			_orderService.AddItemToBasket(currentUserGuid, gameKey, 1);

			return RedirectToAction("AllGames", "Game");
		}

		[HttpGet]
		public ActionResult History(OrdersHistoryViewModel viewModel)
		{
			if (CurrentUser.IsInRole(UserRole.User))
			{
				viewModel.UserGuid = CurrentUserGuid;
			}

			_orderViewModelBuilder.RebuildOrdersHistoryViewModel(viewModel);
			return View(viewModel);
		}

		[HttpGet]
		public ActionResult Orders(OrdersHistoryViewModel viewModel)
		{
			_orderViewModelBuilder.RebuildOrdersViewModel(viewModel);
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult ShipOrder(OrdersHistoryViewModel viewModel, int orderId)
		{
			_orderService.ShipOrder(orderId);

			_orderViewModelBuilder.RebuildOrdersViewModel(viewModel);
			return PartialView("_Orders", viewModel.Orders);
		}

		#region Payment

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.User)]
		public ActionResult PayWithBank()
		{
			Guid currentBasketGuid = InitializeBasketGuid();

			var paymentMethod = new BankPayment
			{
				UserGuid = currentBasketGuid
			};

			PaymentStatus status = _orderService.Pay(currentBasketGuid, CurrentUserGuid, paymentMethod);

			if (status == PaymentStatus.Success)
			{
				return RedirectToAction("AllGames", "Game");
			}

			return File(
				Encoding.UTF8.GetBytes(paymentMethod.BuildInvoiceFile()),
				MediaTypeNames.Application.Octet,
				InvoiceFileName);
		}

		[HttpGet]
		[AuthorizeUser(UserRole = UserRole.User)]
		public ActionResult PayWithIBoxTerminal()
		{
			Guid currentBasketGuid = InitializeBasketGuid();

			IBoxTerminalPaymentViewModel viewModel = _orderViewModelBuilder.BuildIBoxTerminalPaymentViewModel(currentBasketGuid);
			return View(viewModel);
		}

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.User)]
		public ActionResult PayWithIBoxTerminal(IBoxTerminalPaymentViewModel viewModel)
		{
			var paymentMethod = new IBoxTerminalPayment
			{
				InvoiceNumber = viewModel.InvoiceNumber
			};

			_orderService.Pay(viewModel.BasketGuid, CurrentUserGuid, paymentMethod);

			return RedirectToAction("AllGames", "Game");
		}

		[HttpGet]
		[AuthorizeUser(UserRole = UserRole.User)]
		public ActionResult PayWithVisa()
		{
			return View();
		}

		[HttpPost]
		[AuthorizeUser(UserRole = UserRole.User)]
		public ActionResult PayWithVisa(VisaPaymentViewModel viewModel)
		{
			Guid currentBasketGuid = InitializeBasketGuid();

			var paymentMethod = new VisaPayment
			{
				CardHolderName = viewModel.CardHolderName,
				CardNumber = viewModel.CardNumber,
				CVV2 = viewModel.CVV2,
				ExpirationMonth = viewModel.ExpirationMonth,
				ExpirationYear = viewModel.ExpirationYear,
				Phone = viewModel.Phone,
				Email = ((UserIdentity)CurrentUser.Identity).CurrentUser.Email
			};

			PaymentStatus status = _orderService.Pay(currentBasketGuid, CurrentUserGuid, paymentMethod);

			if (status != PaymentStatus.Success)
			{
				ModelState.AddModelError(String.Empty, status.ToString());

				return View(viewModel);
			}

			return RedirectToAction("AllGames", "Game");
		}

		#endregion
	}
}