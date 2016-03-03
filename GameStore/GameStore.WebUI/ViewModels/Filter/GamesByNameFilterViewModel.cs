using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByNameFilterViewModel : IViewModelFilter
	{
		private const int MinNameLength = 3;

		[Display(ResourceType = typeof(Resource), Name = "NameFilter_GameTitle")]
		public string Name { get; set; }

		public bool IsSet
		{
			get { return !string.IsNullOrWhiteSpace(Name) && Name.Length >= MinNameLength; }
		}
	}
}