using System;
using System.Linq;
using System.Web.Mvc;
using GameStore.BLL.DTOs;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using GameStore.WebUI.ViewModels.Publisher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class PublisherControllerTest
	{
		private Mock<IPublisherService> _mockPublisherService;
		private Mock<ILoggingService> _mockLogger;
		private Mock<IPublisherViewModelBuilder> _mockViewModelBuilder;

		private PublisherController _publisherController;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockPublisherService = new Mock<IPublisherService>();
			_mockLogger = new Mock<ILoggingService>();
			_mockViewModelBuilder = new Mock<IPublisherViewModelBuilder>();

			_publisherController = new PublisherController(_mockPublisherService.Object, _mockViewModelBuilder.Object, _mockLogger.Object)
			{
				ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Guest).Get()).Object
			};
		}

		#region Publishers action

		[TestMethod]
		public void Publishers_Does_Not_Return_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildPublishersViewModel()).Returns(Enumerable.Empty<PublisherViewModel>());

			var result = _publisherController.Publishers() as ViewResult;

			Assert.IsNotNull(result);
		}

		#endregion

		#region PublisherDetails action

		[TestMethod]
		public void PublisherDetails_Action_Does_Not_Return_Null()
		{
			_mockPublisherService
				.Setup(s => s.GetPublisher(It.IsAny<string>()))
				.Returns(new PublisherDto());

			ActionResult result = _publisherController.Details("game-key");

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void PublisherDetails_Action_Does_Not_Catch_Exception_When_Publisher_Does_Not_Exist()
		{
			_mockViewModelBuilder.Setup(m => m.BuildPublisherViewModel(It.IsAny<string>()))
				.Throws<InvalidOperationException>();

			_publisherController.Details("game-key");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PublisherDetails_Action_Does_Not_Catch_Exception_When_CompanyName_Is_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildPublisherViewModel(It.IsAny<string>()))
				.Throws<ArgumentNullException>();

			_publisherController.Details(null);
		}

		#endregion

		#region New action

		[TestMethod]
		public void New_By_Get_Method_Does_Not_Return_Null()
		{
			var result = _publisherController.New() as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void New_By_Post_Method_Returns_ViewResult_If_Model_Has_Errors()
		{
			_publisherController.ModelState.AddModelError(string.Empty, string.Empty);

			ActionResult result = _publisherController.New(new CreateEditPublisherViewModel());

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public void New_By_Post_Method_Maps_Publisher_Appropriately()
		{
			var publisher = new CreateEditPublisherViewModel
			{
				CurrentPublisher = new PublisherViewModel
				{
					PublisherId = 1,
					CompanyName = "companyName",
					Description = "description",
					HomePage = "home-page"
				}
			};

			PublisherDto publisherDto = null;
			_mockPublisherService
				.Setup(s => s.Create(It.IsAny<PublisherDto>()))
				.Callback<PublisherDto>(item => publisherDto = item);

			_publisherController.New(publisher);

			Assert.AreEqual(publisher.CurrentPublisher.PublisherId, publisherDto.PublisherId);
			Assert.AreEqual(publisher.CurrentPublisher.CompanyName, publisherDto.CompanyName);
			Assert.AreEqual(publisher.CurrentPublisher.Description, publisherDto.Description);
			Assert.AreEqual(publisher.CurrentPublisher.HomePage, publisherDto.HomePage);
		}

		[TestMethod]
		public void New_By_Post_Method_Redirects_To_AllGames_Action_After_Successfull_Creating()
		{
			_mockPublisherService.Setup(s => s.Create(It.IsAny<PublisherDto>()));

			var result = _publisherController.New(new CreateEditPublisherViewModel()) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Publishers", result.RouteValues["action"].ToString());
			Assert.AreEqual("Publisher", result.RouteValues["controller"].ToString());
		}

		//[TestMethod]
		//public void New_By_Post_Method_Adds_Module_Error_When_Publisher_With_Specified_Name_Exists()
		//{
		//	_mockPublisherService.Setup(m => m.IsExist(It.IsAny<string>())).Returns(true);

		//	_publisherController.New(new CreateEditPublisherViewModel());

		//	Assert.IsTrue(_publisherController.ModelState.Keys.Contains("CompanyName"));
		//}

		#endregion

		#region Update action

		[TestMethod]
		public void Update_By_Get_Method_Redirects_To_Publishers_When_User_Not_Manager()
		{
			_mockViewModelBuilder.Setup(m => m.BuildUpdatePublisherViewModel(It.IsAny<string>())).Returns(new CreateEditPublisherViewModel());

			var result = _publisherController.Update("publisher") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Publishers", result.RouteValues["action"]);
		}

		[TestMethod]
		public void Update_By_Get_Method_Redirects_To_Publishers_When_User_Not_Specific_Publisher()
		{
			_mockViewModelBuilder.Setup(m => m.BuildUpdatePublisherViewModel(It.IsAny<string>())).Returns(new CreateEditPublisherViewModel());
			_publisherController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Publisher).SetPublisher("publisher").Get()).Object;

			var result = _publisherController.Update("not-publisher") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Publishers", result.RouteValues["action"]);
		}

		[TestMethod]
		public void Update_By_Get_Method_Returns_View_When_User_Manager()
		{
			_publisherController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Manager).Get()).Object;

			var result = _publisherController.Update("publisher") as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Update_By_Get_Method_Returns_View_When_User_Specific_Publisher()
		{
			_mockViewModelBuilder
				.Setup(m => m.BuildUpdatePublisherViewModel(It.IsAny<string>()))
				.Returns(new CreateEditPublisherViewModel {CurrentPublisher = new PublisherViewModel{CompanyName = "publisher" }});

			_publisherController.ControllerContext = ControllerContextMockBuilder.GetContextMock(AuthMockBuilder
					.Build(UserRole.Publisher)
					.SetPublisher("publisher")
					.Get()).Object;

			var result = _publisherController.Update("publisher") as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Update_By_Get_Does_Not_Catch_Exceptions()
		{
			_mockViewModelBuilder
				.Setup(m => m.BuildUpdatePublisherViewModel(It.IsAny<string>()))
				.Throws(new InvalidOperationException());

			_publisherController.Update("publisher");
		}

		[TestMethod]
		public void Update_By_Post_Method_Returns_ViewResult_If_Model_Has_Errors()
		{
			_publisherController.ModelState.AddModelError(string.Empty, string.Empty);

			var result = _publisherController.Update(new CreateEditPublisherViewModel(), "publisher") as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Update_By_Post_Method_Maps_Publisher_Appropriately()
		{
			var publisher = new CreateEditPublisherViewModel
			{
				CurrentPublisher = new PublisherViewModel
				{
					CompanyName = "publisher",
					Description = "publisher-description",
					HomePage = "page",
					UserGuid = Guid.NewGuid(),
					PublisherId = 1
				}
			};

			PublisherDto publisherDto = null;

			_mockPublisherService
				.Setup(s => s.Update(It.IsAny<PublisherDto>()))
				.Callback<PublisherDto>(item => publisherDto = item);

			_publisherController.Update(publisher, "publisher");

			Assert.AreEqual(publisher.CurrentPublisher.PublisherId, publisherDto.PublisherId);
			Assert.AreEqual(publisher.CurrentPublisher.CompanyName, publisherDto.CompanyName);
			Assert.AreEqual(publisher.CurrentPublisher.Description, publisherDto.Description);
			Assert.AreEqual(publisher.CurrentPublisher.HomePage, publisherDto.HomePage);
			Assert.AreEqual(publisher.CurrentPublisher.UserGuid, publisherDto.UserGuid);
		}

		[TestMethod]
		public void Update_By_Post_Method_Redirects_To_Publishers_After_Successfull_Updating()
		{
			var result = _publisherController.Update(new CreateEditPublisherViewModel
			{
				CurrentPublisher = new PublisherViewModel
				{
					CompanyName = "publisher"
				}
			}, "publisher") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Publishers", result.RouteValues["action"].ToString());
		}

		//[TestMethod]
		//public void Update_By_Post_Method_Adds_Module_Error_When_Publisher_With_Specified_Name_Exists()
		//{
		//	_mockPublisherService.Setup(m => m.IsExist(It.IsAny<string>())).Returns(true);

		//	_publisherController.Update(new CreateEditPublisherViewModel
		//	{
		//		CurrentPublisher = new PublisherViewModel
		//		{
		//			CompanyName = "publisher"
		//		}
		//	}, "another-publisher");

		//	Assert.IsNotNull(_publisherController.ModelState.Keys.Contains("CompanyName"));
		//}

		#endregion

		#region Delete action

		[TestMethod]
		public void Delete_Does_Not_Return_Null()
		{
			var result = _publisherController.Delete("publisher") as RedirectToRouteResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Delete_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockPublisherService.Setup(s => s.Delete(It.IsAny<string>())).Throws<InvalidOperationException>();

			_publisherController.Delete("publisher");
		}

		[TestMethod]
		public void Delete_Redirects_To_Games_Action_After_Successfull_Deleting()
		{
			var result = _publisherController.Delete("publisher") as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Publishers", result.RouteValues["action"].ToString());
		}

		#endregion
	}
}