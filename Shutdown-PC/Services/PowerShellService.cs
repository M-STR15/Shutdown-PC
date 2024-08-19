using System.Diagnostics;
using System.IO;

namespace ShutdownPC.Services
{
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
				Console.WriteLine($"Chyba při spuštění PowerShell skriptu: {ex.Message}");
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
				Console.WriteLine($"Chyba při spuštění PowerShell skriptu: {ex.Message}");
			}
		}

		public string RunFromPathWithResult(string scriptPath)
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
							return output; // Vrať úspěšný výstup skriptu
						}
						else
						{
							return $"Chyba: {error}"; // Vrať chybovou hlášku
						}
					}
				}
			}
			catch (Exception ex)
			{
				return $"Chyba při spuštění PowerShell skriptu: {ex.Message}";
			}
		}

		public string RunWithResult(string script)
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
							return output; // Vrať úspěšný výstup skriptu
						}
						else
						{
							return $"Chyba: {error}"; // Vrať chybovou hlášku
						}
					}
				}
			}
			catch (Exception ex)
			{
				return $"Chyba při spuštění PowerShell skriptu: {ex.Message}";
			}
		}
	}
}