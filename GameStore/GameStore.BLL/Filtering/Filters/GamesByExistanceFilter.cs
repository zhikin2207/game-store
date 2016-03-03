using System;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByExistanceFilter : IFilter<Game>
	{
		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			return game => !game.IsDeleted;
		}
	}
}
