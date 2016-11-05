using System;
using System.Configuration;
using System.IO;

namespace BellatrixTest.Logger
{
    public class FileLogger : AbstractLogger
    {
        private string filePath;

        public FileLogger(string filePath, params LogMessageType[] messageTypes)
            : base(messageTypes)
        {
            this.filePath = filePath;
        }

        protected override void WriteToLog(string message, LogMessageType messageType)
        {
            string logFileContent = "";

            if (!File.Exists(filePath + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                logFileContent = File.ReadAllText(filePath + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
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

            File.WriteAllText(filePath + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", logFileContent);
        }
    }
}
