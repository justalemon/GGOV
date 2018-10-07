using System;
using System.IO;

namespace GGO.Common
{
    public class LogHelper
    {
        private static string filename = "log.txt";
        private static FileInfo logFile;
        private static LogLevel logLevel; 
        public LogHelper()
        {
            logFile = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory+"\\"+filename);
            logFile.Open(FileMode.Append);
            logLevel = LogLevel.Info;
        }

        public void Trace(string message)
        {
            if(logLevel <= LogLevel.Trace)
            {
                logFile.AppendText().WriteLine(string.Format("{0} - [TRACE] - {1}",DateTime.Now.ToString(),message))       ;
            }
        }
        public void Debug(string message)
        {
            if (logLevel <= LogLevel.Debug)
            {
                logFile.AppendText().WriteLine(string.Format("{0} - [DEBUG] - {1}", DateTime.Now.ToString(), message));
            }
        }
        public void Info(string message)
        {
            if (logLevel <= LogLevel.Info)
            {
                logFile.AppendText().WriteLine(string.Format("{0} - [INFO] - {1}", DateTime.Now.ToString(), message));
            }
        }
        public void Warn(string message)
        {
            if (logLevel <= LogLevel.Warn)
            {
                logFile.AppendText().WriteLine(string.Format("{0} - [WARN] - {1}", DateTime.Now.ToString(), message));
            }
        }
        public void Error(string message)
        {
            if (logLevel <= LogLevel.Error)
            {
                logFile.AppendText().WriteLine(string.Format("{0} - [ERROR] - {1}", DateTime.Now.ToString(), message));
            }
        }
        public void Critical(string message)
        {
            if (logLevel <= LogLevel.Critical)
            {
                logFile.AppendText().WriteLine(string.Format("{0} - [CRITICAL] - {1}", DateTime.Now.ToString(), message));
            }
        }

        public static LogLevel LogLevel
        {
            get
            {
                return logLevel;
            }
            set
            {
                logLevel = value;
            }
        }

    }


    public enum LogLevel
    {
        Trace = 0,
        Debug,
        Info,
        Warn,
        Error,
        Critical
    }
}
