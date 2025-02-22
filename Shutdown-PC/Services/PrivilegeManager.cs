﻿using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ShutdownPC.Services
{
	/// <summary>
	/// Služba nastavující privilegií v počitači, aby šel počítač vypnout a restartovat.
	/// </summary>
	public class PrivilegeManager
	{
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint Zero, IntPtr PreviousState, IntPtr ReturnLength);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetCurrentProcess();

		private const uint TOKEN_QUERY = 0x0008;
		private const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;

		private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

		[StructLayout(LayoutKind.Sequential)]
		private struct LUID
		{
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct TOKEN_PRIVILEGES
		{
			public uint PrivilegeCount;
			public LUID_AND_ATTRIBUTES Privileges;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct LUID_AND_ATTRIBUTES
		{
			public LUID Luid;
			public uint Attributes;
		}

		private const uint SE_PRIVILEGE_ENABLED = 0x00000002;

		/// <summary>
		/// Tato metoda povoluje privilegium vypnutí počítače.
		/// </summary>
		public static void EnableShutdownPrivilege()
		{
			IntPtr tokenHandle;
			if (!OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES, out tokenHandle))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			LUID luid;
			if (!LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out luid))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			TOKEN_PRIVILEGES tp;
			tp.PrivilegeCount = 1;
			tp.Privileges.Luid = luid;
			tp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;

			if (!AdjustTokenPrivileges(tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}
	}
}