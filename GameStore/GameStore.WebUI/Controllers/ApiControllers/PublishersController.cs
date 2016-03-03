using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModels.Publisher;

namespace GameStore.WebUI.Controllers.ApiControllers
{
	[AuthorizeUser(UserRole = UserRole.Manager)]
	public class PublishersController : ApiController
	{
		private readonly IPublisherService _publisherService;

		public PublishersController(IPublisherService publisherService)
		{
			_publisherService = publisherService;
		}

		[AllowAnonymous]
		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Get()
		{
			IEnumerable<PublisherDto> publishers = _publisherService.GetPublishers();
			return Request.CreateResponse(HttpStatusCode.OK, publishers);
		}

		[AllowAnonymous]
		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Get(string key)
		{
			PublisherDto publisher = _publisherService.GetPublisher(key);
			return Request.CreateResponse(HttpStatusCode.OK, publisher);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Post([FromBody]CreateEditPublisherViewModel createEditPublisherViewModel)
		{
			if (ModelState.IsValid)
			{
				if (!_publisherService.IsExist(createEditPublisherViewModel.CurrentPublisher.CompanyName))
				{
					var publisher = Mapper.Map<PublisherDto>(createEditPublisherViewModel);
					_publisherService.Create(publisher);

					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}

			return Request.CreateResponse(HttpStatusCode.InternalServerError);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.InternalServerError)]
		public HttpResponseMessage Put([FromBody]CreateEditPublisherViewModel createEditPublisherViewModel, [FromUri]string oldCompanyName)
		{
			if (ModelState.IsValid)
			{
				bool skipExistanceCheck = createEditPublisherViewModel.CurrentPublisher.CompanyName == oldCompanyName;

				if (skipExistanceCheck || !_publisherService.IsExist(createEditPublisherViewModel.CurrentPublisher.CompanyName))
				{
					var publisher = Mapper.Map<PublisherDto>(createEditPublisherViewModel);
					_publisherService.Update(publisher);

					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}

			return Request.CreateResponse(HttpStatusCode.InternalServerError);
		}

		[ActionName("Default")]
		[ApiLogError(ExceptionType = typeof(InvalidOperationException), StatusCode = HttpStatusCode.NotFound)]
		public HttpResponseMessage Delete(string key)
		{
			_publisherService.Delete(key);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpGet]
		[AllowAnonymous]
		public HttpResponseMessage Games(string key)
		{
			IEnumerable<GameDto> games = _publisherService.GetPublisherGames(key);
			return Request.CreateResponse(HttpStatusCode.OK, games);
		} 
	}
}