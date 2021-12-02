using System.Diagnostics;
using System.Reflection;

namespace MonitoringService
{
    public static class AssemblyVersionHelpers
    {
        public static string GetAssemblyVersion(
            this string filePath)
        {
            return FileVersionInfo.GetVersionInfo(filePath).ProductVersion!;
        }

        public static string GetAppVersion()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductVersion!;
        }
    }
}