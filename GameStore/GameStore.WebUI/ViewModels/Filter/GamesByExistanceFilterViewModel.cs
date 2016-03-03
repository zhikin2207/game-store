using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByExistanceFilterViewModel : IViewModelFilter
	{
		public bool HideDeletedGames { get; set; }

		public bool IsSet
		{
			get { return HideDeletedGames; }
		}
	}
}