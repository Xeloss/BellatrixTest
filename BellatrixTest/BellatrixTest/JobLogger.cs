namespace BellatrixTest{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class JobLogger
    {
        private bool _logToFile;
        private bool _logToConsole;
        private bool LogToDatabase;

        private bool _logMessage;
        private bool _logWarning;
        private bool _logError;

        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            LogToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
        }

        public void LogMessage(string message_s, bool message, bool warning, bool error)
        {
            message_s.Trim();
            if (message_s == null || message_s.Length == 0)
            {
                return;
            }
                    
            if (!_logToConsole && !_logToFile && !LogToDatabase)
            {
                throw new Exception("Invalid configuration");
            }

            if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && !error))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            #region Database Loggin
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();

            int t = 0;
            if (message && _logMessage)
            {
                t = 1;
            }
            if (error && _logError)
            {
                t = 2;
            }
            if (warning && _logWarning)
            {
                t = 3;
            }

            SqlCommand command = new SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")");
            command.ExecuteNonQuery();
            #endregion

            #region File Logging
            string logFileContent = "";

            if (!File.Exists(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                logFileContent = File.ReadAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
            }
            if (error && _logError)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message_s;
            }
            if (warning && _logWarning)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message_s;
            }
            if (message && _logMessage)
            {
                logFileContent = logFileContent + DateTime.Now.ToShortDateString() + message_s;
            }

            File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", logFileContent);
            #endregion

            #region Console Logging
            if (error && _logError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (warning && _logWarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (message && _logMessage)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
            #endregion
        }
    }}
