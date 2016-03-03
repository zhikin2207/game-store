using System;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Sorting.Sortings
{
	public class GameByPriceSorting : ISorting<Game>
	{
		private readonly bool _isAscending;

		public GameByPriceSorting(bool isAscending)
		{
			_isAscending = isAscending;
		}

		public bool IsAscending
		{
			get { return _isAscending; }
		}

		public Func<Game, object> SortCondition
		{
			get { return game => game.Price; }
		}
	}
}