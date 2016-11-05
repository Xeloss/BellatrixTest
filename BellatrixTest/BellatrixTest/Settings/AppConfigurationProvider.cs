using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixTest.Settings
{
    public class AppConfigurationProvider : IConfigurationProvider
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["ConnectionString"];
        }

        public string GetLogFilePath()
        {
            return ConfigurationManager.AppSettings["LogFileDirectory"];
        }
    }
}
