using System;

namespace BellatrixTest.Logger
{
    public class ConsoleLogger : AbstractLogger
    {
        public ConsoleLogger(params LogMessageType[] messageTypes)
            : base(messageTypes)
        { }

        protected override void WriteToLog(string message, LogMessageType messageType)
        {
            if (!ShouldBeLogged(messageType))
                return;

            if (messageType == LogMessageType.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (messageType == LogMessageType.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (messageType == LogMessageType.Message)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }
    }
}
