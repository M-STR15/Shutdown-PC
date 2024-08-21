using ShutdownPC.Services.Models.EventLogService;

namespace ShutdownPC.Services
{
	public interface IEventLogService
	{
		void Dispose();
		void Error(Guid guid, string message);
		void Fatal(Guid guid, string message);
		void Information(Guid guid, string message);
		string ParseLogEntry(string logLine);
		List<CustomLogEvent> ReadEventLogs();
		void Warning(Guid guid, string message);
	}
}