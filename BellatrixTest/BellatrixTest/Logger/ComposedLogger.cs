using System;
using System.Collections.Generic;

namespace BellatrixTest.Logger
{
    public class ComposedLogger : ILogger
    {
        public List<ILogger> Loggers { get; private set; }

        public ComposedLogger()
        {
            this.Loggers = new List<ILogger>();
        }
        public ComposedLogger(params ILogger[] loggers)
            : this()
        {
            this.Loggers.AddRange(loggers);
        }
        public ComposedLogger(IEnumerable<ILogger> loggers)
            : this()
        {
            this.Loggers.AddRange(loggers);
        }

        public void LogMessage(string message, LogMessageType messageType)
        {
            this.Loggers.ForEach(l => l.LogMessage(message, messageType));
        }
    }
}
