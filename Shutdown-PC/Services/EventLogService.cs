﻿using Serilog;
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
	public class EventLogService : IDisposable, IEventLogService
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

		public void WriteInformation(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Information, _version));

		/// <summary>
		/// Zapíše varovnou zprávu do logu.
		/// </summary>
		/// <param name="guid">Identifikátor události.</param>
		/// <param name="message">Zpráva, která má být zapsána do logu.</param>
		public void WriteWarning(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Warning, _version));

		/// <summary>
		/// Zapíše chybovou zprávu do logu.
		/// </summary>
		/// <param name="guid">Identifikátor události.</param>
		/// <param name="message">Zpráva, která má být zapsána do logu.</param>
		public void WriteError(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Error, _version));

		/// <summary>
		/// Zapíše fatální chybovou zprávu do logu.
		/// </summary>
		/// <param name="guid">Identifikátor události.</param>
		/// <param name="message">Zpráva, která má být zapsána do logu.</param>
		public void WriteFatal(Guid guid, string message) => writeEvent(new CustomLogEvent(guid, message, LogEventLevel.Fatal, _version));

		private void writeEvent(CustomLogEvent customLogEvent)
		{
			Log.Write(customLogEvent.Level, JsonSerializer.Serialize(customLogEvent));
		}

		/// Metoda pro čtení logovacích záznamů z logovacího souboru.
		/// Předpokládá, že formát logu je "Timestamp;[Level];Message".
		/// Načte logovací soubor pro aktuální den, parsuje jednotlivé řádky a deserializuje je do objektů CustomLogEvent.
		/// Pokud záznam neobsahuje platnou zprávu, je ignorován.
		/// </summary>
		/// <returns>Vrací seznam objektů CustomLogEvent.</returns>
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

		/// <summary>
		/// Metoda pro parsování logovacího záznamu.
		/// Předpokládá, že formát logu je "Timestamp;[Level];Message".
		/// Rozdělí záznam na tři části: Timestamp, Level a Message.
		/// Pokud záznam neobsahuje alespoň tři části, vrátí prázdný řetězec.
		/// </summary>
		/// <param name="logLine">Řádek logu, který má být parsován.</param>
		/// <returns>Vrací zprávu z logu jako řetězec.</returns>
		public string ParseLogEntry(string logLine)
		{
			// Předpoklad: Logovací formát je "Timestamp;[Level];Message"
			var parts = logLine.Split(';', 3);  // Rozdělíme na 3 části: Timestamp, Level, a Message
			if (parts.Length < 3)
				return "";

			string timestamp = parts[0];  // Spojíme datum a čas
			string level = parts[1].Trim('[', ']');  // Vyčistíme log level
			string message = parts[2];  // Zbytek je zpráva

			//Console.WriteLine($"Timestamp: {timestamp}, Level: {level}, Message: {message}");

			return message;
		}
	}
}
