using BellatrixTest.Logger;
using BellatrixTest.Logger.Builder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["default"];
            var logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"];

            var logger = new LoggerBuilder().WithConsoleLogger()
                                            .WithFileLogger(logFileDirectory)
                                            .WithDatabaseLogger(connectionString.ConnectionString)
                                            .Build();

            logger.LogMessage("Error text", LogMessageType.Error);
            logger.LogMessage("Warning message", LogMessageType.Warning);
            logger.LogMessage("Message message", LogMessageType.Message);

            Console.ReadKey();
        }
    }
}
