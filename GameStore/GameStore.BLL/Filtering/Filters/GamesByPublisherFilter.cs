using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Filtering.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.BLL.Filtering.Filters
{
	public class GamesByPublisherFilter : IFilter<Game>
	{
		public IEnumerable<string> PublisherCompanies { get; set; }

		public bool IsSet { get; set; }

		public Func<Game, bool> GetCondition()
		{
			return game => game.Publisher != null && PublisherCompanies.Contains(game.Publisher.CompanyName);
		}
	}
}