using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.Game
{
	public class GameDetailsViewModel
	{
		public GameDetailsViewModel()
		{
			Game = new GameViewModel();
			Publisher = new PublisherViewModel();
			Genres = new List<GenreViewModel>();
			PlatformTypes = new List<PlatformTypeViewModel>();
		}

		public GameViewModel Game { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Publisher")]
		public PublisherViewModel Publisher { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Genres")]
		public IEnumerable<GenreViewModel> Genres { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "Common_Platforms")]
		public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }
	}
}