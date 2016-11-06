using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BellatrixTest.Logger.Builder;
using BellatrixTest.Logger;
using System.Linq;

namespace BelatrixTest.Tests
{
    [TestClass]
    public class LoggerBuilderTest
    {
        [TestMethod]
        public void WhenBuildingOnlyADatabaseLoggerShouldNotReturnAComposedLogger()
        {
            var logger = new LoggerBuilder().WithDatabaseLogger("some useless connection string")
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(DatabaseLogger));
        }

        [TestMethod]
        public void WhenBuildingOnlyAConsoleLoggerShouldNotReturnAComposedLogger()
        {
            var logger = new LoggerBuilder().WithConsoleLogger()
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(ConsoleLogger));
        }

        [TestMethod]
        public void WhenBuildingOnlyAFileLoggerShouldNotReturnAComposedLogger()
        {
            var logger = new LoggerBuilder().WithFileLogger("Some useless path")
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(FileLogger));
        }

        [TestMethod]
        public void WhenBuildingWithFileLoggerAndConsoleLoggerShouldReturnAComposedLoggerWithBothLoggers()
        {
            var logger = new LoggerBuilder().WithConsoleLogger()
                                            .WithFileLogger("Some useless path")
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(ComposedLogger));
            var composed = logger as ComposedLogger;

            Assert.AreEqual(2, composed.Loggers.Count);
            Assert.IsTrue(composed.Loggers.Any(l => (l as ConsoleLogger) != null));
            Assert.IsTrue(composed.Loggers.Any(l => (l as FileLogger) != null));
        }

        [TestMethod]
        public void WhenBuildingWithFileLoggerAndDatabaseLoggerShouldReturnAComposedLoggerWithBothLoggers()
        {
            var logger = new LoggerBuilder().WithDatabaseLogger("some useless connection string")
                                            .WithFileLogger("Some useless path")
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(ComposedLogger));
            var composed = logger as ComposedLogger;

            Assert.AreEqual(2, composed.Loggers.Count);
            Assert.IsTrue(composed.Loggers.Any(l => (l as DatabaseLogger) != null));
            Assert.IsTrue(composed.Loggers.Any(l => (l as FileLogger) != null));
        }

        [TestMethod]
        public void WhenBuildingWithConsoleLoggerAndDatabaseLoggerShouldReturnAComposedLoggerWithBothLoggers()
        {
            var logger = new LoggerBuilder().WithDatabaseLogger("some useless connection string")
                                            .WithConsoleLogger()
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(ComposedLogger));
            var composed = logger as ComposedLogger;

            Assert.AreEqual(2, composed.Loggers.Count);
            Assert.IsTrue(composed.Loggers.Any(l => (l as DatabaseLogger) != null));
            Assert.IsTrue(composed.Loggers.Any(l => (l as ConsoleLogger) != null));
        }

        [TestMethod]
        public void WhenBuildingWithAllLoggersShouldReturnAComposedLoggerWithAllLoggers()
        {
            var logger = new LoggerBuilder().WithDatabaseLogger("some useless connection string")
                                            .WithFileLogger("some useless path")
                                            .WithConsoleLogger()
                                            .Build();

            Assert.IsInstanceOfType(logger, typeof(ComposedLogger));
            var composed = logger as ComposedLogger;

            Assert.AreEqual(3, composed.Loggers.Count);
            Assert.IsTrue(composed.Loggers.Any(l => (l as DatabaseLogger) != null));
            Assert.IsTrue(composed.Loggers.Any(l => (l as ConsoleLogger) != null));
            Assert.IsTrue(composed.Loggers.Any(l => (l as FileLogger) != null));
        }

        [TestMethod]
        public void WhenBuildingWithConsoleLoggerWithMessageTypeShouldIncludeOnlyThoseTypes()
        {
            var logger = new LoggerBuilder().WithConsoleLogger(LogMessageType.Message, LogMessageType.Warning)
                                            .Build() as ConsoleLogger;

            Assert.AreEqual(2, logger.MessageTypesToBeLogged.Count());
            Assert.IsTrue(logger.MessageTypesToBeLogged.Any(t => t == LogMessageType.Message));
            Assert.IsTrue(logger.MessageTypesToBeLogged.Any(t => t == LogMessageType.Warning));
        }

        [TestMethod]
        public void WhenBuildingWithFileLoggerWithMessageTypeShouldIncludeOnlyThoseTypes()
        {
            var logger = new LoggerBuilder().WithFileLogger("soome useless directory", LogMessageType.Error, LogMessageType.Warning)
                                            .Build() as FileLogger;

            Assert.AreEqual(2, logger.MessageTypesToBeLogged.Count());
            Assert.IsTrue(logger.MessageTypesToBeLogged.Any(t => t == LogMessageType.Error));
            Assert.IsTrue(logger.MessageTypesToBeLogged.Any(t => t == LogMessageType.Warning));
        }

        [TestMethod]
        public void WhenBuildingWithDatabaseLoggerWithMessageTypeShouldIncludeOnlyThoseTypes()
        {
            var logger = new LoggerBuilder().WithDatabaseLogger("soome useless directory", LogMessageType.Error, LogMessageType.Message)
                                            .Build() as DatabaseLogger;

            Assert.AreEqual(2, logger.MessageTypesToBeLogged.Count());
            Assert.IsTrue(logger.MessageTypesToBeLogged.Any(t => t == LogMessageType.Error));
            Assert.IsTrue(logger.MessageTypesToBeLogged.Any(t => t == LogMessageType.Message));
        }
    }
}
