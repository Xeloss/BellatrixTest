using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BellatrixTest.Logger
{
    public class DatabaseLogger : AbstractLogger
    {
        private string connectionString;

        public DatabaseLogger(string connectionString, params LogMessageType[] messageTypes)
            : base(messageTypes)
        {
            this.connectionString = connectionString;
        }

        protected override void WriteToLog(string message, LogMessageType messageType)
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            connection.Open();

            int t = 0;
            if (messageType == LogMessageType.Message)
            {
                t = 1;
            }
            if (messageType == LogMessageType.Error)
            {
                t = 2;
            }
            if (messageType == LogMessageType.Warning)
            {
                t = 3;
            }

            SqlCommand command = new SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")");
            command.ExecuteNonQuery();
        }
    }
}
