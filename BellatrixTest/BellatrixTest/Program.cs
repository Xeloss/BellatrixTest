using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new JobLogger(true, true, true, false, false, true);

            logger.LogMessage("mensaje", true, true, false);
        }
    }
}
