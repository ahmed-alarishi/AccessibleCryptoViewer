using System;
using System.IO;

namespace AccessibleCryptoViewer
{
    public static class RuntimeLogger
    {
        private static readonly string LogPath = "runtime_log.txt";

        public static void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
            File.AppendAllText(LogPath, logEntry);
        }
    }
}
