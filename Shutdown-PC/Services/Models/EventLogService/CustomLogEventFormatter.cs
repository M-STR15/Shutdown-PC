using Serilog.Events;
using Serilog.Formatting;
using System.IO;

namespace ShutdownPC.Services.Models.EventLogService
{
	public class CustomLogEventFormatter : ITextFormatter
	{
		public void Format(LogEvent logEvent, TextWriter output)
		{
			var customLog = new CustomLogEvent
			{
				Message = logEvent.MessageTemplate.Text,
				Level = logEvent.Level,
				Timestamp = logEvent.Timestamp.UtcDateTime,
				Exception = logEvent.Exception?.ToString()
			};

			// Přizpůsobte formátování dle potřeby
			output.WriteLine($"{customLog.Timestamp} [{customLog.Level}] {customLog.Message}");
			if (!string.IsNullOrEmpty(customLog.Exception))
			{
				output.WriteLine(customLog.Exception);
			}
		}
	}
}
