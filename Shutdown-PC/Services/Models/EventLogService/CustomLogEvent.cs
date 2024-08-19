using System.Diagnostics;

namespace ShutdownPC.Services.Models.EventLogService
{
	public class CustomLogEvent
	{
		public string Message { get; private set; }
		public EventLogEntryType Level { get; private set; }
		public DateTime Timestamp { get; private set; }
		public Guid GuidID { get; private set; }
		public string Version { get; private set; }
		public CustomLogEvent()
		{ }
		public CustomLogEvent(Guid guidID, string message, EventLogEntryType level, string version)
		{
			Timestamp = DateTime.Now;
			Message = message;
			Level = level;
			Version = version;
			GuidID = guidID;
		}
	}
}
