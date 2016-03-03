using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.BLL.Sorting.Components;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Sorting
{
	public class GameSortingViewModel
	{
		[Display(ResourceType = typeof(Resource), Name = "GameSortingViewModel_Sort")]
		public GameSortDisplayOptions SelectedSortOption { get; set; }

		public IEnumerable<SelectListItem> SortDisplayOptions
		{
			get
			{
				return new[]
				{
					new SelectListItem { Text = Resource.SortDisplayOption_MostViewed, Value = GameSortDisplayOptions.MostViewed.ToString() },
					new SelectListItem { Text = Resource.SortDisplayOption_MostCommented, Value = GameSortDisplayOptions.MostCommented.ToString() },
					new SelectListItem { Text = Resource.SortDisplayOption_New, Value = GameSortDisplayOptions.New.ToString() },
					new SelectListItem { Text = Resource.SortDisplayOption_PriceAsc, Value = GameSortDisplayOptions.PriceAsc.ToString() },
					new SelectListItem { Text = Resource.SortDisplayOption_PriceDesc, Value = GameSortDisplayOptions.PriceDesc.ToString() }
				};
			}
		}
	}
}