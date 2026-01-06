using System;
using System.IO;

namespace ESCICLibraryManager.Helpers
{
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR,
        SUCCESS
    }

    public class Logger
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string LogFile = Path.Combine(LogDirectory, $"escic_log_{DateTime.Now:yyyy-MM-dd}.log");

        static Logger()
        {
            // Créer le dossier Logs s'il n'existe pas
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        public static void Log(string message, LogLevel level = LogLevel.INFO)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logMessage = $"[{timestamp}] [{level}] {message}";

                // Écrire dans le fichier
                File.AppendAllText(LogFile, logMessage + Environment.NewLine);

                // Afficher dans la console avec couleur
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = GetColorForLevel(level);
                Console.WriteLine($"[LOG] {message}");
                Console.ForegroundColor = originalColor;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture du log : {ex.Message}");
            }
        }

        public static void LogInfo(string message)
        {
            Log(message, LogLevel.INFO);
        }

        public static void LogWarning(string message)
        {
            Log(message, LogLevel.WARNING);
        }

        public static void LogError(string message, Exception ex = null)
        {
            string fullMessage = ex != null ? $"{message} | Exception: {ex.Message}" : message;
            Log(fullMessage, LogLevel.ERROR);
        }

        public static void LogSuccess(string message)
        {
            Log(message, LogLevel.SUCCESS);
        }

        private static ConsoleColor GetColorForLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.INFO => ConsoleColor.Cyan,
                LogLevel.WARNING => ConsoleColor.Yellow,
                LogLevel.ERROR => ConsoleColor.Red,
                LogLevel.SUCCESS => ConsoleColor.Green,
                _ => ConsoleColor.White
            };
        }

        public static void AfficherLogsRecents(int nombreLignes = 20)
        {
            try
            {
                if (!File.Exists(LogFile))
                {
                    Console.WriteLine(" Aucun fichier de log disponible.");
                    return;
                }

                string[] lines = File.ReadAllLines(LogFile);
                int startIndex = Math.Max(0, lines.Length - nombreLignes);

                Console.WriteLine($"\n {nombreLignes} derniers logs :");
                Console.WriteLine(new string('-', 80));

                for (int i = startIndex; i < lines.Length; i++)
                {
                    Console.WriteLine(lines[i]);
                }

                Console.WriteLine(new string('-', 80));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture des logs : {ex.Message}");
            }
        }
    }
}