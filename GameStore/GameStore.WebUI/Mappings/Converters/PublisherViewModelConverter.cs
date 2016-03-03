using System.Collections.ObjectModel;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Publisher;

namespace GameStore.WebUI.Mappings.Converters
{
	public class PublisherViewModelConverter : ITypeConverter<CreateEditPublisherViewModel, PublisherDto>
	{
		public PublisherDto Convert(ResolutionContext context)
		{
			var viewModel = (CreateEditPublisherViewModel)context.SourceValue;
			var publisher = Mapper.Map<PublisherDto>(viewModel.CurrentPublisher);

			publisher.PublisherLocalizations = new Collection<PublisherLocalizationDto>();

			if (viewModel.UkrainianPublisherLocalization != null)
			{
				var ukrainianLocalization = Mapper.Map<PublisherLocalizationDto>(viewModel.UkrainianPublisherLocalization);
				publisher.PublisherLocalizations.Add(ukrainianLocalization);
			}

			return publisher;
		}
	}
}