using System;
using System.Net.Sockets;

namespace Client.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("Enter the username for the" +
                "group chat: ");
            string username = Console.ReadLine();
            Console.Title = $"Client logged: {username}";

            TcpClient socket = new TcpClient("localhost", 5000);
            Client client = new Client(socket, username);
            client.ListenForMessage();
            client.SendMessage();
        }
    }
}
