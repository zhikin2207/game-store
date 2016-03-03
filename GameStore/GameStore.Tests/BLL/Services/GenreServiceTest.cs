using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.Mappings;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Domain.GameStoreDb.Entities;
using GameStore.Domain.GameStoreDb.Entities.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.BLL.Services
{
	[TestClass]
	public class GenreServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private IGenreService _genreService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingBllProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_genreService = new GenreService(_mockUnitOfWork.Object);
		}

		#region GetGenres method

		[TestMethod]
		public void GetGenres_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(u => u.GenreRepository.GetList()).Returns(Enumerable.Empty<Genre>());

			IEnumerable<GenreDto> genres = _genreService.GetGenres();

			Assert.IsNotNull(genres);
		}

		[TestMethod]
		public void GetGenres_Maps_Genres_Appropriately()
		{
			IEnumerable<Genre> genres = new[]
			{
				new Genre { GenreId = 1, Name = "genre-1" },
				new Genre { GenreId = 2, Name = "genre-2" },
				new Genre { GenreId = 2, Name = "genre-3" }
			};

			_mockUnitOfWork.Setup(u => u.GenreRepository.GetList()).Returns(genres);

			IEnumerable<GenreDto> genresDto = _genreService.GetGenres();

			Assert.AreEqual(3, genresDto.Count());
			for (int i = 0; i < genresDto.Count(); i++)
			{
				Assert.AreEqual(genres.ElementAt(i).GenreId, genresDto.ElementAt(i).GenreId);
				Assert.AreEqual(genres.ElementAt(i).Name, genresDto.ElementAt(i).Name);
			}
		}

		#endregion

		#region IsExist method

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsExist_Throws_ArgumentNullException_When_Company_Name_Is_Not_Set()
		{
			_genreService.IsExist(null);
		}

		[TestMethod]
		public void IsExist_Returns_True_If_Genre_Exists()
		{
			_mockUnitOfWork.Setup(u => u.GenreRepository.IsExist(It.IsAny<string>())).Returns(true);

			bool result = _genreService.IsExist("genre-1");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsExist_Returns_False_If_Genre_Does_Not_Exist()
		{
			_mockUnitOfWork.Setup(u => u.GenreRepository.IsExist(It.IsAny<string>())).Returns(false);

			bool result = _genreService.IsExist("genre-1");

			Assert.IsFalse(result);
		}

		#endregion

		#region GetGenre method

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetGenre_Throws_ArgumentNullException_When_Genre_Name_Is_Not_Set()
		{
			_genreService.GetGenre(null);
		}

		[TestMethod]
		public void GetGenre_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(s => s.GenreRepository.GetGenre(It.IsAny<string>())).Returns(new Genre());

			GenreDto result = _genreService.GetGenre("genre-1");

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetGenre_Maps_Genre_Correctly()
		{
			var genre = new Genre
			{
				GenreId = 1,
				Name = "genre-1",
				Database = DatabaseName.GameStore
			};

			_mockUnitOfWork.Setup(s => s.GenreRepository.GetGenre(It.IsAny<string>())).Returns(genre);

			GenreDto result = _genreService.GetGenre("genre-1");

			Assert.AreEqual(genre.GenreId, result.GenreId);
			Assert.AreEqual(genre.Name, result.Name);
			Assert.IsFalse(result.IsReadOnly);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
		public void GetGenre_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(s => s.GenreRepository.GetGenre(It.IsAny<string>())).Throws<Exception>();

			_genreService.GetGenre("genre-1");
		}

		#endregion

		#region Create method

		[TestMethod]
		public void Create_Maps_Genre_Appropriately()
		{
			Genre genreResult = null;

			var genreDto = new GenreDto
			{
				GenreId = 1,
				Name = "genre-1"
			};

			_mockUnitOfWork.Setup(m => m.GenreRepository.Add(It.IsAny<Genre>())).Callback<Genre>(genre =>
			{
				genreResult = genre;
			});

			_genreService.Create(genreDto, "parent");

			Assert.AreEqual(genreDto.GenreId, genreResult.GenreId);
			Assert.AreEqual(genreDto.Name, genreResult.Name);
			Assert.AreEqual(DatabaseName.GameStore, genreResult.Database);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Create_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.Add(It.IsAny<Genre>())).Throws<Exception>();

			_genreService.Create(new GenreDto(), "parent");
		}

		[TestMethod]
		public void Create_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.Add(It.IsAny<Genre>()));

			_genreService.Create(new GenreDto(), "parent");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region Update method

		[TestMethod]
		public void Update_Maps_Genre_Appropriately()
		{
			Genre genreResult = null;

			var genreDto = new GenreDto
			{
				GenreId = 1,
				Name = "genre-1"
			};

			_mockUnitOfWork.Setup(m => m.GenreRepository.Get(It.IsAny<int>())).Returns(new Genre());
			_mockUnitOfWork.Setup(m => m.GenreRepository.Update(It.IsAny<Genre>())).Callback<Genre>(genre =>
			{
				genreResult = genre;
			});

			_genreService.Update(genreDto, "parent");

			Assert.AreEqual(genreDto.Name, genreResult.Name);
			Assert.AreEqual(DatabaseName.GameStore, genreResult.Database);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Update_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.Get(It.IsAny<int>())).Returns(new Genre());
			_mockUnitOfWork.Setup(m => m.GenreRepository.Update(It.IsAny<Genre>())).Throws<Exception>();

			_genreService.Update(new GenreDto(), "parent");
		}

		[TestMethod]
		public void Update_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.Get(It.IsAny<int>())).Returns(new Genre());

			_genreService.Update(new GenreDto(), "parent");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region Delete method

		[TestMethod]
		public void Delete_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.GetGenre(It.IsAny<string>())).Returns(new Genre());

			_genreService.Delete("genre-1");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Delete_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.GenreRepository.GetGenre(It.IsAny<string>())).Returns(new Genre());
			_mockUnitOfWork.Setup(m => m.GenreRepository.Delete(It.IsAny<Genre>())).Throws<Exception>();

			_genreService.Delete("genre-name");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Delete_Throws_ArgumentNullException_When_Genre_Name_Is_Null_Or_Empty()
		{
			_genreService.Delete(string.Empty);
		}

		#endregion
	}
}