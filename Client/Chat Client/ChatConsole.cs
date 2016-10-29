using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Chat_Client
{
    class ChatConsole
    {
        ChatClient _client;

        public ChatConsole(ChatClient client)
        {
            _client = client;
            Launch();
        }

        private void Launch()
        {
            string name = "";
            string chatpalName = "";

            Console.WriteLine("Name?");
            name = Console.ReadLine();

            _client.NickName = name;

            Console.WriteLine("Chat pal name?");
            chatpalName = Console.ReadLine();

            Console.WriteLine("1. Connect");

            while (true)
            {
                var cmd = Console.ReadLine();

                if (cmd == "1")
                {
                    try
                    {
                        _client.Connect(IPAddress.Parse("127.0.0.1"), 8888);
                        Log("Succesfully connected to their server.");

                        // Authenticate
                        var status = _client.Authenticate();
                        Log("Authenticated! Sending contact request");

                        new Thread(new ThreadStart(() => { _client.ReadStream(new Action<string>(Log)); })).Start();

                        while (true)
                        {
                            Console.WriteLine($"Enter message to user {chatpalName}");
                            var msg = Console.ReadLine();
                            _client.SendMsgToUser(chatpalName, msg);
                        }

                    }
                    catch (SocketException e)
                    {
                        Log("Cannot connect to the server. Try again.");
                    }
                }
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }


}
