using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace chat_server
{
    class Program
    {
        static ChatController chatController;

        static void Main(string[] args)
        {
            chatController = ChatController.GetInstance();

            // Initializes listener on specified IP/Port combination
            chatController.listener = new Listener(IPAddress.Parse("127.0.0.1"), 8888);
            chatController.listener.Start();
            chatController.listener.HasStartedAcceptingClient = false;
            chatController.Listen();
        }
    }
}
