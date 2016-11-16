using chat_server.Entities;
using chat_server.Tests;
using chat_server.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace chat_server
{
    class Program
    {
        static ChatController chatController;

        static void Main(string[] args)
        {
            //EntityManagerTests.Init();
            //ChatControllerTests.Init();
            //FileLoggerTests.Init();
            //RouterTests.Init();

            SubscribeLoggers();

            if (Test.errorsExist)
            {
                Debug.WriteLine("[TEST] Fix testing errors");
                Environment.Exit(1);
            }

            chatController = ChatController.GetInstance();
            chatController.loggers = GetLoggers();

            // Initializes listener on specified IP/Port combination
            chatController.listener = new Listener(IPAddress.Parse(ConfigurationManager.AppSettings["IP"]),
                                                   Int32.Parse(ConfigurationManager.AppSettings["PORT"]));
            chatController.listener.Start();
            chatController.listener.HasStartedAcceptingClient = false;
            chatController.Listen();
        }

        private static void SubscribeLoggers()
        {
            // Log user creation
            EntityManager<User>.onEntityCreate += (user) => FileLogger.Instance.Log(user.ToString());
        }

        private static List<ILog> GetLoggers()
        {
            return new List<ILog> { FileLogger.Instance, ConsoleLogger.Instance };
        }
    }
}
