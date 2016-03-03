using System;
using System.Linq;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.BLL.Sorting.Sortings
{
	public class GameByMostCommentedSorting : ISorting<Game>
	{
		public bool IsAscending
		{
			get { return false; }
		}

		public Func<Game, object> SortCondition
		{
			get { return game => game.GameHistory.Count(hist => hist.Type == OperationType.Comment); }
		}
	}
}
