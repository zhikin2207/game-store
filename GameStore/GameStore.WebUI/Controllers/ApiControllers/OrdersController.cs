using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Filters;

namespace GameStore.WebUI.Controllers.ApiControllers
{
	public class OrdersController : ApiController
	{
		private readonly IOrderService _orderService;

		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Get(int key)
		{
			OrderDto orderDto = _orderService.Get(key);
			return Request.CreateResponse(HttpStatusCode.OK, orderDto);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Post([FromBody]OrderDetailsDto orderDetails, [FromUri]Guid userGuid)
		{
			_orderService.AddItemToBasket(userGuid, orderDetails.GameKey, orderDetails.Quantity);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Delete([FromBody]OrderDetailsDto orderDetails, [FromUri]Guid userGuid)
		{
			_orderService.DeleteItemFromBasket(userGuid, orderDetails.GameKey);

			return Request.CreateResponse(HttpStatusCode.OK);
		} 
	}
}