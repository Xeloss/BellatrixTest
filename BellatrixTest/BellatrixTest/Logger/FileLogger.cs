using System;
using System.Configuration;
using System.IO;

namespace BellatrixTest.Logger
{
    public class FileLogger : AbstractLogger
    {
        public FileLogger(params LogMessageType[] messageTypes)
            : base(messageTypes)
        { }

        protected override void WriteToLog(string message, LogMessageType messageType)
        {
            string logFileContent = "";

            if (!File.Exists(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                logFileContent = File.ReadAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
            }
            if (messageType == LogMessageType.Error)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message;
            }
            if (messageType == LogMessageType.Warning)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message;
            }
            if (messageType == LogMessageType.Message)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message;
            }

            File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", logFileContent);
        }
    }
}
