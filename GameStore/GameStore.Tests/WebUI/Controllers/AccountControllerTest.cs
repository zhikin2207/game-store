using AutoMapper;
using GameStore.BLL.Services.Interfaces;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Controllers
{
	[TestClass]
	public class AccountControllerTest
	{
		private Mock<IUserService> _mockUserService;
		private Mock<ILoggingService> _mockLogger;
		private AccountController _accountController;

		[ClassInitialize]
		public static void InitializeClass(TestContext context)
		{
			Mapper.Initialize(cfg => cfg.AddProfile<MappingWebProfile>());
		}

		[TestInitialize]
		public void InitializeTest()
		{
			_mockLogger = new Mock<ILoggingService>();
			_mockUserService = new Mock<IUserService>();

			_accountController = new AccountController(_mockUserService.Object, _mockLogger.Object);
		}
	}
}