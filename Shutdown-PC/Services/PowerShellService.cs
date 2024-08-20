using ShutdownPC.Services.Models.PowerShell;
using System.Diagnostics;
using System.IO;

namespace ShutdownPC.Services
{
	/// <summary>
	/// Služba spouštěcí PowerShell scripty.
	/// </summary>
	public class PowerShellService
	{
		public void Run(string script)
		{
			ProcessStartInfo processInfo = new ProcessStartInfo();
			processInfo.FileName = "powershell.exe";
			processInfo.Arguments = $"-WindowStyle Hidden -Command \"{script}\"";
			processInfo.Verb = "runas";
			processInfo.UseShellExecute = false;
			processInfo.CreateNoWindow = true;

			try
			{
				Process process = Process.Start(processInfo);
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				var message = "Chyba při spuštění PowerShell skriptu.";
#if DEBUG
				message = $"Chyba při spuštění PowerShell skriptu:{ex.Message}";
#endif

				throw new Exception(message);
			}
		}

		public void RunForPath(string scriptPath)
		{
			ProcessStartInfo processInfo = new ProcessStartInfo();
			processInfo.FileName = "powershell.exe";
			processInfo.Arguments = $"-WindowStyle Hidden -File \"{scriptPath}\"";
			processInfo.Verb = "runas";
			processInfo.UseShellExecute = false;
			processInfo.CreateNoWindow = true;

			try
			{
				Process process = Process.Start(processInfo);
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				var message = "Chyba při spuštění PowerShell skriptu.";
#if DEBUG
				message = $"Chyba při spuštění PowerShell skriptu:{ex.Message}";
#endif

				throw new Exception(message);
			}
		}

		public Tuple<eResultOutputStatus, string> RunFromPathWithResult(string scriptPath)
		{
			ProcessStartInfo processInfo = new ProcessStartInfo();
			processInfo.FileName = "powershell.exe";
			processInfo.Arguments = $"-WindowStyle Hidden -File \"{scriptPath}\"";
			processInfo.Verb = "runas";
			processInfo.RedirectStandardOutput = true;
			processInfo.RedirectStandardError = true;
			processInfo.UseShellExecute = false;
			processInfo.CreateNoWindow = true;

			try
			{
				using (Process process = Process.Start(processInfo))
				{
					using (StreamReader outputReader = process.StandardOutput)
					using (StreamReader errorReader = process.StandardError)
					{
						string output = outputReader.ReadToEnd();
						string error = errorReader.ReadToEnd();

						process.WaitForExit();

						if (process.ExitCode == 0)
						{
							return new Tuple<eResultOutputStatus, string>(eResultOutputStatus.Success, output);
						}
						else
						{
							return new Tuple<eResultOutputStatus, string>(eResultOutputStatus.Success, $"Chyba: {error}");
						}
					}
				}
			}
			catch (Exception ex)
			{
				var message = "Chyba při spuštění PowerShell skriptu.";
#if DEBUG
				message = $"Chyba při spuštění PowerShell skriptu:{ex.Message}";
#endif

				throw new Exception(message);
			}
		}

		public Tuple<eResultOutputStatus, string> RunWithResult(string script)
		{
			ProcessStartInfo processInfo = new ProcessStartInfo();
			processInfo.FileName = "powershell.exe";
			processInfo.Arguments = $"-WindowStyle Hidden -Command \"{script}\"";
			processInfo.Verb = "runas";
			processInfo.RedirectStandardOutput = true;
			processInfo.RedirectStandardError = true;
			processInfo.UseShellExecute = false;
			processInfo.CreateNoWindow = true;

			try
			{
				using (Process process = Process.Start(processInfo))
				{
					using (StreamReader outputReader = process.StandardOutput)
					using (StreamReader errorReader = process.StandardError)
					{
						string output = outputReader.ReadToEnd();
						string error = errorReader.ReadToEnd();

						process.WaitForExit();

						if (process.ExitCode == 0)
						{
							return new Tuple<eResultOutputStatus, string>(eResultOutputStatus.Success, output);
						}
						else
						{
							return new Tuple<eResultOutputStatus, string>(eResultOutputStatus.Success, $"Chyba: {error}");
						}
					}
				}
			}
			catch (Exception ex)
			{
				var message = "Chyba při spuštění PowerShell skriptu.";
#if DEBUG
				message = $"Chyba při spuštění PowerShell skriptu:{ex.Message}";
#endif

				throw new Exception(message);
			}
		}
	}
}