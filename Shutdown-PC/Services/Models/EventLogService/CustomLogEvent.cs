using Serilog.Events;

namespace ShutdownPC.Services.Models.EventLogService
{
	public class CustomLogEvent
	{
		public string Message { get; set; }
		public LogEventLevel Level { get; set; }
		public DateTime Timestamp { get; set; }
		public Guid GuidID { get; set; }
		public string Version { get; set; }
		public string Exception { get; set; }

		public CustomLogEvent() { }
		public CustomLogEvent(Guid GuidID, string message, LogEventLevel level, string version)
		{
			Timestamp = DateTime.Now;
			Message = message;
			Level = level;
			Version = version;
		}
	}
}
