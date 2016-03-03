using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByGenreFilter : IFilter<Game>
	{
		public IEnumerable<string> GenreNames { get; set; }

		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			return game => game.Genres.Select(genre => genre.Name).Any(genre => GenreNames.Contains(genre));
		}
	}
}
