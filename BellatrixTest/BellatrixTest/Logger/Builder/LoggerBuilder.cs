using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixTest.Logger.Builder
{
    public class LoggerBuilder
    {
        private List<ILogger> loggers;

        public LoggerBuilder()
        {
            this.loggers = new List<ILogger>();
        }

        public LoggerBuilder WithDatabaseLogger(string connectionString)
        {
            this.loggers.Add(new DatabaseLogger(connectionString));
            return this;
        }
        public LoggerBuilder WithDatabaseLogger(string connectionString, params LogMessageType[] messageTypes)
        {
            this.loggers.Add(new DatabaseLogger(connectionString, messageTypes));
            return this;
        }

        public LoggerBuilder WithFileLogger(string logDirectory)
        {
            this.loggers.Add(new FileLogger(logDirectory));
            return this;
        }
        public LoggerBuilder WithFileLogger(string logDirectory, params LogMessageType[] messageTypes)
        {
            this.loggers.Add(new FileLogger(logDirectory, messageTypes));
            return this;
        }

        public LoggerBuilder WithConsoleLogger()
        {
            this.loggers.Add(new ConsoleLogger());
            return this;
        }
        public LoggerBuilder WithConsoleLogger(params LogMessageType[] messageTypes)
        {
            this.loggers.Add(new ConsoleLogger(messageTypes));
            return this;
        }

        public ILogger Build()
        {
            if (this.loggers.Count == 1)
                return this.loggers.First();

            return new ComposedLogger(this.loggers.ToArray());
        }
    }
}
