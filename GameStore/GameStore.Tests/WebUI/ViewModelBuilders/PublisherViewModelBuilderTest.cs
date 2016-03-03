using System;
using System.Collections.ObjectModel;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.ViewModelBuilders;
using GameStore.WebUI.ViewModels.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.ViewModelBuilders
{
	[TestClass]
	public class PublisherViewModelBuilderTest
	{
		private Mock<IPublisherService> _mockPublisherService;
		private PublisherViewModelBuilder _publisherViewModelBuilder;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockPublisherService = new Mock<IPublisherService>();

			_publisherViewModelBuilder = new PublisherViewModelBuilder(_mockPublisherService.Object);
		}

		#region BuildPublisherDetailsViewModel method

		[TestMethod]
		public void BuildPublisherDetailsViewModel_Does_Not_Return_Null()
		{
			_mockPublisherService.Setup(m => m.GetPublisher(It.IsAny<string>())).Returns(new PublisherDto() { PublisherLocalizations = new Collection<PublisherLocalizationDto>()});

			PublisherViewModel viewModel = _publisherViewModelBuilder.BuildPublisherViewModel("company-name");

			Assert.IsNotNull(viewModel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildPublisherDetailsViewModel_Throws_ArgumentNullException_When_Company_Name_Is_Null_Or_Empty()
		{
			_publisherViewModelBuilder.BuildPublisherViewModel(string.Empty);
		}

		[TestMethod]
		public void BuildPublisherDetailsViewModel_Maps_Publisher_Appropriately()
		{
			_mockPublisherService.Setup(m => m.GetPublisher(It.IsAny<string>())).Returns(new PublisherDto
			{
				CompanyName = "company-name",
				PublisherId = 0,
				PublisherLocalizations = new Collection<PublisherLocalizationDto>()
			});

			PublisherViewModel viewModel = _publisherViewModelBuilder.BuildPublisherViewModel("company-name");

			Assert.AreEqual("company-name", viewModel.CompanyName);
			Assert.AreEqual(0, viewModel.PublisherId);
		}

		#endregion
	}
}