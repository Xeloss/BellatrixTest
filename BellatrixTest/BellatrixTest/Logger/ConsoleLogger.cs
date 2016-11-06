﻿using System;

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

            var defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = GetForegroundColorFor(messageType);

            Console.WriteLine(DateTime.Now.ToShortDateString() + ": " + message);

            Console.ForegroundColor = defaultColor;
        }

        private ConsoleColor GetForegroundColorFor(LogMessageType type)
        {
            switch (type)
            {
                case LogMessageType.Error: return ConsoleColor.Red;
                case LogMessageType.Warning: return ConsoleColor.Yellow;
                case LogMessageType.Message: 
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
