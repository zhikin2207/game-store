using System;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByPriceFilter : IFilter<Game>
	{
		public decimal MinPrice { get; set; }

		public decimal MaxPrice { get; set; }

		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			return game => game.Price >= MinPrice && game.Price <= MaxPrice;
		}
	}
}
