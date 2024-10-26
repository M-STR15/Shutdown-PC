using System.Runtime.InteropServices;

namespace ShutdownPC.Services
{
	/// <summary>
	/// Služka vyvolává jednotlivé akce s počítačem. 
	/// Přes službu se dá: vypnout, restartovat a odhlásit PC.
	/// </summary>
	public class PcActionService
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool InitiateSystemShutdownEx(
			string lpMachineName, string lpMessage, uint dwTimeout,
			bool bForceAppsClosed, bool bRebootAfterShutdown, uint dwReason);

		private const uint EWX_LOGOFF = 0x00000000;

		private const uint EWX_SHUTDOWN = 0x00000001;
		private const uint EWX_REBOOT = 0x00000002;
		private const uint EWX_FORCE = 0x00000004;
		private const uint EWX_FORCEIFHUNG = 0x00000010;

		/// <summary>
		/// Odhlásí aktuálního uživatele.
		/// </summary>
		public static void LogOff()
		{
			if (!ExitWindowsEx(EWX_LOGOFF | EWX_FORCE, 0))
				throwLastWin32Error();
		}
		/// <summary>
		/// Vypne počítač.
		/// </summary>
		public static void Shutdown()
		{
			if (!ExitWindowsEx(EWX_SHUTDOWN | EWX_FORCE, 0))
				throwLastWin32Error();
		}
		/// <summary>
		/// Restartuje počítač.
		/// </summary>
		public static void Restart()
		{
			if (!ExitWindowsEx(EWX_REBOOT | EWX_FORCE, 0))
				throwLastWin32Error();
		}

		//public static void ShutdownEx(string message, uint timeout, bool forceAppsClosed, bool rebootAfterShutdown, uint reason)
		//{
		//	if (!InitiateSystemShutdownEx(null, message, timeout, forceAppsClosed, rebootAfterShutdown, reason))
		//		ThrowLastWin32Error();
		//}

		private static void throwLastWin32Error()
		{
			int errorCode = Marshal.GetLastWin32Error();
			throw new System.ComponentModel.Win32Exception(errorCode);
		}
	}
}