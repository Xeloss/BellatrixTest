using System;
using System.Configuration;
using System.IO;

namespace BellatrixTest.Logger
{
    public class FileLogger : ILogger
    {
        private bool logMessages;
        private bool logWarnings;
        private bool logErrors;

        public FileLogger(bool logMessages, bool logWarnings, bool logErrors)
        {
            this.logMessages = logMessages;
            this.logWarnings = logWarnings;
            this.logErrors = logErrors;
        }

        public void LogMessage(string message, LogMessageType messageType)
        {
            string logFileContent = "";

            if (!File.Exists(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                logFileContent = File.ReadAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
            }
            if (messageType == LogMessageType.Error && this.logErrors)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message;
            }
            if (messageType == LogMessageType.Warning && this.logWarnings)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message;
            }
            if (messageType == LogMessageType.Message && this.logMessages)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message;
            }

            File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", logFileContent);
        }
    }
}
