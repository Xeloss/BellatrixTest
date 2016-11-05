using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixTest.Settings
{
    public interface IConfigurationProvider
    {
        string GetConnectionString();

        string GetLogFilePath();
    }
}
