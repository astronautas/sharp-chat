using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Chat_Client
{

    public class ChatClient
    {

        public string NickName { get; set; }
        public string Password { get; set; }
        public string Log;
        public string CurrentChatPal { get; set; }

        public List<string> CurrChatMsgHistory = new List<string>();
        public List<string> ClientList = new List<string>();

        public event Action OnClientListChange = delegate { };
        public event Action OnReceiveMessageFromUser = delegate { };
        public event Action OnFailedChatRequest = delegate { };
        public event Action OnInitiateChatWithUser = delegate { };
        public event Action onAuthSuccess = delegate { };
        public event Action onAuthFailure = delegate { };

        public TcpClient Connection = new TcpClient();

        // Methods add themselves to the router.
        // Router maps string commands comming from the server to (extracted from XML)
        // to callbacks receiving the full message again
        private Dictionary<string, Action<XmlDocument>> _router = new Dictionary<string, Action<XmlDocument>>() { };

        public ChatClient(string nickName)
        {
            NickName = nickName;
        }

        public ChatClient()
        {
            _router.Add("authConfirmation", AuthenticationConfirmation);
            _router.Add("receivedChatRequest", ReceiveChatRequest);
            _router.Add("sendMsgToUser", ReceiveMessageFromUser);
            _router.Add("allClients", ReceiveClientListUpdate);

            new Task(ReadStream).Start();
        }

        public bool ConnectToServer()
        {
            try
            {
                Connection.Connect(IPAddress.Parse(ConfigurationManager.AppSettings["IP"]), 8888);
            } catch (SocketException exception)
            {
                return false;
            }

            return true;
        }

        public bool IsConnected()
        {
            if (!SendMessage(""))
            {
                return false;
            } else
            {
                return true;
            }
        }

        public void WaitForData()
        {
            while (true)
            {
                this.Connection.ReceiveBufferSize = 200;
                var reader = new StreamReader(this.Connection.GetStream());
                char[] buffer = new char[this.Connection.ReceiveBufferSize];

                int bytesRead = reader.Read(buffer, 0, this.Connection.ReceiveBufferSize);
                Array.Resize<Char>(ref buffer, bytesRead);

                string dataReceived = new string(buffer);

                if (dataReceived != null)
                {
                    Log += dataReceived;
                }
            }
        }

        public bool SendMessage(string message)
        {
            // Add terminating sign to the string
            message = '\u0002' + message + Char.MinValue;

            var charsToSend = Encoding.Default.GetChars(Encoding.Default.GetBytes(message));

            try
            {
                var writer = new StreamWriter(this.Connection.GetStream());
                writer.Write(charsToSend, 0, charsToSend.Length);
                writer.Flush();
            } catch (Exception ex)
            {
                return false;
            }
           
            return true;
        }

        public bool Authenticate()
        {
            var doc = new XmlDocument();
            var message = $"<msg command=\"authenticate\" username=\"{NickName}\" password=\"{Password}\"></msg>";

            return SendMessage(message);            
        }

        public void SendChatRequest(string username)
        {
            var doc = new XmlDocument();
            var xml = $"<msg command=\"requestToChat\" username=\"{username}\"></msg>";

            _router.Add("chatRequestConfirmation", ChatRequestConfirmationCallback);

            // No need to wait for chat request if connection failed
            if (!SendMessage(xml)) {
                _router.Remove("chatRequestGranted");
                OnFailedChatRequest.Invoke();
            }
        }

        public bool SendMsgToUser(string recipient, string message)
        {
            var doc = new XmlDocument();
            var xml = $"<msg command=\"sendMsgToUser\" username=\"{recipient}\" message=\"{message}\"></msg>";

            CurrChatMsgHistory.Add($"[{NickName}] {message}");
            OnReceiveMessageFromUser.Invoke();

            return SendMessage(xml);
        }

        // Reads stream comming from server
        // Informs subscribers when there is new data
        public void ReadStream()
        {
            while (true)
            {
                if (!IsConnected()) continue;

                this.Connection.ReceiveBufferSize = 200;
                var netStream = new StreamReader(this.Connection.GetStream());

                // Using a list as we do not not the length of incoming data
                List<char> data = new List<char>(0);
                char[] buffer = new char[1];

                var readBytes = netStream.Read(buffer, 0, buffer.Length);

                // Read everything from start to end delimeter
                if (buffer[0] == '\u0002' && readBytes != -1)
                {
                    do
                    {
                        readBytes = netStream.Read(buffer, 0, buffer.Length);
                        data.Add(buffer[0]);
                    } while (!(buffer[0] == '\0'));   
                              
                    string receivedString = new string(data.ToArray());

                    data.RemoveAt(data.Count - 1);

                    var doc = new XmlDocument();
                    doc.LoadXml(receivedString);
                    var cmd = doc.DocumentElement.GetAttribute("command");

                    // Router maps commands to corespoding actions
                    if (_router.ContainsKey(cmd)) {
                        _router[cmd](doc);
                    }
                }
            }
        }

        public void UpdateClientsList()
        {
            SendMessage("<msg command=\"allUsers\"></msg>");
        }

        // CALLBACKS

        // Sending Chat request
        public void ChatRequestConfirmationCallback(XmlDocument messageDoc)
        {
            var rootEl = messageDoc.DocumentElement;

            // If passes, it has all the required attributes for this callback
            if (!rootEl.HasAttribute("userToChatWith") ||
                !rootEl.HasAttribute("command") ||
                !rootEl.HasAttribute("value"))
            {
                return;
            }

            var username = messageDoc.DocumentElement.GetAttribute("userToChatWith");
            var cmd = messageDoc.DocumentElement.GetAttribute("command");
            var value = messageDoc.DocumentElement.GetAttribute("value");

            if (value == "true")
            {
                CurrentChatPal = username;
                OnInitiateChatWithUser.Invoke();
            } else
            {
                OnFailedChatRequest.Invoke();
            }

        }

        // Receiving chat request
        public void ReceiveChatRequest(XmlDocument messageDoc)
        {
            // TODO: add confirmation popup
            var rootEl = messageDoc.DocumentElement;

            CurrentChatPal = rootEl.GetAttribute("sender");
            OnInitiateChatWithUser.Invoke();

            // Inform the server that request has been granted
            var msg = $"<msg acceptedUser=\"{CurrentChatPal}\" command=\"requestConfirmationFromClient\" value=\"true\"></msg>";
            SendMessage(msg);
        }

        // Receiving messages
        private void ReceiveMessageFromUser(XmlDocument messageDoc)
        {
            var rootEl = messageDoc.DocumentElement;
            var msg = $"[{rootEl.GetAttribute("username")}] ";

            msg += rootEl.GetAttribute("message");
            CurrChatMsgHistory.Add(msg);

            OnReceiveMessageFromUser.Invoke();
        }

        private void ReceiveClientListUpdate(XmlDocument messageDoc)
        {
            var rootEl = messageDoc.DocumentElement;

            var clientNameElements = rootEl.GetElementsByTagName("user");
            ClientList = new List<string>();

            foreach (XmlNode clientNameElement in clientNameElements)
            {
                ClientList.Add(clientNameElement.Attributes["username"].Value);
            }

            OnClientListChange.Invoke();
        }

        private void AuthenticationConfirmation(XmlDocument messageDoc)
        {
            var rootEl = messageDoc.DocumentElement;

            var value = rootEl.GetAttribute("value");

            if (value == "true")
            {
                onAuthSuccess.Invoke();
            } else
            {
                onAuthFailure.Invoke();
            }
        }
    }
}
