using System;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByNameFilter : IFilter<Game>
	{
		public string Name { get; set; }

		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			return game => game.Name.ToLower().Contains(Name.ToLower());
		}
	}
}
