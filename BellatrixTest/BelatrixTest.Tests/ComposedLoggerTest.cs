using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BellatrixTest.Logger.Builder;
using BellatrixTest.Logger;
using System.Linq;
using System.IO;
using System.Text;
using System.Configuration;
using BelatrixTest.Tests.Utils;

namespace BelatrixTest.Tests
{
    [TestClass]
    [DeploymentItem(@"LocalDB\BelatrixLog.mdf")]
    [DeploymentItem(@"LocalDB\BelatrixLog_log.ldf")]
    public class ComposedLoggerTest
    {
        private static LogFileHelper fileHelper;
        private static DBHelper dbHelper;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(context.DeploymentDirectory, string.Empty));
            var connectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;

            dbHelper = new DBHelper(connectionString);
            fileHelper = new LogFileHelper(context.TestRunDirectory);
        }

        [TestInitialize]
        public void TestSetUp()
        {
            ConsoleHelper.SetUpTestConsole();
        }
        
        [TestCleanup]
        public void TestCleanUp()
        {
            ConsoleHelper.RestoreDefaultConsole();
            dbHelper.CleanLogTable();

            fileHelper.DeleteLogFile();
        }

        [TestMethod]
        public void WithConsoleShouldOnlyLogToTheConsole()
        {
            var console = new ConsoleLogger();
            var logger = new ComposedLogger(console);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsTrue(consoleContent.Any());
            Assert.IsFalse(dbRecords.Any());
            Assert.IsFalse(fileContent.Any());
        }

        [TestMethod]
        public void WithDatabaseShouldOnlyLogToTheDatabase()
        {
            var db = new DatabaseLogger(dbHelper.ConnectionString);
            var logger = new ComposedLogger(db);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsFalse(consoleContent.Any());
            Assert.IsTrue(dbRecords.Any());
            Assert.IsFalse(fileContent.Any());
        }

        [TestMethod]
        public void WithFileShouldOnlyLogToAFile()
        {
            var file = new FileLogger(fileHelper.FileDirectory);
            var logger = new ComposedLogger(file);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsFalse(consoleContent.Any());
            Assert.IsFalse(dbRecords.Any());
            Assert.IsTrue(fileContent.Any());
        }

        [TestMethod]
        public void WithFileAndConsoleShouldOnlyLogToBothOutputs()
        {
            var console = new ConsoleLogger();
            var file = new FileLogger(fileHelper.FileDirectory);
            var logger = new ComposedLogger(console, file);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsTrue(consoleContent.Any());
            Assert.IsFalse(dbRecords.Any());
            Assert.IsTrue(fileContent.Any());
        }

        [TestMethod]
        public void WithFileAndDatabaseShouldOnlyLogToBothOutputs()
        {
            var db = new DatabaseLogger(dbHelper.ConnectionString);
            var file = new FileLogger(fileHelper.FileDirectory);
            var logger = new ComposedLogger(db, file);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsFalse(consoleContent.Any());
            Assert.IsTrue(dbRecords.Any());
            Assert.IsTrue(fileContent.Any());
        }

        [TestMethod]
        public void WithConsoleAndDatabaseShouldOnlyLogToBothOutputs()
        {
            var db = new DatabaseLogger(dbHelper.ConnectionString);
            var console = new ConsoleLogger();
            var logger = new ComposedLogger(db, console);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsTrue(consoleContent.Any());
            Assert.IsTrue(dbRecords.Any());
            Assert.IsFalse(fileContent.Any());
        }

        [TestMethod]
        public void WithAllLoggersShouldLogToAllOutputs()
        {
            var file = new FileLogger(fileHelper.FileDirectory);
            var db = new DatabaseLogger(dbHelper.ConnectionString);
            var console = new ConsoleLogger();
            var logger = new ComposedLogger(file, db, console);

            logger.LogMessage("This is a test message", LogMessageType.Message);

            var consoleContent = ConsoleHelper.ReadOutput();
            var dbRecords = dbHelper.GetLogContent();
            var fileContent = fileHelper.GetLogFileContent();

            Assert.IsTrue(consoleContent.Any());
            Assert.IsTrue(dbRecords.Any());
            Assert.IsTrue(fileContent.Any());
        }
    }
}
