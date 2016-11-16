using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chat_server.Utils
{
    class FileLogger : ILog
    {
        protected static String projectDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                 + $@"\{ConfigurationManager.AppSettings["project_name"]}\";

        public string Filename = "log.txt";

        private static readonly FileLogger instance = new FileLogger();

        private StreamWriter _logFileStream;

        private Queue<string> _logQueue = new Queue<string>();

        public static FileLogger Instance
        {
            get
            {
                return instance;
            }
        }

        private FileLogger()
        {
            InitStream();

            new Task(() => 
            {
                WriteLog();
            }).Start();
        }

        private void InitStream()
        {
            var path = projectDocPath + Filename;

            if (!Directory.Exists(projectDocPath))
            {
                Directory.CreateDirectory(projectDocPath);
            }
        }

        public void Log(string message)
        {
            string path = projectDocPath + Filename;

            new Thread(() =>
            {
                while (true)
                {
                    if (Monitor.TryEnter(_logQueue))
                    {
                        try
                        {
                            _logQueue.Enqueue(message);
                            break;
                        }
                        finally
                        {
                            Monitor.Exit(_logQueue);
                        }
                    }
                }
            }).Start();
        }

        private void WriteLog()
        {
            var path = projectDocPath + Filename;

            while (true)
            {
                if (_logQueue.Count() != 0)
                {
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(_logQueue.Dequeue());
                        }
                    }

                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(_logQueue.Dequeue());
                    }
                }
            }
        }

    }
}
