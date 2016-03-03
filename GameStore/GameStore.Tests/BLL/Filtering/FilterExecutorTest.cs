using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Filtering;
using GameStore.BLL.Filtering.Components;
using GameStore.BLL.Filtering.Filters;
using GameStore.Domain.GameStoreDb.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Tests.BLL.Filtering
{
	[TestClass]
	public class FilterExecutorTest
	{
		private const int GamesCount = 10;
		private const int PlatformsCount = 5;
		private const int GenresCount = 5;
		private const int PublishersCount = 10;

		private FilterExecutor<Game> _filterExecutor;

		[TestInitialize]
		public void InitializeTest()
		{
			_filterExecutor = new FilterExecutor<Game>();
		}

		#region GamesByDateFilter test

		[TestMethod]
		public void GamesByDateFilter_Applies_Correctly_With_All_Display_Option()
		{
			var filter = new GamesByDateFilter
			{
				SelectedDateOption = GameDateDisplayOptions.All,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(10, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Applies_Correctly_With_LastWeek_Display_Option()
		{
			var filter = new GamesByDateFilter
			{
				SelectedDateOption = GameDateDisplayOptions.LastWeek,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(2, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Applies_Correctly_With_LastMonth_Display_Option()
		{
			var filter = new GamesByDateFilter
			{
				SelectedDateOption = GameDateDisplayOptions.LastMonth,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(3, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Applies_Correctly_With_LastYear_Display_Option()
		{
			var filter = new GamesByDateFilter
			{
				SelectedDateOption = GameDateDisplayOptions.LastYear,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(4, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Applies_Correctly_With_Last2Years_Display_Option()
		{
			var filter = new GamesByDateFilter
			{
				SelectedDateOption = GameDateDisplayOptions.TwoYears,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(5, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Applies_Correctly_With_Last3Years_Display_Option()
		{
			var filter = new GamesByDateFilter
			{
				SelectedDateOption = GameDateDisplayOptions.ThreeYears,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(6, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByDateFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(GamesCount, games.Count());
		}

		[TestMethod]
		public void GamesByDateFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByDateFilter());

			IEnumerable<Func<Game, bool>> conditoins = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditoins);
		}

		#endregion

		#region GamesByGenreFilter test

		[TestMethod]
		public void GamesByGenreFilter_Applies_Correctly()
		{
			var filter = new GamesByGenreFilter
			{
				GenreNames = new[] { "1", "2" },
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(6, games.Count());
		}

		[TestMethod]
		public void GamesByGenreFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByGenreFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(GamesCount, games.Count());
		}

		[TestMethod]
		public void GamesByGenreFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByGenreFilter());

			IEnumerable<Func<Game, bool>> conditoins = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditoins);
		}

		#endregion

		#region GamesByNameFilter test

		[TestMethod]
		public void GamesByNameFilter_Applies_Correctly()
		{
			var filter = new GamesByNameFilter
			{
				Name = "000",
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(1, games.Count());
		}

		[TestMethod]
		public void GamesByNameFilter_Applies_Correctly_Without_Case_Sesitive()
		{
			var filter = new GamesByNameFilter
			{
				Name = "GAME-KEY",
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(10, games.Count());
		}

		[TestMethod]
		public void GamesByNameFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByNameFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(GamesCount, games.Count());
		}

		[TestMethod]
		public void GamesByNameFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByNameFilter());

			IEnumerable<Func<Game, bool>> conditoins = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditoins);
		}

		#endregion

		#region GamesByPlatformFilter test

		[TestMethod]
		public void GamesByPlatformFilter_Applies_Correctly()
		{
			var filter = new GamesByPlatformFilter
			{
				SelectedPlatformTypes = new[] { "1", "2" },
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(6, games.Count());
		}

		[TestMethod]
		public void GamesByPlatformFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByPlatformFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(GamesCount, games.Count());
		}

		[TestMethod]
		public void GamesByPlatformFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByPlatformFilter());

			IEnumerable<Func<Game, bool>> conditoins = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditoins);
		}

		#endregion

		#region GamesByPriceFilter test

		[TestMethod]
		public void GamesByPriceFilter_Applies_Correctly()
		{
			var filter = new GamesByPriceFilter
			{
				MaxPrice = 500,
				MinPrice = 10,
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(5, games.Count());
		}

		[TestMethod]
		public void GamesByPriceFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByPriceFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(GamesCount, games.Count());
		}

		[TestMethod]
		public void GamesByPriceFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByPriceFilter());

			IEnumerable<Func<Game, bool>> conditoins = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditoins);
		}

		#endregion

		#region GamesByPublisherFilter test

		[TestMethod]
		public void GamesByPublisherFilter_Applies_Correctly()
		{
			var filter = new GamesByPublisherFilter
			{
				PublisherCompanies = new[] { "1", "2" },
				IsSet = true
			};

			_filterExecutor.Register(filter);

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(2, games.Count());
		}

		[TestMethod]
		public void GamesByPublisherFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByPublisherFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(10, games.Count());
		}

		[TestMethod]
		public void GamesByPublisherFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByPublisherFilter());

			IEnumerable<Func<Game, bool>> conditoins = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditoins);
		}

		#endregion

		#region GamesByExistanceFilter test

		[TestMethod]
		public void GamesByExistanceFilter_Applies_Correctly()
		{
			_filterExecutor.Register(new GamesByExistanceFilter { IsSet = true });

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(5, games.Count());
		}

		[TestMethod]
		public void GamesByExistanceFilter_Does_Not_Work_When_IsSet_Property_Equals_False()
		{
			_filterExecutor.Register(new GamesByExistanceFilter());

			IEnumerable<Game> games = ApplyFilter(GetGamesCollection(), _filterExecutor.GetConditions());

			Assert.AreEqual(10, games.Count());
		}

		[TestMethod]
		public void GamesByExistanceFilter_Does_Not_Return_Null()
		{
			_filterExecutor.Register(new GamesByExistanceFilter());

			IEnumerable<Func<Game, bool>> conditions = _filterExecutor.GetConditions();

			Assert.IsNotNull(conditions);
		}

		#endregion

		#region Test helpers

		private IEnumerable<Game> GetGamesCollection()
		{
			IEnumerable<PlatformType> platformTypes = GetPlatformTypes();
			IEnumerable<Genre> genres = GetGenres();
			IEnumerable<Publisher> publishers = GetPublishers();

			var days = new[] { -1, -5, -20, -50, -400, -800, -1200, -2000, -3000, -10000 };

			var games = new List<Game>();

			for (int i = 0; i < GamesCount; i++)
			{
				var game = new Game
				{
					Name = string.Format("game-key-{0}{0}{0}", i),
					Price = i * 100,
					PublisherId = i,
					DatePublished = DateTime.UtcNow.AddDays(days[i]),
					IsDeleted = i % 2 == 0,
					PlatformTypes = new List<PlatformType>
					{
						platformTypes.ElementAt(i % platformTypes.Count()),
						platformTypes.ElementAt((i + 1) % platformTypes.Count())
					},
					Genres = new List<Genre>
					{
						genres.ElementAt(i % genres.Count()),
						genres.ElementAt((i + 1) % genres.Count())
					},
					Publisher = publishers.ElementAt(i % publishers.Count())
				};

				games.Add(game);
			}

			return games;
		}

		private IEnumerable<PlatformType> GetPlatformTypes()
		{
			var platformTypes = new List<PlatformType>();

			for (int i = 0; i < PlatformsCount; i++)
			{
				platformTypes.Add(new PlatformType { Type = i.ToString() });
			}

			return platformTypes;
		}

		private IEnumerable<Genre> GetGenres()
		{
			var genres = new List<Genre>();

			for (int i = 0; i < GenresCount; i++)
			{
				genres.Add(new Genre { Name = i.ToString() });
			}

			return genres;
		}

		private IEnumerable<Publisher> GetPublishers()
		{
			var publishers = new List<Publisher>();

			for (int i = 0; i < PublishersCount; i++)
			{
				publishers.Add(new Publisher { CompanyName = i.ToString() });
			}

			return publishers;
		}

		private IEnumerable<Game> ApplyFilter(IEnumerable<Game> source, IEnumerable<Func<Game, bool>> conditions)
		{
			IEnumerable<Game> gamesResult = source;
			foreach (Func<Game, bool> condition in conditions)
			{
				gamesResult = gamesResult.Where(condition);
			}

			return gamesResult.ToList();
		}

		#endregion
	}
}