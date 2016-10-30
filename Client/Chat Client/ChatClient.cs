using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace Chat_Client
{
    // Declaring outside the class as it is a separate type
    // and to share it between multiple classes
    public delegate void ChangeEventHandler();

    public class ChatClient : TcpClient
    {
        public string NickName { get; set; }
        public string Log;

        public event ChangeEventHandler LogChanged;

        public ChatClient(string nickName)
        {
            NickName = nickName;
        }

        public ChatClient()
        {

        }

        public bool ConnectToServer()
        {
            try
            {
                Connect(IPAddress.Parse("127.0.0.1"), 8888);
            } catch (SocketException exception)
            {
                return false;
            }

            return true;
        }

        public void WaitForData()
        {
            while (true)
            {
                this.ReceiveBufferSize = 200;
                var reader = new StreamReader(this.GetStream());
                char[] buffer = new char[this.ReceiveBufferSize];

                int bytesRead = reader.Read(buffer, 0, this.ReceiveBufferSize);
                Array.Resize<Char>(ref buffer, bytesRead);

                string dataReceived = new string(buffer);

                if (dataReceived != null)
                {
                    Log += dataReceived;
                    OnLogChanged();
                }
            }
        }

        protected void OnLogChanged()
        {
            if (LogChanged != null)
            {
                LogChanged();
            }
        }

        public bool SendMessage(string message)
        {
            // Add terminating sign to the string
            message = '\u0002' + message + Char.MinValue;

            var charsToSend = Encoding.Default.GetChars(Encoding.Default.GetBytes(message));
            var writer = new StreamWriter(this.GetStream());

            try
            {
                writer.Write(charsToSend, 0, charsToSend.Length);
                writer.Flush();
            } catch (IOException ex)
            {
                return false;
            }
           
            return true;
        }

        public bool Authenticate()
        {
            var doc = new XmlDocument();
            var message = $"<msg command=\"authenticate\"><user username=\"{NickName}\"></user></msg>";

            return SendMessage(message);            
        }

        public bool SendMsgToUser(string username, string message)
        {
            var doc = new XmlDocument();
            var xml = $"<msg command=\"sendMsgToUser\"><user username=\"{username}\" message=\"{message}\"></user></msg>";

            return SendMessage(xml);
        }

        public void ReadStream(Action<string> action)
        {
            while (true)
            {
                this.ReceiveBufferSize = 200;
                var netStream = new StreamReader(this.GetStream());

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

                    string combindedString = string.Join("", data.ToArray());

                    action(combindedString);
                }
            }
        }
    }
}
