using ShutdownPC.Services.Models.PowerShell;

namespace ShutdownPC.Services
{
	public interface IPowerShellService
	{
		void Run(string script);
		void RunForPath(string scriptPath);
		Tuple<eResultOutputStatus, string> RunFromPathWithResult(string scriptPath);
		Tuple<eResultOutputStatus, string> RunWithResult(string script);
	}
}