using chat_server.Models;
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
    class ChatController
    {
        private static bool _hasInstance;
        public static ChatController Instance { get; set; }

        public Listener listener;
        private List<User> _clients = new List<User>();

        // Thread signal.
        public static ManualResetEvent tcpClientConnected =  new ManualResetEvent(false);

        private Dictionary<string, Action<User, string>> _router = new Dictionary<string, Action<User, string>>() {};

        private ChatController()
        {
            loadRoutes();
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

        private void loadRoutes()
        {
            _router.Add("sendMsgToUser", new Action<User, string>((user, parameters) => 
                        SendMsgToUser(user, parameters)));
            _router.Add("authenticate", new Action<User, string>((user, parameters) => 
                        AuthenticateUser(user, parameters)));
        }

        public void AcceptClient(IAsyncResult result)
        {
            // Get the listener that handles the client request.
            Listener listener = (Listener)result.AsyncState;

            var client = listener.EndAcceptTcpClient(result);

            User user = new User(client);
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
            StreamReader netStream = new StreamReader(user.Connection.GetStream());

            while (true)
            {
                string receivedData;

                // TODO - ping user to check if it is connected
                receivedData = GetUserStreamData(netStream);

                if (receivedData.Count() > 0)
                {
                    // Retrieve user information
                    var data = new XmlDocument();
                    data.LoadXml(receivedData);

                    var cmd   = data.DocumentElement.GetAttribute("command");
                    var parameters = data.DocumentElement.InnerXml;

                    _router[cmd](user, parameters);

                    // TODO - what if user sends fake commands
                }
            }
        }

        private static void ActiveConsole()
        {
            while (true)
            {
                Console.Write("\rListening...");
            }
        }

        private string GetUserStreamData(StreamReader netStream)
        {
            // Using a list as we do not not the length of incoming data
            List<char> data = new List<char>(0);
            char[] buffer = new char[1];
            int readBytes;

            readBytes = netStream.Read(buffer, 0, buffer.Length);

            // Read everything from start to end delimeter
            if (buffer[0] == '\u0002' && readBytes != -1)
            {
                do
                {

                    readBytes = netStream.Read(buffer, 0, buffer.Length);
                    data.Add(buffer[0]);
                } while (!(buffer[0] == '\0'));
            }
            else
            {
                return new string(data.ToArray());
            }

            // Remove terminating char from data
            data.RemoveAt(data.Count - 1);

            return new string(data.ToArray());
        }

        private void WriteToUserStream(User user, string text)
        {
            // Add delimiters
            text = '\u0002' + text + '\0';

            var charsToSend = text.ToCharArray();
            var writer = new StreamWriter(user.Connection.GetStream());

            writer.Write(charsToSend, 0, charsToSend.Length);
            writer.Flush();
        }

        // Route callbacks - data = xml element
        private void AuthenticateUser(User user, string parameters)
        {
            var data = new XmlDocument();
            data.LoadXml(parameters);
            var username = data.DocumentElement.GetAttribute("username" );

            user.Name = username;
        }

        private void SendMsgToUser(User user, string parameters)
        {
            var data = new XmlDocument();
            data.LoadXml(parameters);

            var username = data.DocumentElement.GetAttribute("username");
            var message  = data.DocumentElement.GetAttribute("message");

            var recipient = _clients.FirstOrDefault((client) => client.Name == username);

            if (recipient != null)
            {
                WriteToUserStream(recipient, message);
            }
        }
    }
}