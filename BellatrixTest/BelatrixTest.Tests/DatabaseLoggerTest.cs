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

namespace BelatrixTest.Tests
{
    [TestClass]
    [DeploymentItem(@"LocalDB\BelatrixLog.mdf")]
    [DeploymentItem(@"LocalDB\BelatrixLog_log.ldf")]
    public class DatabaseLoggerTest
    {
        private static string ConnectionString;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(context.DeploymentDirectory, string.Empty));
            ConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;
        }        
        
        [TestCleanup]
        public void TestCleanUp()
        {
            CleanLogTable();
        }

        [TestMethod]
        public void DefaultConstructorShouldLogAllKindOfMessages()
        {
            var logger = new DatabaseLogger(ConnectionString);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 3);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogErrorShouldOnlyLogErrors()
        {
            var logger = new DatabaseLogger(ConnectionString, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 1);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningShouldOnlyLogWarning()
        {
            var logger = new DatabaseLogger(ConnectionString, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 1);
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesShouldOnlyLogMessages()
        {
            var logger = new DatabaseLogger(ConnectionString, LogMessageType.Message);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 1);
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndWarningsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new DatabaseLogger(ConnectionString, LogMessageType.Message, LogMessageType.Warning);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 2);
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogMessagesAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new DatabaseLogger(ConnectionString, LogMessageType.Message, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 2);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        [TestMethod]
        public void WhenSpecifingToOnlyLogWarningsAndErrorsShouldOnlyLogThoseTypeOfMessages()
        {
            var logger = new DatabaseLogger(ConnectionString, LogMessageType.Warning, LogMessageType.Error);

            logger.LogMessage("Error Message", LogMessageType.Error);
            logger.LogMessage("Warning Message", LogMessageType.Warning);
            logger.LogMessage("Info Message", LogMessageType.Message);

            var logRecords = GetLogContent();

            Assert.AreEqual(logRecords.Count(), 2);
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Error));
            Assert.IsTrue(logRecords.Any(r => r.Type == LogMessageType.Warning));
            Assert.IsFalse(logRecords.Any(r => r.Type == LogMessageType.Message));
        }

        private IEnumerable<LogRecord> GetLogContent()
        {
            var result = new List<LogRecord>();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "SELECT Message, Type FROM Log";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var record = new LogRecord()
                    {
                        Message = reader.GetString(0),
                        Type = reader.GetFieldValue<LogMessageType>(1)
                    };

                    result.Add(record);
                }

                reader.Close();
            }

            return result;  
        }

        private void CleanLogTable()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "DELETE FROM Log";
                command.ExecuteNonQuery();
            }
        }

        private class LogRecord
        {
            public string Message { get; set; }

            public LogMessageType Type { get; set; }
        }
    }
}
