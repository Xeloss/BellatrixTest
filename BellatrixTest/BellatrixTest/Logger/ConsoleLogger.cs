using System;

namespace BellatrixTest.Logger
{
    public class ConsoleLogger : ILogger
    {
        private bool logMessages;
        private bool logWarnings;
        private bool logErrors;

        public ConsoleLogger(bool logMessages, bool logWarnings, bool logErrors)
        {
            this.logMessages = logMessages;
            this.logWarnings = logWarnings;
            this.logErrors = logErrors;
        }

        public void LogMessage(string message, LogMessageType messageType)
        {
            if (messageType == LogMessageType.Error && logErrors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (messageType == LogMessageType.Warning && logWarnings)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (messageType == LogMessageType.Message && logMessages)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }
    }
}
