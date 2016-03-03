using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.Filtering.Components;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Filter.Interfaces;

namespace GameStore.WebUI.ViewModels.Filter
{
	public class GamesByDateFilterViewModel : IViewModelFilter
	{
		public GameDateDisplayOptions SelectedDateOption { get; set; }

		public IEnumerable<SelectListItem> DateFilterOptions
		{
			get
			{
				return new[]
				{
					new SelectListItem { Text = Resource.DateFilterOption_All, Value = GameDateDisplayOptions.All.ToString() },
					new SelectListItem { Text = Resource.DateFilterOption_Week, Value = GameDateDisplayOptions.LastWeek.ToString() },
					new SelectListItem { Text = Resource.DateFilterOption_Month, Value = GameDateDisplayOptions.LastMonth.ToString() },
					new SelectListItem { Text = Resource.DateFilterOption_Year, Value = GameDateDisplayOptions.LastYear.ToString() },
					new SelectListItem { Text = Resource.DateFilterOption_2Years, Value = GameDateDisplayOptions.TwoYears.ToString() },
					new SelectListItem { Text = Resource.DateFilterOption_3Years, Value = GameDateDisplayOptions.ThreeYears.ToString() }
				};
			}
		}

		public bool IsSet
		{
			get { return true; }
		}
	}
}