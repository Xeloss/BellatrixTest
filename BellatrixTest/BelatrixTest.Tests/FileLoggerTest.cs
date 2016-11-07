using System;
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
    public class FileLoggerTest
    {
        private static LogFileHelper fileHelper;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            fileHelper = new LogFileHelper(context.TestRunDirectory);
        }        
        
        [TestCleanup]
        public void TestCleanUp()
        {
            fileHelper.DeleteLogFile();
        }

        [TestMethod]
        public void DefaultConstructorShouldLogAllKindOfMessages()
        {
            var logger = new FileLogger(fileHelper.FileDirectory);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogErrorShouldOnlyLogErrors()
        {
            var logger = new FileLogger(fileHelper.FileDirectory, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsFalse(logFile.Contains("Warning Message"));
            Assert.IsFalse(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningShouldOnlyLogWarning()
        {
            var logger = new FileLogger(fileHelper.FileDirectory, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsFalse(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsFalse(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesShouldOnlyLogMessages()
        {
            var logger = new FileLogger(fileHelper.FileDirectory, LogMessageType.Message);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsFalse(logFile.Contains("Error Message"));
            Assert.IsFalse(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndWarningsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new FileLogger(fileHelper.FileDirectory, LogMessageType.Message, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsFalse(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new FileLogger(fileHelper.FileDirectory, LogMessageType.Message, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsFalse(logFile.Contains("Warning Message"));
            Assert.IsTrue(logFile.Contains("Info Message"));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningsAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new FileLogger(fileHelper.FileDirectory, LogMessageType.Warning, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logFile = fileHelper.GetLogFileContent();

            Assert.IsTrue(logFile.Contains("Error Message"));
            Assert.IsTrue(logFile.Contains("Warning Message"));
            Assert.IsFalse(logFile.Contains("Info Message"));
        }
    }
}
