using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public delegate void LogMessage(string message);

namespace chat_server
{
    class Logger
    {
        public static Logger GetInstance { get; } = new Logger();
        private event LogMessage LogMessageEvent;

        private Logger()
        {
            //LogConsole();
        }

        public void Log(string message)
        {
            LogMessageEvent(message);
        }
    }
}
