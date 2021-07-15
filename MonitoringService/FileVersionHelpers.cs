using System.Diagnostics;

namespace MonitoringService
{
    public static class FileVersionHelpers
    {
        public static string FileProductVersion(this string filePath)
        {
            return FileVersionInfo.GetVersionInfo(filePath).ProductVersion;
        }
    }
}