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
		private readonly EventLog _eventLog;

		public CustomEventLogSink(string source)
		{
			_source = source;
			if (!EventLog.SourceExists(source))
			{
				EventLog.CreateEventSource(source, "Application");
			}
			_eventLog = new EventLog("Application") { Source = source };
		}

		public void Emit(LogEvent logEvent)
		{
			var sb = new StringBuilder();
			sb.AppendLine("<Event xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\">");
			sb.AppendLine("<System>");
			sb.AppendLine($"<Provider Name=\"{_source}\" />");
			sb.AppendLine($"<EventID>{logEvent.GetHashCode()}</EventID>");
			sb.AppendLine($"<Version>{logEvent.Properties.GetValueOrDefault("SoftwareVersion")?.ToString() ?? "Unknown"}</Version>"); // Přidání verze jako statické hodnoty
			sb.AppendLine("</System>");
			sb.AppendLine("<EventData>");
			sb.AppendLine($"<Data Name=\"Message\">{logEvent.RenderMessage()}</Data>");
			sb.AppendLine($"<Data Name=\"SoftwareVersion\">{logEvent.Properties.GetValueOrDefault("SoftwareVersion")?.ToString() ?? "Unknown"}</Data>");
			sb.AppendLine("</EventData>");
			sb.AppendLine("</Event>");

			var result= sb.ToString();
			_eventLog.WriteEntry(sb.ToString(), GetEventLogEntryType(logEvent.Level));
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
