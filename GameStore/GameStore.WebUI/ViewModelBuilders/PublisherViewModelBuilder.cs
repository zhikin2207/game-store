using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.Components;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Basic.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Publisher;

namespace GameStore.WebUI.ViewModelBuilders
{
	public class PublisherViewModelBuilder : IPublisherViewModelBuilder
	{
		private readonly IPublisherService _publisherService;

		public PublisherViewModelBuilder(IPublisherService publisherService)
		{
			_publisherService = publisherService;
		}

		public PublisherViewModel BuildPublisherViewModel(string companyName)
		{
			if (string.IsNullOrEmpty(companyName))
			{
				throw new ArgumentNullException("companyName");
			}

			PublisherDto publisher = _publisherService.GetPublisher(companyName);
			return Mapper.Map<PublisherViewModel>(publisher);
		}

		public CreateEditPublisherViewModel BuildCreatePublisherViewModel()
		{
			return new CreateEditPublisherViewModel();
		}

		public CreateEditPublisherViewModel BuildUpdatePublisherViewModel(string companyName)
		{
			if (string.IsNullOrEmpty(companyName))
			{
				throw new ArgumentNullException("companyName");
			}

			PublisherDto publisher = _publisherService.GetPublisher(companyName);

			PublisherLocalizationDto ukrainianLocalization = publisher.PublisherLocalizations.FirstOrDefault(lg => lg.Language == LanguageDto.Uk);

			return new CreateEditPublisherViewModel
			{
				CurrentPublisher = Mapper.Map<PublisherViewModel>(publisher),
				UkrainianPublisherLocalization = Mapper.Map<PublisherLocalizationViewModel>(ukrainianLocalization)
			};
		}

		public IEnumerable<PublisherViewModel> BuildPublishersViewModel()
		{
			IEnumerable<PublisherDto> publishers = _publisherService.GetPublishers();
			return Mapper.Map<IEnumerable<PublisherViewModel>>(publishers);
		}
	}
}