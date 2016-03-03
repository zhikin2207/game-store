using System.Collections.Generic;
using System.Linq;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByGenreFilterViewModel : IViewModelFilter
	{
		public GamesByGenreFilterViewModel()
		{
			GenreNames = new List<string>();
		}

		public IEnumerable<GenreViewModel> Genres { get; set; }

		public IEnumerable<string> GenreNames { get; set; }

		public bool IsSet
		{
			get { return GenreNames.Any(); }
		}
	}
}