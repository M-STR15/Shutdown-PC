using ShutdownPC.Services.Models.EventLogService;

namespace ShutdownPC.Services
{
	public interface IEventLogService
	{
		void Dispose();
		void WriteError(Guid guid, string message);
		void WriteFatal(Guid guid, string message);
		void WriteInformation(Guid guid, string message);
		string ParseLogEntry(string logLine);
		List<CustomLogEvent> ReadEventLogs();
		void WriteWarning(Guid guid, string message);
	}
}