using System;
using System.Linq;
using GameStore.BLL.Sorting.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;

namespace GameStore.BLL.Sorting.Sortings
{
	public class GameByDateAddingSorting : ISorting<Game>
	{
		public bool IsAscending
		{
			get { return false; }
		}

		public Func<Game, object> SortCondition
		{
			get
			{
				return game =>
				{
					GameHistory history = game.GameHistory.FirstOrDefault(hist => hist.Type == OperationType.Add);

					if (history == null)
					{
						return default(DateTime);
					}

					return history.Date;
				};
			}
		}
	}
}