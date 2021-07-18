using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Editor
{
    public static class BrowserHelper
    {
        /// <summary>
        /// Opens an URL in the system default browser.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        public static void Open(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
                return;
            }

            throw new PlatformNotSupportedException("Cannot open link in browser.");
        }
    }
}
