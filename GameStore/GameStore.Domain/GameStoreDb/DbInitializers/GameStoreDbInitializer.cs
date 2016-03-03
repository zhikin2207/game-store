using System;
using System.Collections.Generic;
using System.Data.Entity;
using GameStore.Domain.GameStoreDb.Entities;

namespace GameStore.Domain.GameStoreDb.DbInitializers
{
	public class GameStoreDbInitializer : CreateDatabaseIfNotExists<GameStoreContext>
	{
		protected override void Seed(GameStoreContext context)
		{
			InitGenres(context);
			InitPlatformTypes(context);
			InitPublishers(context);
			InitComments(context);
			InitGames(context);

			base.Seed(context);
		}

		private void InitGenres(GameStoreContext context)
		{
			context.Genres.Add(new Genre { Name = "Strategy", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "RPG", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Sports", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Races", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Action", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Adventure", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Puzzle&Skill", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Misc", ParentGenreId = null });

			// "Strategy" nested genres
			context.Genres.Add(new Genre { Name = "RTS", ParentGenreId = 1 });
			context.Genres.Add(new Genre { Name = "TBS", ParentGenreId = 1 });

			// "Races" nested genres
			context.Genres.Add(new Genre { Name = "Rally", ParentGenreId = 4 });
			context.Genres.Add(new Genre { Name = "Arcade", ParentGenreId = 4 });
			context.Genres.Add(new Genre { Name = "Formula", ParentGenreId = 4 });
			context.Genres.Add(new Genre { Name = "Off-road", ParentGenreId = 4 });

			// "Action" nested genres
			context.Genres.Add(new Genre { Name = "FPS", ParentGenreId = 5 });
			context.Genres.Add(new Genre { Name = "TPS", ParentGenreId = 5 });
			context.Genres.Add(new Genre { Name = "Misc", ParentGenreId = 5 });
			context.SaveChanges();
		}

		private void InitPlatformTypes(GameStoreContext context)
		{
			context.PlatformTypes.Add(new PlatformType { Type = "Mobile" });
			context.PlatformTypes.Add(new PlatformType { Type = "Browser" });
			context.PlatformTypes.Add(new PlatformType { Type = "Desctop" });
			context.PlatformTypes.Add(new PlatformType { Type = "Console" });
			context.SaveChanges();
		}

		private void InitPublishers(GameStoreContext context)
		{
			context.Publishers.Add(new Publisher
			{
				CompanyName = "Valve-Corporation",
				HomePage = "http://valvesoftware.com",
				Description = "We’re always creating"
			});

			context.Publishers.Add(new Publisher
			{
				CompanyName = "Microsoft",
				HomePage = "http://www.microsoft.com",
				Description = "Big company"
			});

			context.Publishers.Add(new Publisher
			{
				CompanyName = "EA-Games",
				HomePage = "http://www.ea.com",
				Description = "EA Games is awesome game company"
			});
			context.SaveChanges();
		}

		private void InitComments(GameStoreContext context)
		{
			context.Comments.Add(new Comment
			{
				Name = "Mutex",
				Body = "Good game.",
				GameKey = "nfs-most-wanted"
			});

			context.Comments.Add(new Comment
			{
				Name = "Vasia",
				Body = "Mutex, I also think so!!!",
				GameKey = "nfs-most-wanted",
				ParentCommentId = 1
			});
		}

		private void InitGames(GameStoreContext context)
		{
			context.Games.Add(new Game
			{
				Key = "nfs-most-wanted",
				Name = "Need for Speed. Most Wanted",
				Description = "The best race game ever.",
				PublisherId = 3,
				Price = 280,
				UnitsInStock = 200,
				Discontinued = false,
				DatePublished = DateTime.UtcNow,
				Genres = new List<Genre>
				{
					context.Genres.Find(4)
				},
				PlatformTypes = new List<PlatformType>
				{
					context.PlatformTypes.Find(1),
					context.PlatformTypes.Find(3)
				},
				Comments = new List<Comment>
				{
					context.Comments.Find(1),
					context.Comments.Find(2)
				}
			});

			context.Games.Add(new Game
			{
				Key = "cs-1-6",
				Name = "Counter Strike 1.6",
				Description = "If you want to spend time perfect this game is for you.",
				PublisherId = 1,
				Price = 100,
				UnitsInStock = 1000,
				Discontinued = false,
				DatePublished = DateTime.UtcNow,
				Genres = new List<Genre>
				{
					context.Genres.Find(5)
				},
				PlatformTypes = new List<PlatformType>
				{
					context.PlatformTypes.Find(2),
					context.PlatformTypes.Find(3)
				}
			});

			context.Games.Add(new Game
			{
				Key = "Kosinka",
				Name = "Old PC hardcore game.",
				Description = "Do not use Alt + Shift + 2.",
				PublisherId = 2,
				Price = 0,
				UnitsInStock = 2000,
				Discontinued = false,
				DatePublished = DateTime.UtcNow,
				Genres = new List<Genre>
				{
					context.Genres.Find(7)
				},
				PlatformTypes = new List<PlatformType>
				{
					context.PlatformTypes.Find(3)
				}
			});

			context.SaveChanges();
		}
	}
}