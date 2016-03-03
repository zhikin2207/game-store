using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Attributes;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Publisher;
using Resources;

namespace GameStore.WebUI.Controllers
{
	[AuthorizeUser(UserRole = UserRole.Manager)]
	public class PublisherController : BaseController
	{
		private const string PublisherDoesNotExistTemplate = "Requested publisher does not exist: {0}";
		private const string PublisherHasEmptyKeyTemplate = "Requested publisher has empty key";

		private readonly IPublisherService _publisherService;
		private readonly IPublisherViewModelBuilder _publisherViewModelBuilder;

		public PublisherController(
			IPublisherService publisherService, 
			IPublisherViewModelBuilder publisherViewModelBuilder, 
			ILoggingService loggingService) : base(loggingService)
		{
			_publisherService = publisherService;
			_publisherViewModelBuilder = publisherViewModelBuilder;
		}

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Publishers()
        {
            IEnumerable<PublisherViewModel> publishers = _publisherViewModelBuilder.BuildPublishersViewModel();
            return View(publishers);
        }

        [HttpGet]
		[AllowAnonymous]
		[LogError(ExceptionType = typeof(ArgumentNullException), LogMessage = PublisherHasEmptyKeyTemplate)]
		[LogError(ExceptionType = typeof(InvalidOperationException), LogMessage = PublisherDoesNotExistTemplate, Parameter = "company")]
		public ActionResult Details(string company)
		{
			PublisherViewModel publisherViewModel = _publisherViewModelBuilder.BuildPublisherViewModel(company);
			return View(publisherViewModel);
		}

        [HttpGet]
		public ActionResult New()
        {
	        var createEditViewModel = new CreateEditPublisherViewModel();
			return View(createEditViewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult New(CreateEditPublisherViewModel createEditPublisherViewModel)
		{
			if (ModelState.IsValid)
			{
				if (!_publisherService.IsExist(createEditPublisherViewModel.CurrentPublisher.CompanyName))
				{
					var publisher = Mapper.Map<PublisherDto>(createEditPublisherViewModel);
					_publisherService.Create(publisher);

					return RedirectToAction("Publishers", "Publisher");
				}

				ModelState.AddModelError("CompanyName", GlobalResource.Publisher_PublisherExistValidation);
			}

			return View(createEditPublisherViewModel);
		}

		[HttpGet]
		[AuthorizeUser(UserRole = UserRole.Manager | UserRole.Publisher)]
		public ActionResult Update(string company)
		{
			CreateEditPublisherViewModel publisherViewModel = _publisherViewModelBuilder.BuildUpdatePublisherViewModel(company);

			if (!CurrentUser.IsInRole(UserRole.Manager) && !IsUserSpecificPublisher(publisherViewModel.CurrentPublisher.CompanyName))
			{
				return RedirectToAction("Publishers");
			}

			return View(publisherViewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		[AuthorizeUser(UserRole = UserRole.Manager | UserRole.Publisher)]
		public ActionResult Update(CreateEditPublisherViewModel publisherViewModel, string currentCompany)
		{
			if (ModelState.IsValid)
			{
				bool skipExistanceCheck = publisherViewModel.CurrentPublisher.CompanyName == currentCompany;

				if (skipExistanceCheck || !_publisherService.IsExist(publisherViewModel.CurrentPublisher.CompanyName))
				{
					var publisher = Mapper.Map<PublisherDto>(publisherViewModel);
					_publisherService.Update(publisher);

					return RedirectToAction("Publishers", "Publisher");
				}

				ModelState.AddModelError("CompanyName", GlobalResource.Publisher_PublisherExistValidation);
			}

			return View(publisherViewModel);
		}

        [HttpPost]
        public ActionResult Delete(string company)
        {
            _publisherService.Delete(company);

            return RedirectToAction("Publishers");
        }
	}
}
