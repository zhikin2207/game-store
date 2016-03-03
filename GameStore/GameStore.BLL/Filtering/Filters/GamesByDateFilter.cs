using System;
using GameStore.BLL.Filtering.Components;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByDateFilter : IFilter<Game>
	{
		public GameDateDisplayOptions SelectedDateOption { get; set; }

		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			switch (SelectedDateOption)
			{
				case GameDateDisplayOptions.LastWeek:
					return game => game.DatePublished >= DateTime.UtcNow.AddDays(-7);
				case GameDateDisplayOptions.LastMonth:
					return game => game.DatePublished >= DateTime.UtcNow.AddMonths(-1);
				case GameDateDisplayOptions.LastYear:
					return game => game.DatePublished >= DateTime.UtcNow.AddYears(-1);
				case GameDateDisplayOptions.TwoYears:
					return game => game.DatePublished >= DateTime.UtcNow.AddYears(-2);
				case GameDateDisplayOptions.ThreeYears:
					return game => game.DatePublished >= DateTime.UtcNow.AddYears(-3);
				default:
					return game => true;
			}
		}
	}
}
