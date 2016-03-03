using System.Collections.Generic;
using System.Linq;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByPlatformFilterViewModel : IViewModelFilter
	{
		public GamesByPlatformFilterViewModel()
		{
			SelectedPlatformTypes = new List<string>();
		}

		public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }

		public IEnumerable<string> SelectedPlatformTypes { get; set; }

		public bool IsSet
		{
			get { return SelectedPlatformTypes.Any(); }
		}
	}
}