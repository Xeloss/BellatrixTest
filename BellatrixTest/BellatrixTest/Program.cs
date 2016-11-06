using BellatrixTest.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new ConsoleLogger(LogMessageType.Message, LogMessageType.Warning, LogMessageType.Error);
            log.LogMessage("Logger text", LogMessageType.Error);
            Console.WriteLine("Application text");
            log.LogMessage("Some message", LogMessageType.Warning);
            Console.WriteLine("Application text");
            log.LogMessage("Some message", LogMessageType.Message);
            Console.WriteLine("Application text");

            Console.ReadKey();
        }
    }
}
