using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Sorting.Sortings;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Tests.BLL.Sorting
{
	[TestClass]
	public class SortingTest
	{
		private const int GamesCount = 10;

		#region GameByDateAddingSorting

		[TestMethod]
		public void GameByDateAddingSorting_Applies_Correctly()
		{
			var sorting = new GameByDateAddingSorting();

			IEnumerable<Game> resultGames = ApplySortCondition(GetGames(), sorting.SortCondition, sorting.IsAscending);

			for (int i = 0; i < GamesCount - 1; i++)
			{
				Assert.AreEqual((i + 1).ToString(), resultGames.ElementAt(i).Key);
			}
		}

		#endregion

		#region GameByMostCommentedSorting

		[TestMethod]
		public void GameByMostCommentedSorting_Applies_Correctly()
		{
			var sorting = new GameByMostCommentedSorting();

			IEnumerable<Game> resultGames = ApplySortCondition(GetGames(), sorting.SortCondition, sorting.IsAscending);

			for (int i = 0; i < GamesCount; i++)
			{
				Assert.AreEqual((GamesCount - i - 1).ToString(), resultGames.ElementAt(i).Key);
			}
		}

		#endregion

		#region GameByMostViewedSorting

		[TestMethod]
		public void GameByMostViewedSorting_Applies_Correctly()
		{
			var sorting = new GameByMostViewedSorting();

			IEnumerable<Game> resultGames = ApplySortCondition(GetGames(), sorting.SortCondition, sorting.IsAscending);

			for (int i = 0; i < GamesCount; i++)
			{
				Assert.AreEqual((GamesCount - i - 1).ToString(), resultGames.ElementAt(i).Key);
			}
		}

		#endregion

		#region GameByPriceSorting

		[TestMethod]
		public void GameByPriceSorting_Applies_Correctly_When_Sorting_Is_Ascending()
		{
			var sorting = new GameByPriceSorting(true);

			IEnumerable<Game> resultGames = ApplySortCondition(GetGames(), sorting.SortCondition, sorting.IsAscending);

			for (int i = 0; i < GamesCount; i++)
			{
				Assert.AreEqual(i.ToString(), resultGames.ElementAt(i).Key);
			}
		}

		[TestMethod]
		public void GameByPriceSorting_Applies_Correctly_When_Sorting_Is_Descending()
		{
			var sorting = new GameByPriceSorting(false);

			IEnumerable<Game> resultGames = ApplySortCondition(GetGames(), sorting.SortCondition, sorting.IsAscending);

			for (int i = 0; i < GamesCount; i++)
			{
				Assert.AreEqual((GamesCount - i - 1).ToString(), resultGames.ElementAt(i).Key);
			}
		}

		#endregion

		#region Test helpers

		private IEnumerable<Game> GetGames()
		{
			var games = new List<Game>();

			for (int i = 0; i < GamesCount; i++)
			{
				var game = new Game
				{
					Key = i.ToString(),
					Price = i * 100,
					Name = string.Format("game-key-{0}{0}{0}", i),
					GameHistory = new List<GameHistory>()
				};

				for (int h = 0; h < (i + 1); h++)
				{
					WriteGameViewHistory(game);

					WriteGameCommentHistory(game);
				}

				if (i != 0)
				{
					WriteGameAddHistory(game, i);
				}

				games.Add(game);
			}

			return games;
		}

		private void WriteGameAddHistory(Game game, int i)
		{
			var days = new[] { -1, -5, -20, -50, -400, -800, -1200, -2000, -3000, -10000 };
			game.GameHistory.Add(new GameHistory
			{
				GameKey = game.Key,
				Type = OperationType.Add,
				Date = DateTime.UtcNow.AddDays(days[i])
			});
		}

		private void WriteGameCommentHistory(Game game)
		{
			game.GameHistory.Add(new GameHistory
			{
				GameKey = game.Key,
				Type = OperationType.Comment
			});
		}

		private void WriteGameViewHistory(Game game)
		{
			game.GameHistory.Add(new GameHistory
			{
				GameKey = game.Key,
				Type = OperationType.View
			});
		}

		private IEnumerable<Game> ApplySortCondition(IEnumerable<Game> source, Func<Game, object> condition, bool isAscending)
		{
			if (isAscending)
			{
				return source.OrderBy(condition).ToList();
			}

			return source.OrderByDescending(condition).ToList();
		}

		#endregion
	}
}