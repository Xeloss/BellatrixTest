using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BellatrixTest.Logger.Builder;
using BellatrixTest.Logger;
using System.Linq;
using System.IO;
using System.Text;

namespace BelatrixTest.Tests
{
    [TestClass]
    public class FileLoggerTest
    {
        private static string TestDirectory;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            TestDirectory = context.TestRunDirectory;
        }        
        
        [TestCleanup]
        public void TestCleanUp()
        {
            File.Delete(GetLogFilePath());
        }

        [TestMethod]
        public void DefaultConstructorShouldLogAllKindOfMessages()
        {
            var logger = new FileLogger(TestDirectory);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogErrorShouldOnlyLogErrors()
        {
            var logger = new FileLogger(TestDirectory, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsFalse(logFile.Contains("Warning Message"));
            Assert.IsFalse(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningShouldOnlyLogWarning()
        {
            var logger = new FileLogger(TestDirectory, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsFalse(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsFalse(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesShouldOnlyLogMessages()
        {
            var logger = new FileLogger(TestDirectory, LogMessageType.Message);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsFalse(logFile.Contains("Error Message"));
            Assert.IsFalse(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndWarningsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new FileLogger(TestDirectory, LogMessageType.Message, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsFalse(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new FileLogger(TestDirectory, LogMessageType.Message, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsFalse(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningsAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new FileLogger(TestDirectory, LogMessageType.Warning, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsFalse(logFile.Contains("Info Message"));
        }

        private string GetLogFilePath()
        {
            return Directory.GetFiles(TestDirectory)
                            .FirstOrDefault(name => name.Contains("LogFile"));
        }

        private string GetLogFileContent()
        {
            return File.ReadAllText(GetLogFilePath());
        }
    }
}
