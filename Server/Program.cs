using System;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "GroupChatServer";
            TcpListener serverSocket = new TcpListener(IPAddress.Loopback,5000);
            ChatServer server = new ChatServer(serverSocket);
            server.Start();

        }
    }
}
