using System.Diagnostics;
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

        public void LogOff(bool force)
        {
            getPrivileges();
            bool success = ExitWindowsEx(EWX_LOGOFF |
              (uint)(force ? EWX_FORCE : 0), 0) != 0;
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show($"Chyba vypnutí: {error}");
            }
        }

        public void Reboot()
        {
            Reboot(false);
        }

        public void Reboot(bool force)
        {
            getPrivileges();
            bool success = ExitWindowsEx(EWX_REBOOT |
              (uint)(force ? EWX_FORCE : 0), 0) != 0;

            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show($"Chyba vypnutí: {error}");
            }
        }

        public void Shutdown()
        {
            Shutdown(false);
        }

        public void Shutdown(bool force)
        {
            getPrivileges();
            bool success = ExitWindowsEx(EWX_SHUTDOWN |
           (uint)(force ? EWX_FORCE : 0) | EWX_POWEROFF, 0) != 0;

            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show($"Chyba vypnutí: {error}");
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
          [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
          ref TOKEN_PRIVILEGES NewState,
          UInt32 BufferLength,
          IntPtr PreviousState,
          IntPtr ReturnLength);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LookupPrivilegeValue(string lpSystemName,
          string lpName, out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int OpenProcessToken(IntPtr ProcessHandle,
          int DesiredAccess, out IntPtr TokenHandle);

        private void getPrivileges()
        {
            IntPtr hToken;
            TOKEN_PRIVILEGES tkp;

            OpenProcessToken(Process.GetCurrentProcess().Handle,
              TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken);
            tkp.PrivilegeCount = 1;
            tkp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;
            LookupPrivilegeValue("", SE_SHUTDOWN_NAME,
              out tkp.Privileges.pLuid);
            AdjustTokenPrivileges(hToken, false, ref tkp,
              0U, IntPtr.Zero, IntPtr.Zero);
        }

        private struct LUID
        {
            public int HighPart;
            public int LowPart;
        }

        private struct LUID_AND_ATTRIBUTES
        {
            public int Attributes;
            public LUID pLuid;
        }

        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }
    }
}