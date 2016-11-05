using System.Configuration;
using System.Data.SqlClient;

namespace BellatrixTest.Logger
{
    public class DatabaseLogger : ILogger
    {
        private bool logMessages;
        private bool logWarnings;
        private bool logErrors;

        public DatabaseLogger(bool logMessages, bool logWarnings, bool logErrors)
        {
            this.logMessages = logMessages;
            this.logWarnings = logWarnings;
            this.logErrors = logErrors;
        }

        public void LogMessage(string message, LogMessageType messageType)
        {
            #region Database Loggin
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();

            int t = 0;
            if (messageType == LogMessageType.Message && this.logMessages)
            {
                t = 1;
            }
            if (messageType == LogMessageType.Error && this.logErrors)
            {
                t = 2;
            }
            if (messageType == LogMessageType.Warning && this.logWarnings)
            {
                t = 3;
            }

            SqlCommand command = new SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")");
            command.ExecuteNonQuery();
            #endregion
        }
    }
}
