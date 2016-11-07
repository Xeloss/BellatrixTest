using BellatrixTest.Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelatrixTest.Tests.Utils
{
    public static class ConsoleHelper
    {
        private static TextWriter defaultConsoleOut;
        private static TextWriter consoleWriter;
        private static StringBuilder consoleOutput;

        public static void SetUpTestConsole()
        {
            defaultConsoleOut = Console.Out;
            consoleOutput = new StringBuilder();
            consoleWriter = new StringWriter(consoleOutput);
            Console.SetOut(consoleWriter);
        }

        public static void RestoreDefaultConsole()
        {
            consoleWriter.Flush();
            consoleOutput.Clear();
            Console.Clear();

            consoleWriter.Dispose();
            Console.SetOut(defaultConsoleOut);
        }

        public static string ReadOutput()
        {
            return consoleOutput.ToString();
        }
    }
}
