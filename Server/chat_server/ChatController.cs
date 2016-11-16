using chat_server.Exceptions;
using chat_server.Entities;
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
    public class ChatController
    {
        private static bool _hasInstance;
        public static ChatController Instance { get; set; }

        public Listener listener;
        private Router _router;

        private EntityManager<User> _userManager = EntityManagerFactory.Instance.GetManager<User>();


        // Events
        private event Action<User> OnUserNotConnectedCheck;

        // Online client list
        private List<User> _clients;
        private List<User> Clients
        {
            get
            {
                if (_clients == null)
                {
                    _clients = new List<User>();
                }

                return _clients;
            }
        }

        private List<User> _allClients;
        private List<User> AllClients
        {
            get
            {
                if (_allClients == null)
                {
                    _allClients = _userManager.All();
                }

                return _allClients;
            }
        }

        public List<ILog> loggers;

        // Thread signal.
        public static ManualResetEvent tcpClientConnected =  new ManualResetEvent(false);

        private ChatController()
        {
            _router = new Router();
            loadRoutes();

            OnUserNotConnectedCheck += (user) =>
            {
                user.Disconnect();

                _clients.RemoveAll((client) =>
                {
                    return client.Name == user.Name;
                });
            };
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
            _router.AddRoute("sendMsgToUser", BeforeSendMsgToUser);
            _router.AddRoute("requestToChat", RequestToChatWith);
            _router.AddRoute("requestConfirmationFromClient", AcceptChatRequest);
            _router.AddRoute("allUsers", SendAllClients);

            _router.AddRoute("authenticate", new Action<User, string>
                ((user, parameters) => AuthenticateUser(user, parameters)));
        }

        public void RequestToChatWith(User requester, string message)
        {
            var doc = new XmlDocument();
            doc.LoadXml(message);

            var userToChatWith = doc.DocumentElement.GetAttribute("username");

            var chatpal = Clients.Find((client) => client.Name == userToChatWith);

            var xmlMessage = $"<msg command=\"receivedChatRequest\" sender=\"{requester.Name}\"></msg>";

            if (chatpal != null)
            {
                SendMsgToUser(chatpal, xmlMessage);
            }
        }

        public void AcceptClient(IAsyncResult result)
        {
            // Get the listener that handles the client request.
            Listener listener = (Listener)result.AsyncState;

            var client = listener.EndAcceptTcpClient(result);

            User user = new User(client);
            Clients.Add(user);

            // Make the server listen for the client messages
            new Task(delegate { ListenForMessages(user); }).Start();

            Log("Accepted a client connection");

            // Listen for more clients
            Listen();
        }

        public void Listen()
        {
            Log($"Listening for clients...");
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
                receivedData = GetUserStreamData(netStream, user);

                if (receivedData.Count() > 0)
                {
                    // Retrieve user information
                    var data = new XmlDocument();
                    data.LoadXml(receivedData);

                    var cmd   = data.DocumentElement.GetAttribute("command");

                    try
                    {
                        Log($"{user.Name} sent a message " + receivedData);
                        _router.Route(cmd, user, receivedData);
                    } catch (UserCommandNotRecognizedException e)
                    {
                        Log(e.Message);
                    }
                }
            }
        }

        private string GetUserStreamData(StreamReader netStream, User user)
        {
            // Using a list as we do not not the length of incoming data
            List<char> data = new List<char>(0);
            char[] buffer = new char[1];
            int readBytes;

            try
            {
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

            } catch (Exception ex) when (ex is IOException|| ex is ObjectDisposedException)
            {
                OnUserNotConnectedCheck.Invoke(user);
            }

            return new string(data.ToArray());
        }

        private void WriteToUserStream(User user, string text)
        {
            // Add delimiters
            StreamWriter writer;
            text = '\u0002' + text + '\0';

            var charsToSend = text.ToCharArray();

            try
            {
                writer = new StreamWriter(user.Connection.GetStream());
                writer.Write(charsToSend, 0, charsToSend.Length);
                writer.Flush();
            } catch (Exception e)
            {
                OnUserNotConnectedCheck.Invoke(user);
            }
        }

        // Route callbacks - data = xml element
        private void AuthenticateUser(User user, string parameters)
        {
            string acceptanceMessage;
            var data = new XmlDocument();
            data.LoadXml(parameters);
            var username = data.DocumentElement.GetAttribute("username" );
            var password = data.DocumentElement.GetAttribute("password");

            // Temporary user details
            user.Name = username;
            user.PasswordHash = password;

            // Get full user details
            var realUser = _userManager.Get(new Dictionary<string, string>
            {
                {"key", "Name" },
                {"value", user.Name }
            });

            if (!_userManager.Exists("Name", user.Name))
            {
                _userManager.Create(user);

                acceptanceMessage = $"<msg command=\"authConfirmation\" value=\"true\"></msg>";
            }
            else
            {
                if (realUser.Authenticate(user.PasswordHash)) {
                    acceptanceMessage = $"<msg command=\"authConfirmation\" value=\"true\"></msg>";
                }
                else
                {
                    acceptanceMessage = $"<msg command=\"authConfirmation\" value=\"false\"></msg>";
                    user.PasswordHash = null;
                }
            }

            SendMsgToUser(user, acceptanceMessage);
        }

        private void SendMsgToUser(User recipient, string xmlMessage)
        {
            var data = new XmlDocument();
            data.LoadXml(xmlMessage);

            if (recipient != null)
            {
                WriteToUserStream(recipient, xmlMessage);
            }
        }

        private void AcceptChatRequest(User sender, string xmlMessage)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlMessage);
            var rootEl = doc.DocumentElement;

            var acceptedUsername = rootEl.GetAttribute("acceptedUser");
            var acceptedClient = Clients.Find((client) => client.Name == acceptedUsername);

            var acceptanceMessage = $"<msg userToChatWith=\"{sender.Name}\" command=\"chatRequestConfirmation\" value=\"true\"></msg>";

            SendMsgToUser(acceptedClient, acceptanceMessage);
        }

        private void BeforeSendMsgToUser(User sender, string xmlMessage)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlMessage);
            var rootEl = doc.DocumentElement;

            var recipient = Clients.Find((client) => client.Name == rootEl.GetAttribute("username"));

            rootEl.SetAttribute("username", sender.Name);

            SendMsgToUser(recipient, rootEl.OuterXml);
        }

        private void SendAllClients(User sender, string xmlMessage)
        {
            string clientsXml = "<msg command=\"allClients\">";

            var clients = AllClients.Where((client) =>
            {
                // Checks if current client is online client
                var clientMatch = Clients.Find((onlineClient) =>
                {
                    return client.Name == onlineClient.Name
                           && client.Name != sender.Name;
                });

                if (clientMatch != null)
                {
                    return true;
                } else
                {
                    return false;
                }
            });

            foreach (var client in clients)
            {
                var nodeXml = $"<user username=\"{client.Name}\"></user>";
                clientsXml += nodeXml;
            }

            clientsXml += "</msg>";

            SendMsgToUser(sender, clientsXml);
        }

        private void Log(string message)
        {
            foreach (var logger in loggers)
            {
                logger.Log(message);
            }
        }
    }
}