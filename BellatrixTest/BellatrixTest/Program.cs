﻿using BellatrixTest.Logger;
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
            var log = new FileLogger(@"c:\", LogMessageType.Message, LogMessageType.Warning, LogMessageType.Error);

            log.LogMessage("Error text", LogMessageType.Error);
            log.LogMessage("Warning message", LogMessageType.Warning);
            log.LogMessage("Message message", LogMessageType.Message);

            Console.ReadKey();
        }
    }
}
