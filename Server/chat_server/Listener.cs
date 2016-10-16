using System.Net;
using System.Net.Sockets;

namespace chat_server
{
    public class Listener : TcpListener
    {
        public new bool Active // TODO: why NEW??
        {
            get
            {
                return base.Active;
            }
        }

        public bool HasStartedAcceptingClient { get; set; }

        public Listener(IPEndPoint localEP) : base(localEP)
        {
        }

        public Listener(IPAddress localaddr, int port) : base(localaddr, port)
        {
        }
    }
}
