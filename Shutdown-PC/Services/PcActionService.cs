using System.Runtime.InteropServices;

namespace ShutdownPC.Services
{
	public class PcActionService
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool InitiateSystemShutdownEx(
			string lpMachineName, string lpMessage, uint dwTimeout,
			bool bForceAppsClosed, bool bRebootAfterShutdown, uint dwReason);

		// Flags for ExitWindowsEx
		private const uint EWX_LOGOFF = 0x00000000;

		private const uint EWX_SHUTDOWN = 0x00000001;
		private const uint EWX_REBOOT = 0x00000002;
		private const uint EWX_FORCE = 0x00000004;
		private const uint EWX_FORCEIFHUNG = 0x00000010;

		// Function to log off the user
		public static void LogOff()
		{
			if (!ExitWindowsEx(EWX_LOGOFF, 0))
				ThrowLastWin32Error();
		}

		// Function to shutdown the computer
		public static void Shutdown()
		{
			if (!ExitWindowsEx(EWX_SHUTDOWN | EWX_FORCE, 0))
				ThrowLastWin32Error();
		}

		// Function to restart the computer
		public static void Restart()
		{
			if (!ExitWindowsEx(EWX_REBOOT | EWX_FORCE, 0))
				ThrowLastWin32Error();
		}

		// Function to shutdown with custom parameters
		public static void ShutdownEx(string message, uint timeout, bool forceAppsClosed, bool rebootAfterShutdown, uint reason)
		{
			if (!InitiateSystemShutdownEx(null, message, timeout, forceAppsClosed, rebootAfterShutdown, reason))
				ThrowLastWin32Error();
		}

		private static void ThrowLastWin32Error()
		{
			int errorCode = Marshal.GetLastWin32Error();
			throw new System.ComponentModel.Win32Exception(errorCode);
		}
	}
}