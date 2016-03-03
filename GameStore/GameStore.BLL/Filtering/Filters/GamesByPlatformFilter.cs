using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByPlatformFilter : IFilter<Game>
	{
		public IEnumerable<string> SelectedPlatformTypes { get; set; }

		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			return game => game.PlatformTypes
				.Select(platform => platform.Type)
				.Any(platform => SelectedPlatformTypes.Contains(platform));
		}
	}
}