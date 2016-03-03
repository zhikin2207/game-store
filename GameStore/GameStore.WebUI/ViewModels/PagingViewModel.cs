using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels
{
	public class PagingViewModel
	{
		public const int DisplayedPagesCount = 10;

		public PagingViewModel()
		{
			CurrentPage = 1;
			ItemsPerPage = 10;
		}

		[Display(ResourceType = typeof(Resource), Name = "PagingViewModel_ItemsPerPage")]
		public IEnumerable<SelectListItem> ItemPerPageOptions
		{
			get
			{
				return new[]
				{
					new SelectListItem { Text = 10.ToString(), Value = "10" },
					new SelectListItem { Text = 20.ToString(), Value = "20" },
					new SelectListItem { Text = 50.ToString(), Value = "50" },
					new SelectListItem { Text = 100.ToString(), Value = "100" },
					new SelectListItem { Text = Resource.ItemsPerPageDisplayItem_All, Value = "0" }
				};
			}
		}

		public int CurrentPage { get; set; }

		public int ItemsPerPage { get; set; }

		public int TotalGamesNumber { get; set; }

		public bool CanGoNext
		{
			get { return CurrentPage < PagesCount; }
		}

		public bool CanGoPrev
		{
			get { return CurrentPage > 1; }
		}

		public int PagesCount
		{
			get { return (int)Math.Ceiling((double)TotalGamesNumber / ItemsPerPage); }
		}

		public int LeftDisplayedPagesCount
		{
			get
			{
				bool displayCondition = CurrentPage > PagesCount - (DisplayedPagesCount / 2);

				if (displayCondition)
				{
					return DisplayedPagesCount - (PagesCount - CurrentPage + 1);
				}

				return DisplayedPagesCount - RightDisplayedPagesCount - 1;
			}
		}

		public int RightDisplayedPagesCount
		{
			get
			{
				if (CurrentPage <= DisplayedPagesCount / 2)
				{
					return DisplayedPagesCount - CurrentPage;
				}

				return DisplayedPagesCount / 2;
			}
		}
	}
}