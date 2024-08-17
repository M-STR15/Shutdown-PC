﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace ShutdownPC.Services
{
    public class PcActionService
    {
        private const ushort EWX_FORCE = 0x00000004;

        private const ushort EWX_LOGOFF = 0x00000000;

        private const ushort EWX_POWEROFF = 0x00000008;

        private const ushort EWX_REBOOT = 0x00000002;

        private const ushort EWX_RESTARTAPPS = 0x00000040;

        private const ushort EWX_SHUTDOWN = 0x00000001;

        private const short SE_PRIVILEGE_ENABLED = 0x00000002;

        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        private const short TOKEN_ADJUST_PRIVILEGES = 0x0020;

        private const short TOKEN_QUERY = 0x0008;

        public void LogOff()
        {
            LogOff(false);
        }

        public static void LogOff(bool force)
        {
            getPrivileges();
            bool success = ExitWindowsEx(EWX_LOGOFF |
              (uint)(force ? EWX_FORCE : 0), 0) != 0;
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show($"Chyba při odhlášení: {error}");
            }
        }

        public static void Reboot(bool force = false)
        {
            getPrivileges();
            bool success = ExitWindowsEx(EWX_REBOOT |
              (uint)(force ? EWX_FORCE : 0), 0) != 0;

            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show($"Chyba při restartování: {error}");
            }
        }

        /// <summary>
        ///  Volání funkce pro vypnutí (false znamená normální vypnutí, true by vynutilo vypnutí)
        /// </summary>
        /// <param name="force"></param>
        public static void Shutdown(bool force = false)
        {
            getPrivileges();
            bool success = ExitWindowsEx(EWX_SHUTDOWN | EWX_POWEROFF | (uint)(force ? EWX_FORCE : 0), 0) != 0;

            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show($"Chyba při vypnutí: {error}");
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, out IntPtr TokenHandle);

        private static void getPrivileges()
        {
            IntPtr hToken;
            TOKEN_PRIVILEGES tkp;

            OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken);
            tkp.PrivilegeCount = 1;
            tkp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;
            LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out tkp.Privileges.Luid);
            AdjustTokenPrivileges(hToken, false, ref tkp, 0U, IntPtr.Zero, IntPtr.Zero);
        }

        private struct LUID
        {
            public int LowPart;
            public int HighPart;
        }

        private struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public int Attributes;
        }

        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }
    }
}