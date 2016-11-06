﻿using System;
using System.Configuration;
using System.IO;

namespace BellatrixTest.Logger
{
    public class FileLogger : AbstractLogger
    {
        private string filePath;

        public FileLogger(string logDirectory, params LogMessageType[] messageTypes)
            : base(messageTypes)
        {
            this.filePath = string.Format(@"{0}\{1}_{2}.txt", logDirectory.TrimEnd('\\'), "LogFile", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected override void WriteToLog(string message, LogMessageType messageType)
        {
            using (var fileWriter = File.AppendText(filePath))
            {
                fileWriter.WriteLine(string.Format("{0}: {1}", DateTime.Now, message));
            }
        }
    }
}
