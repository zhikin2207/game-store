using System;
using System.Linq;
using System.Web.Mvc;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.Authentication.Components;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using GameStore.WebUI.ViewModels.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class RoleControllerTest
	{
		private Mock<IRoleService> _mockRoleService;
		private Mock<ILoggingService> _mockLogger;
		private Mock<IRoleViewModelBuilder> _mockViewModelBuilder;

		private RoleController _roleController;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockRoleService = new Mock<IRoleService>();
			_mockLogger = new Mock<ILoggingService>();
			_mockViewModelBuilder = new Mock<IRoleViewModelBuilder>();

			_roleController = new RoleController(_mockRoleService.Object, _mockViewModelBuilder.Object, _mockLogger.Object)
			{
				ControllerContext =
					ControllerContextMockBuilder.GetContextMock(AuthMockBuilder.Build(UserRole.Administrator).Get()).Object
			};
		}

		#region Roles action

		[TestMethod]
		public void Roles_Does_Not_Return_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildAllRolesViewModel()).Returns(Enumerable.Empty<RoleViewModel>());

			var result = _roleController.Roles() as ViewResult;

			Assert.IsNotNull(result);
		}

		#endregion

		#region Details action

		[TestMethod]
		public void Details_Does_Not_Return_Null()
		{
			_mockViewModelBuilder.Setup(m => m.BuildRoleViewModel(It.IsAny<int>())).Returns(new RoleViewModel());

			var result = _roleController.Details(1) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Details_Does_Not_Catch_Exceptions()
		{
			_mockViewModelBuilder.Setup(m => m.BuildRoleViewModel(It.IsAny<int>())).Throws<InvalidOperationException>();

			_roleController.Details(1);
		}

		#endregion

		#region Create action

		[TestMethod]
		public void Create_By_Get_Does_Not_Return_Null()
		{
			var result = _roleController.Create() as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Create_By_Post_Returns_View_When_Module_Not_Valid()
		{
			_roleController.ModelState.AddModelError(string.Empty, "error message");

			var result = _roleController.Create(new RoleViewModel()) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Create_By_Post_Rerdirects_To_Roles_When_Module_Valid()
		{
			var result = _roleController.Create(new RoleViewModel()) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Roles", result.RouteValues["action"]);
		}

		#endregion

		#region Update action

		[TestMethod]
		public void Update_By_Get_Does_Not_Return_Null()
		{
			var result = _roleController.Update(1) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Update_By_Get_Does_Not_Catch_Exceptions_When_Role_Does_Not_Exist()
		{
			_mockViewModelBuilder.Setup(m => m.BuildRoleViewModel(It.IsAny<int>())).Throws<InvalidOperationException>();

			_roleController.Update(1);
		}

		[TestMethod]
		public void Update_By_Post_Returns_View_When_Module_Not_Valid()
		{
			_roleController.ModelState.AddModelError(string.Empty, "error message");

			var result = _roleController.Update(new RoleViewModel()) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Update_By_Post_Rerdirects_To_Specified_Role_Details_When_Module_Valid()
		{
			var result = _roleController.Update(new RoleViewModel { RoleId = 1 }) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Details", result.RouteValues["action"]);
			Assert.AreEqual(1, result.RouteValues["RoleId"]);
		}

		#endregion

		#region Delete action

		[TestMethod]
		public void Delete_Does_Not_Return_Null()
		{
			var result = _roleController.Delete(1) as RedirectToRouteResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Delete_Does_Not_Catch_Exception_When_Game_Does_Not_Exist()
		{
			_mockRoleService.Setup(m => m.Delete(It.IsAny<int>())).Throws<InvalidOperationException>();

			_roleController.Delete(1);
		}

		[TestMethod]
		public void Delete_Redirects_To_Games_Action_After_Successfull_Deleting()
		{
			var result = _roleController.Delete(1) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Roles", result.RouteValues["action"].ToString());
		}

		#endregion
	}
}
