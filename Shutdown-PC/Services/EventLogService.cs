using Serilog;
using Serilog.Events;
using ShutdownPC.Models;
using ShutdownPC.Services.Models.EventLogService;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;


namespace ShutdownPC.Services
{
	public class EventLogService : IDisposable
	{
		private readonly string _version;
		public EventLogService()
		{
			_version = BuildInfo.VersionStr;

			var assembly = Assembly.GetExecutingAssembly();
			string assemblyName = assembly?.GetName()?.Name ?? "";

			if (!EventLog.SourceExists(assemblyName))
			{
				EventLog.CreateEventSource(assemblyName, "Application");
			}
			Log.Logger = new LoggerConfiguration()
				  .Enrich.WithProperty("SoftwareVersion", _version)
				  .Enrich.FromLogContext()
				  .WriteTo.Sink(new CustomEventLogSink(assemblyName))
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
	}
}
