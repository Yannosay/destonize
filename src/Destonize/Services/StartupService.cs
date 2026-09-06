using Microsoft.Win32;
using System;

namespace Destonize.Services
{
    public static class StartupService
    {
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "Destonize";

        public static void SetStartup(bool enabled)
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true);
            if (key == null)
                throw new InvalidOperationException("Cannot open startup registry key.");

            if (enabled)
            {
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ??
                                 System.Reflection.Assembly.GetExecutingAssembly().Location;
                key.SetValue(AppName, $"\"{exePath}\"");
            }
            else
            {
                key.DeleteValue(AppName, false);
            }
        }

        public static bool IsStartupEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, false);
            return key?.GetValue(AppName) != null;
        }
    }
}