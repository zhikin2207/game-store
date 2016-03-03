using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByPriceFilterViewModel : IViewModelFilter
	{
		[Display(ResourceType = typeof(Resource), Name = "GamesByPriceFilterViewModel_Minimum")]
		public decimal? MinPrice { get; set; }

		[Display(ResourceType = typeof(Resource), Name = "GamesByPriceFilterViewModel_Maximum")]
		public decimal? MaxPrice { get; set; }

		public bool IsSet
		{
			get { return MinPrice.HasValue && MaxPrice.HasValue; }
		}
	}
}