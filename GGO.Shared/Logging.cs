using System;
using System.IO;
using System.Reflection;

namespace GGO.Shared
{
    public class Logging
    {
        /// <summary>
        /// The same logging levels as Python.
        /// For the sake of "What I like".
        /// </summary>
        public enum Level
        {
            NoSet = 0,
            Debug = 10,
            Info = 20,
            Warning = 30,
            Error = 40,
            Critical = 50
        }

        /// <summary>
        /// Destination folder for the Log file.
        /// </summary>
        public static string Folder = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        /// <summary>
        /// Name for the output file.
        /// </summary>
        public static string Filename = Assembly.GetExecutingAssembly().GetName().Name + ".log";
        /// <summary>
        /// Our default logging level.
        /// </summary>
        public static Level CurrentLevel = Level.Info;

        /// <summary>
        /// Log a message to a file.
        /// If the logging level allows it, show it on-screen.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        /// <param name="LoggingLevel">The logging level.</param>
        public static void Log(string Message, Level LoggingLevel)
        {
            // Create the directory of the output file if it does not exist
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }

            // If the message level is higher than the current level, open a StreamWriter and append a line with the date, logging level and message
            if (LoggingLevel >= CurrentLevel)
            {
                // Create a beautiful message to log
                string LogMessage = string.Format("[{0}] [{1}] {2}", DateTime.Now.ToString(), LoggingLevel.ToString(), Message);

                StreamWriter OpenFile = File.AppendText(Path.Combine(Folder, Filename));
                OpenFile.WriteLine(LogMessage);
                OpenFile.Close();
            }
        }

        /// <summary>
        /// Log a Critical Message.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        public static void Critical(string Message)
        {
            Log(Message, Level.Critical);
        }

        /// <summary>
        /// Log an Error Message.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        public static void Error(string Message)
        {
            Log(Message, Level.Error);
        }

        /// <summary>
        /// Log a Warning Message.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        public static void Warning(string Message)
        {
            Log(Message, Level.Warning);
        }

        /// <summary>
        /// Log an Informational Message.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        public static void Info(string Message)
        {
            Log(Message, Level.Info);
        }

        /// <summary>
        /// Log a Debug Message.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        public static void Debug(string Message)
        {
            Log(Message, Level.Debug);
        }
    }
}
