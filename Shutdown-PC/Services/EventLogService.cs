using ShutdownPC.Models;
using ShutdownPC.Services.Models.EventLogService;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Text.Json;

namespace ShutdownPC.Services
{
	public class EventLogService : IDisposable
	{
		private readonly string _version;
		private readonly string _source;
		private readonly string _logName = "Application";
		public EventLogService()
		{
			_version = BuildInfo.VersionStr;

			var assembly = Assembly.GetExecutingAssembly();
			_source = assembly?.GetName()?.Name ?? "";
		}

		~EventLogService()
		{
			Dispose();
		}
		public void Dispose()
		{

		}

		public void Information(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, EventLogEntryType.Information, _version));

		public void Warning(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, EventLogEntryType.Warning, _version));

		public void Error(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, EventLogEntryType.Error, _version));

		private void writeEvent(CustomLogEvent customLogEvent)
		{
			using (EventLog eventLog = new EventLog(_logName))
			{
				eventLog.Source = _source;
				var jsonSerilize = JsonSerializer.Serialize(customLogEvent);
				eventLog.WriteEntry(jsonSerilize, customLogEvent.Level);
			}
		}

		public List<CustomLogEvent> ReadEventLogs()
		{
			var events = new List<CustomLogEvent>();
			var queryText=@"*[System[Provider[@Name='ShutdownPC']]]";
			// Vytvořte EventLogQuery pro získání událostí
			var query = new EventLogQuery(_logName, PathType.LogName, queryText);

			// Použití EventLogReader pro čtení událostí
			var reader = new EventLogReader(query);

			// Čtení událostí a zobrazení jejich obsahu
			EventRecord eventRecord;
			while ((eventRecord = reader.ReadEvent()) != null)
			{
				var message = eventRecord.FormatDescription();
				var eventData = JsonSerializer.Deserialize<CustomLogEvent>(message);

				events.Add(eventData);
			}

			return events;
		}
	}
}
