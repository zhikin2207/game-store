using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.Services.Components;
using GameStore.WebUI.App_LocalResources;
using GameStore.WebUI.ViewModels.Basic;

namespace GameStore.WebUI.ViewModels.User
{
	public class BanUserViewModel
	{
		public UserViewModel User { get; set; }

		public IEnumerable<SelectListItem> BanUserDisplayOptions
		{
			get
			{
				return new SelectListItem[]
				{
					new SelectListItem { Text = Resource.BanUserDisplayOption_Hour, Value = BanUserOption.Hour.ToString() },
					new SelectListItem { Text = Resource.BanUserDisplayOption_Day, Value = BanUserOption.Day.ToString() },
					new SelectListItem { Text = Resource.BanUserDisplayOption_Week, Value = BanUserOption.Week.ToString() },
					new SelectListItem { Text = Resource.BanUserDisplayOption_Month, Value = BanUserOption.Month.ToString() },
					new SelectListItem { Text = Resource.BanUserDisplayOption_Permanent, Value = BanUserOption.Permanent.ToString() }
				};
			}
		}

		public BanUserOption SelectedBanOption { get; set; }
	}
}