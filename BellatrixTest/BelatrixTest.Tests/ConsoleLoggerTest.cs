﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BellatrixTest.Logger.Builder;
using BellatrixTest.Logger;
using System.Linq;
using System.IO;
using System.Text;
using BelatrixTest.Tests.Utils;

namespace BelatrixTest.Tests
{
    [TestClass]
    public class ConsoleLoggerTest
    {
        [TestInitialize]
        public void TestSetUp()
        {
            ConsoleHelper.SetUpTestConsole();
        }
        
        [TestCleanup]
        public void TestCleanUp()
        {
            ConsoleHelper.RestoreDefaultConsole();
        }

        [TestMethod]
        public void EmptyConstructorShouldLogAllKindOfMessages()
        {
            var logger = new ConsoleLogger();

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsTrue(consoleOut.Contains("Error Message"));
            Assert.IsTrue(consoleOut.Contains("Warning Message"));
            Assert.IsTrue(consoleOut.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogErrorShouldOnlyLogErrors()
        {
            var logger = new ConsoleLogger(LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsTrue(consoleOut.Contains("Error Message"));
            Assert.IsFalse(consoleOut.Contains("Warning Message"));
            Assert.IsFalse(consoleOut.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningShouldOnlyLogWarning()
        {
            var logger = new ConsoleLogger(LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsFalse(consoleOut.Contains("Error Message"));
            Assert.IsTrue(consoleOut.Contains("Warning Message"));
            Assert.IsFalse(consoleOut.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesShouldOnlyLogMessages()
        {
            var logger = new ConsoleLogger(LogMessageType.Message);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsFalse(consoleOut.Contains("Error Message"));
            Assert.IsFalse(consoleOut.Contains("Warning Message"));
            Assert.IsTrue(consoleOut.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndWarningsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new ConsoleLogger(LogMessageType.Message, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsFalse(consoleOut.Contains("Error Message"));
            Assert.IsTrue(consoleOut.Contains("Warning Message"));
            Assert.IsTrue(consoleOut.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new ConsoleLogger(LogMessageType.Message, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsTrue(consoleOut.Contains("Error Message"));
            Assert.IsFalse(consoleOut.Contains("Warning Message"));
            Assert.IsTrue(consoleOut.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningsAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new ConsoleLogger(LogMessageType.Warning, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var consoleOut = ConsoleHelper.ReadOutput();

            Assert.IsTrue(consoleOut.Contains("Error Message"));
            Assert.IsTrue(consoleOut.Contains("Warning Message"));
            Assert.IsFalse(consoleOut.Contains("Info Message"));
        }
    }
}
