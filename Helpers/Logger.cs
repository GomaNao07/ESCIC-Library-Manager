using System;
using System.IO;

namespace ESCIC_Library_Manager.Helpers
{
    public enum LogLevel { INFO, WARNING, ERROR, SUCCESS }

    public static class Logger
    {
        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "escic_log.log");

        public static void Log(string message, LogLevel level = LogLevel.INFO)
        {
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            Console.WriteLine(entry);
            try { File.AppendAllText(LogFile, entry + Environment.NewLine); } catch { }
        }

        public static void LogInfo(string m) => Log(m, LogLevel.INFO);
        public static void LogSuccess(string m) => Log(m, LogLevel.SUCCESS);
        public static void LogError(string m, Exception ex = null) => Log($" {ex?.Message}", LogLevel.ERROR);
    }
}
