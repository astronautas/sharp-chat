using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace chat_server.Entities
{
    public class User : Entity
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string PasswordHash { get; set; }

        [XmlIgnore]
        public TcpClient Connection { get; set; }

        public User(string name, TcpClient connection) : this(connection)
        {
            Name = name;
        }

        public User(TcpClient connection)
        {
            Connection = connection;
        }

        public User(string name)
        {
            Name = name;
        }

        public User()
        {
        }

        public bool Authenticate(string password)
        {
            if (PasswordHash == password)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void Disconnect()
        {
            Connection.Close();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Connection: {Connection}";
        }
    }
}
