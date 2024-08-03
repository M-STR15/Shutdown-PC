using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Shutdown_PC.Services
{
    public class PcActionService
    {
        private struct LUID
        {
            public int LowPart;
            public int HighPart;
        }

        private struct LUID_AND_ATTRIBUTES
        {
            public LUID pLuid;
            public int Attributes;
        }

        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }

        [DllImport("advapi32.dll")]
        private static extern int OpenProcessToken(IntPtr ProcessHandle,
          int DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
          [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
          ref TOKEN_PRIVILEGES NewState,
          UInt32 BufferLength,
          IntPtr PreviousState,
          IntPtr ReturnLength);

        [DllImport("advapi32.dll")]
        private static extern int LookupPrivilegeValue(string lpSystemName,
          string lpName, out LUID lpLuid);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        private const short SE_PRIVILEGE_ENABLED = 2;
        private const short TOKEN_ADJUST_PRIVILEGES = 32;
        private const short TOKEN_QUERY = 8;

        private const ushort EWX_LOGOFF = 0;
        private const ushort EWX_POWEROFF = 0x00000008;
        private const ushort EWX_REBOOT = 0x00000002;
        private const ushort EWX_RESTARTAPPS = 0x00000040;
        private const ushort EWX_SHUTDOWN = 0x00000001;
        private const ushort EWX_FORCE = 0x00000004;

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

        public void Shutdown()
        { Shutdown(false); }

        public void Shutdown(bool force)
        {
            getPrivileges();
            ExitWindowsEx(EWX_SHUTDOWN |
              (uint)(force ? EWX_FORCE : 0) | EWX_POWEROFF, 0);
        }

        public void Reboot()
        { Reboot(false); }

        public void Reboot(bool force)
        {
            getPrivileges();
            ExitWindowsEx(EWX_REBOOT |
              (uint)(force ? EWX_FORCE : 0), 0);
        }

        public void LogOff()
        { LogOff(false); }

        public void LogOff(bool force)
        {
            getPrivileges();
            ExitWindowsEx(EWX_LOGOFF |
              (uint)(force ? EWX_FORCE : 0), 0);
        }
    }
}