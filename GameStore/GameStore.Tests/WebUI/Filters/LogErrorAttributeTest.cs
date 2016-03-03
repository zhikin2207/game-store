using System;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.Logging;
using GameStore.Logging.Interfaces;
using GameStore.WebUI.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.WebUI.Filters
{
	[TestClass]
	public class LogErrorAttributeTest
	{
		private Mock<ExceptionContext> _mockContext;
		private Mock<ILoggingService> _mockLogger;
		private LogErrorAttribute _filter;

		[TestInitialize]
		public void InitializeTest()
		{
			_mockContext = new Mock<ExceptionContext>();
			_mockContext.SetupGet(c => c.Exception).Returns(new Exception(string.Empty));
			_mockContext.SetupGet(c => c.HttpContext.Response);
			_mockContext.Setup(c => c.HttpContext.Response.Clear());

			_mockLogger = new Mock<ILoggingService>();

			_filter = new LogErrorAttribute
			{
				LoggingService = _mockLogger.Object
			};
		}

		#region OnException method

		[TestMethod]
		public void OnException_Exits_When_Exception_Is_Handled()
		{
			_mockContext.Object.ExceptionHandled = true;

			_filter.OnException(_mockContext.Object);

			Assert.IsInstanceOfType(_mockContext.Object.Result, typeof(EmptyResult));
		}

		[TestMethod]
		public void OnException_Exits_When_Instance_Is_Not_Of_Required_Type()
		{
			_filter.ExceptionType = typeof(ArgumentException);
			_filter.OnException(_mockContext.Object);

			Assert.IsInstanceOfType(_mockContext.Object.Result, typeof(EmptyResult));
		}

		[TestMethod]
		public void OnException_Sets_Filter_Context_Result_To_ViewResult()
		{
			_filter.OnException(_mockContext.Object);

			Assert.IsNotNull(_mockContext.Object.Result);
			Assert.IsInstanceOfType(_mockContext.Object.Result, typeof(ViewResult));
		}

		[TestMethod]
		public void OnException_Sets_Filter_Context_Result_To_ViewResult_With_Correct_View_Data()
		{
			_filter.View = "view";
			_filter.Master = "master";
			_filter.OnException(_mockContext.Object);

			var result = _mockContext.Object.Result as ViewResult;
			Assert.IsNotNull(result);
			Assert.AreEqual(_filter.View, result.ViewName);
			Assert.AreEqual(_filter.Master, result.MasterName);
		}

		[TestMethod]
		public void OnException_Calls_Loggers_Log_Method()
		{
			_filter.OnException(_mockContext.Object);

			_mockLogger.Verify(
				l => l.Log(It.IsAny<Exception>(), It.IsAny<LoggingLevel>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
		}

		[TestMethod]
		public void OnException_Indicates_That_Exception_Is_Handled()
		{
			_filter.OnException(_mockContext.Object);

			Assert.IsTrue(_mockContext.Object.ExceptionHandled);
		}

		[TestMethod]
		public void OnException_Writes_500_Status_Code_To_Response()
		{
			_mockContext.SetupProperty(p => p.HttpContext.Response.StatusCode);

			_filter.OnException(_mockContext.Object);

			Assert.AreEqual(500, _mockContext.Object.HttpContext.Response.StatusCode);
		}

		[TestMethod]
		public void OnException_Gets_Actual_Parameter_When_It_Is_Set()
		{
			string testMessage = null;
			var routeData = new RouteData();
			routeData.Values["key"] = "data-value";

			_mockContext.SetupGet(p => p.RouteData).Returns(routeData);

			_mockLogger
				.Setup(m => m.Log(It.IsAny<Exception>(), It.IsAny<LoggingLevel>(), It.IsAny<string>(), It.IsAny<object[]>()))
				.Callback<Exception, LoggingLevel, string, object[]>((ex, level, message, parameters) =>
				{
					testMessage = message;
				});

			_filter.Parameter = "key";
			_filter.LogMessage = "{0}";
			_filter.OnException(_mockContext.Object);

			Assert.AreEqual("data-value", testMessage);
		}

		#endregion

		#region LogMessage property

		[TestMethod]
		public void LogMessage_Returns_Default_Log_Message_When_It_Was_Not_Set()
		{
			Assert.AreEqual(LogErrorAttribute.DefaultLogMessage, _filter.LogMessage);
		}

		#endregion

		#region View property

		[TestMethod]
		public void View_Returns_Default_View_When_It_Was_Not_Set()
		{
			Assert.AreEqual(LogErrorAttribute.DefaultViewName, _filter.View);
		}

		#endregion
	}
}