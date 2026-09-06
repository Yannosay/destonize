using System;
using System.Linq;
using System.Management;

namespace Destonize.Services
{
    public static class SystemInfoService
    {
        public static SystemInfo GetSystemInfo()
        {
            var osSearcher = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem");
            var cpuSearcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            var memSearcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");

            var os = osSearcher.Get().Cast<ManagementObject>().FirstOrDefault();
            var cpu = cpuSearcher.Get().Cast<ManagementObject>().FirstOrDefault();
            var mem = memSearcher.Get().Cast<ManagementObject>().FirstOrDefault();

            return new SystemInfo
            {
                ComputerName = Environment.MachineName,
                OS = $"{os?["Caption"]} {os?["Version"]}",
                CPU = cpu?["Name"]?.ToString() ?? "Unknown",
                Memory = $"{Convert.ToInt64(mem?["TotalPhysicalMemory"]) / (1024 * 1024 * 1024)} GB",
                PowerShell = "N/A"
            };
        }
    }

    public class SystemInfo
    {
        public string ComputerName { get; set; } = string.Empty;
        public string OS { get; set; } = string.Empty;
        public string CPU { get; set; } = string.Empty;
        public string Memory { get; set; } = string.Empty;
        public string PowerShell { get; set; } = string.Empty;
    }
}
