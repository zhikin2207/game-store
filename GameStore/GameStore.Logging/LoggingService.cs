using System;
using System.Text;
using GameStore.Logging.Interfaces;
using NLog;

namespace GameStore.Logging
{
	public class LoggingService : ILoggingService
	{
		private readonly Logger _logger;
		private readonly string _loggerName;

		public LoggingService(string loggerName)
		{
			_loggerName = loggerName;
			_logger = LogManager.GetLogger(_loggerName);
		}

		public void Log(LoggingLevel level, string format, params object[] args)
		{
			_logger.Log(LogLevelConverter(level), format, args);
		}

		public void Log(Exception exception, LoggingLevel level, string format, params object[] args)
		{
			LogEventInfo logEvent = GetLogEvent(_loggerName, LogLevelConverter(level), exception, format, args);
			_logger.Log(logEvent);
		}

		private LogLevel LogLevelConverter(LoggingLevel level)
		{
			switch (level)
			{
				case LoggingLevel.Info:
					return LogLevel.Info;
				case LoggingLevel.Trace:
					return LogLevel.Trace;
				case LoggingLevel.Warn:
					return LogLevel.Warn;
				case LoggingLevel.Fatal:
					return LogLevel.Fatal;
				case LoggingLevel.Error:
					return LogLevel.Error;
			}

			return LogLevel.Debug;
		}

		private LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
		{
			string trace = string.Empty;
			string messageProp = string.Empty;
			var innerMessageProp = new StringBuilder();

			var logEvent = new LogEventInfo(level, loggerName, string.Format(format, args));

			if (exception != null)
			{
				trace = exception.StackTrace;
				messageProp = exception.Message;

				Exception currentException = exception;
				for (int i = 1; currentException.InnerException != null; i++)
				{
					var tabs = new string('\t', i);

					innerMessageProp.AppendFormat(
						"{0}{1}\n",
						tabs,
						currentException.InnerException.Message.Replace("\n", tabs));

					currentException = currentException.InnerException;
				}
			}

			logEvent.Properties["error-trace"] = trace;
			logEvent.Properties["error-message"] = messageProp;
			logEvent.Properties["inner-error-message"] = innerMessageProp.ToString();

			return logEvent;
		}
	}
}