using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Models
{
    public class User
    {
        public string Name { get; set; }
        public TcpClient Connection { get; set; }

        public User(string name, TcpClient connection) : this(connection)
        {
            Name = name;
        }

        public User(TcpClient connection)
        {
            Connection = connection;
        }
    }
}
