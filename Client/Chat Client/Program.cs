using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Client
{
    static class Program
    {
        static void Main()
        {
            var client = new ChatClient("lukas123");

            //new Thread(() =>
            //{
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new ChatClientGui(client));
            //}).Start();

            var console = new ChatConsole(client);
        }
    }
}
