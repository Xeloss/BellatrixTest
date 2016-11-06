using System.Collections.Generic;

namespace BellatrixTest.Logger
{
    public class ComposedLogger : ILogger
    {
        private List<ILogger> loggers;

        public ComposedLogger()
        {
            this.loggers = new List<ILogger>();
        }
        public ComposedLogger(params ILogger[] loggers)
            : this()
        {
            this.loggers.AddRange(loggers);
        }
        public ComposedLogger(IEnumerable<ILogger> loggers)
            : this()
        {
            this.loggers.AddRange(loggers);
        }

        public void LogMessage(string message, LogMessageType messageType)
        {
            this.loggers.ForEach(l => l.LogMessage(message, messageType));
        }
    }
}
