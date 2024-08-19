using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ShutdownPC.Services.Models.EventLogService
{
	public class CustomEventLogSink : ILogEventSink
	{
		private readonly string _source;

		public CustomEventLogSink(string source)
		{
			_source = source;
			if (!EventLog.SourceExists(source))
			{
				EventLog.CreateEventSource(source, "Application");
			}
		}

		public void Emit(LogEvent logEvent)
		{
			var message = logEvent.RenderMessage();

			// Map Serilog log level to EventLogEntryType
			EventLogEntryType entryType;
			switch (logEvent.Level)
			{
				case LogEventLevel.Information:
					entryType = EventLogEntryType.Information;
					break;
				case LogEventLevel.Warning:
					entryType = EventLogEntryType.Warning;
					break;
				case LogEventLevel.Error:
				case LogEventLevel.Fatal:
					entryType = EventLogEntryType.Error;
					break;
				default:
					entryType = EventLogEntryType.Information;
					break;
			}

			// Write to Event Log
			using (EventLog eventLog = new EventLog("Application"))
			{
				eventLog.Source = _source;
				eventLog.WriteEntry(message, entryType);
			}
		}

		private EventLogEntryType GetEventLogEntryType(LogEventLevel level)
		{
			return level switch
			{
				LogEventLevel.Error => EventLogEntryType.Error,
				LogEventLevel.Warning => EventLogEntryType.Warning,
				LogEventLevel.Information => EventLogEntryType.Information,
				_ => EventLogEntryType.Information,
			};
		}
	}
}
