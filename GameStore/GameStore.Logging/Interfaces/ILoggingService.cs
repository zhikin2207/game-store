using System;

namespace GameStore.Logging.Interfaces
{
	public interface ILoggingService
	{
		/// <summary>
		/// Log message.
		/// </summary>
		/// <param name="level">Log level</param>
		/// <param name="format">String format</param>
		/// <param name="args">Log arguments</param>
		void Log(LoggingLevel level, string format, params object[] args);

		/// <summary>
		/// Log exception and message.
		/// </summary>
		/// <param name="exception">Log exception</param>
		/// <param name="level">Log level</param>
		/// <param name="format">String format</param>
		/// <param name="args">Log arguments</param>
		void Log(Exception exception, LoggingLevel level, string format, params object[] args);
	}
}
