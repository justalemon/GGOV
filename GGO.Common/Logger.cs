using System;
using System.IO;

namespace GGO.Common
{
    public class Logger
    {
        private static string TempDir = Path.Combine(Path.GetTempPath(), "GGO");
        private static string LogFileName = "GGOHUD.log";
        private FileInfo LogFile;

        public Logger()
        {
            // If our %TEMP%\GGO folder does not exist, create it.
            if (!Directory.Exists(TempDir))
            {
                Directory.CreateDirectory(TempDir);
            }
            
            LogFile = new FileInfo(Path.Combine(TempDir, LogFileName));
        }

        public void Log(string message)
        {
            // Handle stream write in seperate object for each write to ensure safely closing the file.
            StreamWriter Append = LogFile.AppendText();
            Append.WriteLine($"{DateTime.Now.ToString()} - {message}");
            Append.Close();
        }
    }
}
