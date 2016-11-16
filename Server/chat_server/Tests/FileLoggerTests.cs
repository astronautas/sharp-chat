using chat_server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chat_server.Tests
{
    class FileLoggerTests : Test
    {
        public static void Init()
        {
            TestConcurrentLogging();
        }

        private static void TestConcurrentLogging()
        {
            var logger = FileLogger.Instance;

            var logNow = false;

            logger.Log("TESTING" + Environment.NewLine);

            for (int i = 0; i < 10; i++)
            {
                var currMsgNum = i;

                new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(10000);
                    logger.Log($"Message {currMsgNum}");
                })).Start();
            }

            // Check test log - should contain all messages from one to ten
            // Order should not be exact
        }
    }
}
