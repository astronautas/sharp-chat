﻿using System;
using System.IO;
using System.Net.Sockets;

namespace Chat_Client
{
    // Declaring outside the class as it is a separate type
    // and to share it between multiple classes
    public delegate void ChangeEventHandler();

    public class ChatClient : TcpClient
    {
        public string NickName { get; private set; }
        public string Log;

        public event ChangeEventHandler LogChanged;

        public ChatClient(string nickName)
        {
            NickName = nickName;
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
            this.ReceiveBufferSize = message.Length;

            var charsToSend = message.ToCharArray();
            var writer = new StreamWriter(this.GetStream());
            writer.Write(charsToSend, 0, charsToSend.Length);

            writer.Flush();

            return true;
        }
    }
}
