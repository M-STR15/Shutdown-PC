using Serilog;
using Serilog.Events;
using ShutdownPC.Models;
using ShutdownPC.Services.Models.EventLogService;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ShutdownPC.Services
{
	/// <summary>
	/// Event logger pro aktulní aplikaci.Za použití služby serilogu.
	/// Informace se ukládají do složky, v kořenu aplikace do složky "logs" s koncovnou ".log".
	/// </summary>
	public class EventLogService : IDisposable
	{
		private readonly string _version;
		private readonly string _assemblyName;

		public EventLogService()
		{
			_version = BuildInfo.VersionStr;

			var assembly = Assembly.GetExecutingAssembly();
			_assemblyName = assembly?.GetName()?.Name ?? "";
			var format = new CustomLogEvent().GetFormat();

			Log.Logger = new LoggerConfiguration()
			  .WriteTo.File(
				  path: $"logs/{_assemblyName}.log", // Cesta k souboru
				  rollingInterval: RollingInterval.Day, // Denní archivace
				  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz};[{Level}];{Message:lj}{NewLine}", //, // Vlastní formát
				  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information // Minimální úroveň logování
			  )
			  .CreateLogger();
		}

		~EventLogService()
		{
			Dispose();
		}

		public void Dispose()
		{
			Log.CloseAndFlush();
		}

		public void Information(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Information, _version));

		public void Warning(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Warning, _version));

		public void Error(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Error, _version));

		public void Fatal(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Fatal, _version));

		private void writeEvent(CustomLogEvent customLogEvent)
		{
			Log.Write(customLogEvent.Level, JsonSerializer.Serialize(customLogEvent));
		}

		public List<CustomLogEvent> ReadEventLogs()
		{
			var events = new List<CustomLogEvent>();
			string logFilePath = $"logs/{_assemblyName}{DateTime.Now.ToString("yyyyMMdd")}.log";

			using (FileStream fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (StreamReader sr = new StreamReader(fs))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					var message = ParseLogEntry(line);
					if (message != null && message != "")
						events.Add(JsonSerializer.Deserialize<CustomLogEvent>(message));
				}
			}

			return events;
		}

		private static string ParseLogEntry(string logLine)
		{
			// Předpoklad: Logovací formát je "Timestamp [Level] Message"
			var parts = logLine.Split(';', 3);  // Rozdělíme na 3 části: Timestamp, Level, a Message
			if (parts.Length < 3) return "";

			string timestamp = parts[0];  // Spojíme datum a čas
			string level = parts[1].Trim('[', ']');  // Vyčistíme log level
			string message = parts[2];  // Zbytek je zpráva

			//Console.WriteLine($"Timestamp: {timestamp}, Level: {level}, Message: {message}");

			return message;
		}
	}
}