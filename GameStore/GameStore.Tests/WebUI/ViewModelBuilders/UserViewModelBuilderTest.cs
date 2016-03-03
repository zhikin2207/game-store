using GameStore.BLL.Services.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.ViewModelBuilders;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.ViewModelBuilders
{
	[TestClass]
	public class UserViewModelBuilderTest
	{
		private Mock<IUserService> _mockUserService;
		private IUserViewModelBuilder _userViewModelBuilder;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			AutoMapperConfig.RegisterMappings();
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockUserService = new Mock<IUserService>();
			_userViewModelBuilder = new UserViewModelBuilder(_mockUserService.Object);
		}
	}
}