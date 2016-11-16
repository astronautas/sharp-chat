using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Utils
{
    public class ConsoleLogger : ILog
    {
        private static readonly ConsoleLogger instance = new ConsoleLogger();

        private ConsoleLogger() { }

        public static ConsoleLogger Instance
        {
            get
            {
                return instance;
            }
        }


        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
