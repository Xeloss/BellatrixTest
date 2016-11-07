using BellatrixTest.Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelatrixTest.Tests.Utils
{
    public class LogFileHelper
    {
        public string FileDirectory { get; private set; }

        public LogFileHelper(string directory)
        {
            FileDirectory = directory;
        }

        public string GetLogFilePath()
        {
            return Directory.GetFiles(FileDirectory)
                            .FirstOrDefault(name => name.Contains("LogFile"));
        }

        public string GetLogFileContent()
        {
            var path = GetLogFilePath();
            if (path == null)
                return "";

            return File.ReadAllText(path);
        }

        public void DeleteLogFile()
        {
            var path = GetLogFilePath();
            if (path == null)
                return;

            File.Delete(GetLogFilePath());
        }
    }
}
