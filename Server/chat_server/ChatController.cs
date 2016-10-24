using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

/* TODO:
 * - Refactor singleton
 * - Extract data to model
 * - Exceptions
 */

namespace chat_server
{
    public struct User
    {
        public string Name { get; set; }
        public TcpClient Connection { get; set; }

        public User(string name, TcpClient connection)
        {
            Name = name;
            Connection = connection;
        }
    }

    class ChatController
    {
        private static bool _hasInstance;
        public static ChatController Instance { get; set; }

        public Listener listener;
        private List<User> _clients = new List<User>();

        // Thread signal.
        public static ManualResetEvent tcpClientConnected =  new ManualResetEvent(false);

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

        public void AcceptClient(IAsyncResult result)
        {
            // Get the listener that handles the client request.
            Listener listener = (Listener)result.AsyncState;

            var client = listener.EndAcceptTcpClient(result);

            // Retrieve user information
            var data = new XmlDocument();
            data.LoadXml(GetUserData(client));

            User user = new User(data.DocumentElement.GetAttribute("name"), client);
            _clients.Add(user);

            // Make the server listen for the client messages
            new Thread(new ThreadStart(() => ListenForMessages(user))).Start();

            // Listen for more clients
            Listen();
        }

        public void Listen()
        {

            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), listener);

            tcpClientConnected.WaitOne();
        }

        public void ListenForMessages(User user)
        {
            while (true)
            {
                if (user.Connection.Connected)
                {
                    GetUserData(user.Connection);
                }
            }
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
            foreach (User client in _clients)
            {
                SendMessage(client: client.Connection, message: message);
            }
        }

        private static void ActiveConsole()
        {
            while (true)
            {
                Console.Write("\rListening...");
            }
        }

        private string GetUserData(TcpClient client)
        {

            // Using a list as we do not not the length of incoming data
            List<char> data = new List<char>();
            char[] buffer = new char[1];
            int readBytes;

            using (StreamReader netStream = new StreamReader(client.GetStream()))
            {
                do { //determine if there is more data, here we read until the socket is closed

                    readBytes = netStream.Read(buffer, 0, buffer.Length);
                    data.Add(buffer[0]);
                } while (!(buffer[0] == '\0'));
            }

            // Remove terminating char from data
            data.RemoveAt(data.Count - 1);

            return new string(data.ToArray());
        }
    }
}