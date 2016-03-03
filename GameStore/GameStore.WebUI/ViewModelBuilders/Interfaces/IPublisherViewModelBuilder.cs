using System.Collections.Generic;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Publisher;

namespace GameStore.WebUI.ViewModelBuilders.Interfaces
{
	public interface IPublisherViewModelBuilder
	{
		PublisherViewModel BuildPublisherViewModel(string companyName);

		CreateEditPublisherViewModel BuildCreatePublisherViewModel();

		CreateEditPublisherViewModel BuildUpdatePublisherViewModel(string companyName);

		IEnumerable<PublisherViewModel> BuildPublishersViewModel();
	}
}