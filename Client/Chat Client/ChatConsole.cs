using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            Console.WriteLine("Welcome!");
            Console.WriteLine("What's your name?");

            while (name == "")
            {
                name = Console.ReadLine();
            }

            Console.WriteLine("Who do you want to chat with?");
            
            while (chatpalName == "")
            {
                chatpalName = Console.ReadLine();
            }

            var doc = new XmlDocument();
            doc.LoadXml($"<user name=\"{name}\"></user>");

            var data = doc.OuterXml;

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

                        // Send data about current user to the server
                        if (!_client.SendMessage(data))
                        {
                            Log("Something happened when sending login info to the server");
                        } else
                        {

                        }
                    }
                    catch (SocketException e)
                    {
                        Log("Cannot connect to the server. Try again.");
                    }
                }
            }

            //Console.WriteLine("Enter a username of a person you want to chat with");
            //var username = Console.ReadLine();

           // _client.RequestToChat(username);
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }


}
