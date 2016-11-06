using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BellatrixTest.Logger
{
    public class DatabaseLogger : AbstractLogger
    {
        private string connectionString;

        public DatabaseLogger(string connectionString)
            : base()
        {
            this.connectionString = connectionString;
        }

        public DatabaseLogger(string connectionString, params LogMessageType[] messageTypes)
            : base(messageTypes)
        {
            this.connectionString = connectionString;
        }

        protected override void WriteToLog(string message, LogMessageType messageType)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                var type = (int)messageType;

                command.CommandText = "INSERT INTO Log(Message, Type, Timestamp) VALUES(@message, @type, @ts)";
                command.Parameters.AddWithValue("message", message);
                command.Parameters.AddWithValue("type", type);
                command.Parameters.AddWithValue("ts", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
    }
}
