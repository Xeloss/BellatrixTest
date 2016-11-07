using BellatrixTest.Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelatrixTest.Tests.Utils
{
    public class DBHelper
    {
        public string ConnectionString { get; private set; }

        public DBHelper(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public IEnumerable<LogRecord> GetLogContent()
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

        public void CleanLogTable()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "DELETE FROM Log";
                command.ExecuteNonQuery();
            }
        }
    }
}
