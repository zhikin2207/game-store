using System.Collections.Generic;
using System.Linq;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByPublisherFilterViewModel : IViewModelFilter
	{
		public GamesByPublisherFilterViewModel()
		{
			PublisherCompanies = new List<string>();
		}

		public IEnumerable<PublisherViewModel> Publishers { get; set; }

		public IEnumerable<string> PublisherCompanies { get; set; }

		public bool IsSet
		{
			get { return PublisherCompanies.Any(); }
		}
	}
}