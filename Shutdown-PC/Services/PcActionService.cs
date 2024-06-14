using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Shutdown_PC.Services
{
    public class PcActionService
    {
        private const uint EWX_FORCE = 0x00000004;

        private const uint EWX_LOGOFF = 0x00000000;

        private const ushort EWX_POWEROFF = 0x00000008;
        private const uint EWX_REBOOT = 0x00000002;

        private const ushort EWX_RESTARTAPPS = 0x00000040;
        private const uint EWX_SHUTDOWN = 0x00000001;
        private const short SE_PRIVILEGE_ENABLED = 2;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        private const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;
        private const uint SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000;
        private const uint SHTDN_REASON_MINOR_UPGRADE = 0x00000003;

        const short TOKEN_ADJUST_PRIVILEGES = 32;

        const short TOKEN_QUERY = 8;

        public PcActionService(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public void LogOff() { LogOff(false); }

        public void LogOff(bool force)
        {
            getPrivileges();
            bool result = ExitWindowsEx(EWX_LOGOFF | (uint)(force ? EWX_FORCE : 0), 0);

            // Check if the function call was successful
            if (!result)
            {
                // If the function call failed, get the last error code
                int errorCode = Marshal.GetLastWin32Error();
                Message = "Failed to shut down the system. Error code: " + errorCode;
            }
            else
            {
                Message = "System shutdown initiated successfully.";
            }
        }

        public void Reboot() { Reboot(false); }

        public void Reboot(bool force)
        {
            getPrivileges();
            bool result = ExitWindowsEx(EWX_REBOOT | (uint)(force ? EWX_FORCE : 0), 0);

            // Check if the function call was successful
            if (!result)
            {
                // If the function call failed, get the last error code
                int errorCode = Marshal.GetLastWin32Error();
                Message = "Failed to shut down the system. Error code: " + errorCode;
            }
            else
            {
                Message = "System shutdown initiated successfully.";
            }
        }

        public void Shutdown() { Shutdown(false); }

        public void Shutdown(bool force)
        {
            getPrivileges();
            bool result = ExitWindowsEx(EWX_SHUTDOWN | (uint)(force ? EWX_FORCE : 0) | EWX_POWEROFF, 0);
            // Check if the function call was successful
            if (!result)
            {
                // If the function call failed, get the last error code
                int errorCode = Marshal.GetLastWin32Error();
                Message = "Failed to shut down the system. Error code: " + errorCode;
            }
            else
            {
                Message = "System shutdown initiated successfully.";
            }
        }

        public void SleepMode()
        {
            SetSuspendState(false, true, true);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
          [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
          ref TOKEN_PRIVILEGES NewState,
          UInt32 BufferLength,
          IntPtr PreviousState,
          IntPtr ReturnLength);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("advapi32.dll")]
        static extern int LookupPrivilegeValue(string lpSystemName,
          string lpName, out LUID lpLuid);

        [DllImport("advapi32.dll")]
        static extern int OpenProcessToken(IntPtr ProcessHandle,
          int DesiredAccess, out IntPtr TokenHandle);

        [DllImport("PowrProf.dll", SetLastError = true)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

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
