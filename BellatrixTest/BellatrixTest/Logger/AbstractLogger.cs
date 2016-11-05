using System.Collections.Generic;
using System.Linq;

namespace BellatrixTest.Logger
{
    public abstract class AbstractLogger : ILogger
    {
        protected IEnumerable<LogMessageType> MessageTypesToBeLogged;

        public AbstractLogger(params LogMessageType[] messageTypes)
        {
            this.MessageTypesToBeLogged = messageTypes;
        }

        public void LogMessage(string message, LogMessageType messageType)
        {
            if (!ShouldBeLogged(messageType))
                return;

            WriteToLog(message, messageType);
        }

        protected abstract void WriteToLog(string message, LogMessageType messageType);

        protected virtual bool ShouldBeLogged(LogMessageType type)
        {
            return MessageTypesToBeLogged.Contains(type);
        }
    }
}
