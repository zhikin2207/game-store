using System.Web.Mvc;
using GameStore.Logging;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Filters
{
	[TestClass]
	public class LogIpFilterTest
	{
		private Mock<ActionExecutingContext> _mockContext;
		private Mock<ILoggingService> _mockLogger;
		private LogIpFilter _filter;

		[TestInitialize]
		public void InitializeTest()
		{
			_mockContext = new Mock<ActionExecutingContext>();

			_mockLogger = new Mock<ILoggingService>();

			_filter = new LogIpFilter
			{
				LoggingService = _mockLogger.Object
			};

			PupulateContext();
		}

		[TestMethod]
		public void OnActionExecuting_Writes_Controller_Name_To_Log_File()
		{
			string result = null;
			_mockLogger
				.Setup(m => m.Log(It.IsAny<LoggingLevel>(), It.IsAny<string>(), It.IsAny<object[]>()))
				.Callback<LoggingLevel, string, object[]>(
					(level, format, parameters) =>
					{
						result = string.Format(format, parameters);
					});

			_filter.OnActionExecuting(_mockContext.Object);

			Assert.IsTrue(result.Contains("controller-name"));
		}

		[TestMethod]
		public void OnActionExecuting_Writes_Action_Name_To_Log_File()
		{
			string result = null;
			_mockLogger
				.Setup(m => m.Log(It.IsAny<LoggingLevel>(), It.IsAny<string>(), It.IsAny<object[]>()))
				.Callback<LoggingLevel, string, object[]>(
					(level, format, parameters) =>
					{
						result = string.Format(format, parameters);
					});

			_filter.OnActionExecuting(_mockContext.Object);

			Assert.IsTrue(result.Contains("action-name"));
		}

		[TestMethod]
		public void OnActionExecuting_Writes_IP_Address_To_Log_File()
		{
			string result = null;
			_mockLogger
				.Setup(m => m.Log(It.IsAny<LoggingLevel>(), It.IsAny<string>(), It.IsAny<object[]>()))
				.Callback<LoggingLevel, string, object[]>(
					(level, format, parameters) =>
					{
						result = string.Format(format, parameters);
					});

			_filter.OnActionExecuting(_mockContext.Object);

			Assert.IsTrue(result.Contains("address"));
		}

		private void PupulateContext()
		{
			_mockContext.SetupGet(p => p.ActionDescriptor.ControllerDescriptor.ControllerName).Returns("controller-name");
			_mockContext.SetupGet(p => p.ActionDescriptor.ActionName).Returns("action-name");
			_mockContext.SetupGet(p => p.HttpContext.Request.UserHostAddress).Returns("address");
		}
	}
}
