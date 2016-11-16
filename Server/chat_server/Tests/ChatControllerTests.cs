using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Tests
{
    public class ChatControllerTests : Test
    {
        private class MockLogger : ILog
        {
            public string LoggedMessage { get; private set; }

            public void Log(string message)
            {
                LoggedMessage = message;
            }
        }

        public static void Init()
        {
            TestLog();
        }

        private static void TestLog()
        {
            var chatController = ChatController.GetInstance();
            var mockLogger = new MockLogger();

            chatController.loggers.Add(mockLogger);

            // Initializes listener on specified IP/Port combination
            chatController.listener = new Listener(IPAddress.Parse("127.0.0.1"), 8888);
            chatController.listener.Start();
            chatController.listener.HasStartedAcceptingClient = false;
            chatController.Listen();

            // By this moment, the chat controller should have already emitted
            // a listening message
            Assert(mockLogger.LoggedMessage == $"Listening for clients...", "Test chat controller listening start message existence");
        }
    }
}
