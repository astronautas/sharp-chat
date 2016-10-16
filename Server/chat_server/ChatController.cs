using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chat_server
{
    class ChatController
    {
        public TcpClient connection1 { get; set; }
        public TcpClient connection2 { get; set; }

        private static bool _hasInstance;
        public static ChatController Instance { get; set; }

        public Listener listener;
        private List<TcpClient> _clients = new List<TcpClient>();

        private ChatController()
        {
        }

        public static ChatController GetInstance()
        {
            if (_hasInstance)
            {
                return Instance;
            } else
            {
                Instance     = new ChatController();
                _hasInstance = true;

                return Instance;
            }
        }

        public void AcceptClient()
        {
            var client = listener.AcceptTcpClient();
            _clients.Add(client);

            string message = "[SERVER] New client has connected";
            SendMessageToAll(message);

            Logger.GetInstance.Log(message);
        }

        public void Listen()
        {

            while (true)
            {
                if (listener.Pending())
                {
                    new Thread(new ThreadStart(AcceptClient)).Start();
                }
            }


            //if (this.connection1 != null)
            //{
            //    this.connection1.ReceiveBufferSize = 200;
            //    var reader = new StreamReader(this.connection1.GetStream());
            //    char[] buffer = new char[this.connection1.ReceiveBufferSize];

            //    int bytesRead = reader.Read(buffer, 0, this.connection1.ReceiveBufferSize);
            //    Array.Resize<Char>(ref buffer, bytesRead);

            //    string dataReceived = new string(buffer);

            //    Console.WriteLine(dataReceived);
            //}
        }

        private void SendMessage(TcpClient client, string message)
        {
            client.ReceiveBufferSize = message.Length;

            var charsToSend = message.ToCharArray();
            var writer = new StreamWriter(client.GetStream());

            writer.Write(charsToSend, 0, charsToSend.Length);
            writer.Flush();
        }

        private void SendMessageToAll(string message)
        {
            foreach (var client in _clients)
            {
                SendMessage(client: client, message: message);
            }
        }

        private static void ActiveConsole()
        {
            while (true)
            {
                Console.Write("\rListening...");
            }
        }
    }
}