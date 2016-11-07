using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BellatrixTest.Logger.Builder;
using BellatrixTest.Logger;
using System.Linq;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using BelatrixTest.Tests.Utils;

namespace BelatrixTest.Tests
{
    [TestClass]
    [DeploymentItem(@"LocalDB\BelatrixLog.mdf")]
    [DeploymentItem(@"LocalDB\BelatrixLog_log.ldf")]
    public class DatabaseLoggerTest
    {
        private static DBHelper dbHelper;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(context.DeploymentDirectory, string.Empty));
            var ConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;

            dbHelper = new DBHelper(ConnectionString);
        }        
        
        [TestCleanup]
        public void TestCleanUp()
        {
            dbHelper.CleanLogTable();
        }

        [TestMethod]
        public void DefaultConstructorShouldLogAllKindOfMessages()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 3);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogErrorShouldOnlyLogErrors()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 1);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningShouldOnlyLogWarning()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 1);
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesShouldOnlyLogMessages()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString, LogMessageType.Message);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 1);
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndWarningsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString, LogMessageType.Message, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 2);
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString, LogMessageType.Message, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 2);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningsAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new DatabaseLogger(dbHelper.ConnectionString, LogMessageType.Warning, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = dbHelper.GetLogContent();

            Assert.AreEqual(logRecords.Count(), 2);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Message));
        }
    }
}
