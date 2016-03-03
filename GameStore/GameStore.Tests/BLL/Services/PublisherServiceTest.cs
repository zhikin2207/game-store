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
	public class PublisherServiceTest
	{
		private Mock<IGameStoreUnitOfWork> _mockUnitOfWork;
		private IPublisherService _publisherService;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingBllProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUnitOfWork = new Mock<IGameStoreUnitOfWork>();
			_publisherService = new PublisherService(_mockUnitOfWork.Object);
		}

		#region GetPublishers method

		[TestMethod]
		public void GetPublishers_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetList()).Returns(Enumerable.Empty<Publisher>());

			IEnumerable<PublisherDto> publishers = _publisherService.GetPublishers();

			Assert.IsNotNull(publishers);
		}

		[TestMethod]
		public void GetPublishers_Maps_Publishers_Appropriately()
		{
			IEnumerable<Publisher> publishers = new[]
			{
				new Publisher { PublisherId = 1, CompanyName = "publisher-1", Description = "publisher-1", HomePage = "homepage-1" },
				new Publisher { PublisherId = 2, CompanyName = "publisher-2", Description = "publisher-2", HomePage = "homepage-2" },
				new Publisher { PublisherId = 2, CompanyName = "publisher-3", Description = "publisher-3", HomePage = "homepage-3" }
			};

			_mockUnitOfWork.Setup(u => u.PublisherRepository.GetList()).Returns(publishers);

			IEnumerable<PublisherDto> publisherDtos = _publisherService.GetPublishers();

			Assert.AreEqual(3, publisherDtos.Count());
			for (int i = 0; i < publisherDtos.Count(); i++)
			{
				Assert.AreEqual(publishers.ElementAt(i).PublisherId, publisherDtos.ElementAt(i).PublisherId);
				Assert.AreEqual(publishers.ElementAt(i).CompanyName, publisherDtos.ElementAt(i).CompanyName);
				Assert.AreEqual(publishers.ElementAt(i).Description, publisherDtos.ElementAt(i).Description);
				Assert.AreEqual(publishers.ElementAt(i).HomePage, publisherDtos.ElementAt(i).HomePage);
			}
		}

		#endregion

		#region IsExist method

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsExist_Throws_ArgumentNullException_When_Company_Name_Is_Not_Set()
		{
			_publisherService.IsExist(null);
		}

		[TestMethod]
		public void IsExist_Returns_True_If_Company_Exists()
		{
			_mockUnitOfWork.Setup(u => u.PublisherRepository.IsExist(It.IsAny<string>())).Returns(true);

			bool result = _publisherService.IsExist("publisher-1");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsExist_Returns_False_If_Company_Does_Not_Exist()
		{
			_mockUnitOfWork.Setup(u => u.PublisherRepository.IsExist(It.IsAny<string>())).Returns(false);

			bool result = _publisherService.IsExist("publisher-1");

			Assert.IsFalse(result);
		}

		#endregion

		#region GetPublisher method

		[TestMethod]
		public void GetPublisher_Does_Not_Return_Null()
		{
			_mockUnitOfWork.Setup(s => s.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());

			PublisherDto result = _publisherService.GetPublisher("company");

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetPublisher_Maps_Publisher_Correctly()
		{
			var publisher = new Publisher
			{
				PublisherId = 1,
				CompanyName = "company-name",
				Description = "company-description",
				HomePage = "company-homePage"
			};

			_mockUnitOfWork.Setup(s => s.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(publisher);

			PublisherDto result = _publisherService.GetPublisher("company");

			Assert.AreEqual(publisher.PublisherId, result.PublisherId);
			Assert.AreEqual(publisher.CompanyName, result.CompanyName);
			Assert.AreEqual(publisher.Description, result.Description);
			Assert.AreEqual(publisher.HomePage, result.HomePage);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
		public void GetPublisher_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(s => s.PublisherRepository.GetPublisher(It.IsAny<string>())).Throws<Exception>();

			_publisherService.GetPublisher("company");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPublisher_Throws_ArgumentNullException_When_Company_Name_Is_Not_Set()
		{
			_publisherService.GetPublisher(null);
		}

		#endregion

		#region Create method

		[TestMethod]
		public void Create_Maps_Publisher_Correctly()
		{
			var publisherModel = new PublisherDto
			{
				PublisherId = 1,
				CompanyName = "company-name",
				Description = "company-description",
				HomePage = "company-homePage"
			};

			Publisher publisher = null;
			_mockUnitOfWork
				.Setup(s => s.PublisherRepository.Add(It.IsAny<Publisher>()))
				.Callback<Publisher>(item => publisher = item);

			_publisherService.Create(publisherModel);

			Assert.AreEqual(publisherModel.PublisherId, publisher.PublisherId);
			Assert.AreEqual(publisherModel.CompanyName, publisher.CompanyName);
			Assert.AreEqual(publisherModel.Description, publisher.Description);
			Assert.AreEqual(publisherModel.HomePage, publisher.HomePage);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
		public void Create_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(s => s.PublisherRepository.Add(It.IsAny<Publisher>())).Throws<Exception>();

			_publisherService.Create(new PublisherDto());
		}

		[TestMethod]
		public void Create_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(s => s.PublisherRepository.Add(It.IsAny<Publisher>()));

			_publisherService.Create(new PublisherDto());

			_mockUnitOfWork.Verify(s => s.Save(), Times.Once());
		}

		#endregion

		#region Update method

		[TestMethod]
		public void Update_Maps_Publisher_Appropriately()
		{
			Publisher publisherResult = null;

			var publisherDto = new PublisherDto
			{
				PublisherId = 1,
				CompanyName = "publisher-1",
				Description = "publisher-1",
				HomePage = "homepage-1",
			};

			_mockUnitOfWork.Setup(m => m.PublisherRepository.Get(It.IsAny<int>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.PublisherRepository.Update(It.IsAny<Publisher>())).Callback<Publisher>(publisher =>
			{
				publisherResult = publisher;
			});

			_publisherService.Update(publisherDto);

			Assert.AreEqual(publisherDto.CompanyName, publisherResult.CompanyName);
			Assert.AreEqual(publisherDto.Description, publisherResult.Description);
			Assert.AreEqual(publisherDto.HomePage, publisherResult.HomePage);
			Assert.AreEqual(DatabaseName.GameStore, publisherResult.Database);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Update_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.Get(It.IsAny<int>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.PublisherRepository.Update(It.IsAny<Publisher>())).Throws<Exception>();

			_publisherService.Update(new PublisherDto());
		}

		[TestMethod]
		public void Update_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.Get(It.IsAny<int>())).Returns(new Publisher());

			_publisherService.Update(new PublisherDto());

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		#endregion

		#region Delete method

		[TestMethod]
		public void Delete_Calls_Save_Method()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());

			_publisherService.Delete("publisher-1");

			_mockUnitOfWork.Verify(m => m.Save(), Times.Once);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Delete_Does_Not_Catch_Exceptions()
		{
			_mockUnitOfWork.Setup(m => m.PublisherRepository.GetPublisher(It.IsAny<string>())).Returns(new Publisher());
			_mockUnitOfWork.Setup(m => m.PublisherRepository.Delete(It.IsAny<Publisher>())).Throws<Exception>();

			_publisherService.Delete("publisher-name");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Delete_Throws_ArgumentNullException_When_Publisher_Name_Is_Null_Or_Empty()
		{
			_publisherService.Delete(string.Empty);
		}

		#endregion
	}
}