using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Shutdown_PC.Services
{
    public class PcAction
    {
        [DllImport("PowrProf.dll", SetLastError = true)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);


        public void Shutdown()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/s /t 0",
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }

        public void Restart()
        {
            ExitWindowsEx(EWX_REBOOT | EWX_FORCE, 0);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        private const uint EWX_LOGOFF = 0x00000000;
        private const uint EWX_REBOOT = 0x00000002;
        private const uint EWX_FORCE = 0x00000004;

        public void LogTheUserOut()
        {
            // Odhlášení uživatele bez vynucení
            ExitWindowsEx(EWX_LOGOFF, 0);

            // Pokud chcete vynutit odhlášení, použijte následující řádek místo toho výše:
            // ExitWindowsEx(EWX_LOGOFF | EWX_FORCE, 0);
        }
        public void SleepMode()
        {
            SetSuspendState(false, true, true);
        }
    }
}
