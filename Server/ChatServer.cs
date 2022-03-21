using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace Server
{
    public class ChatServer
    {
        private TcpListener serverSocket;

        public ChatServer(TcpListener serverSocket)
        {
            this.serverSocket = serverSocket;
        }

        public void Start()
        {
            // server start listening for clients
            serverSocket.Start();

            try
            {
                while (serverSocket != null)
                {
                    TcpClient socket = serverSocket.AcceptTcpClient();
                    Console.WriteLine("A new client has connected!");
                    ClientHandler clientHandler = new ClientHandler(socket);

                    Thread thread = new Thread(() => RunOnThread(clientHandler));
                    thread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.Error.WriteLine(e.StackTrace);
                Stop();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.InnerException.StackTrace);
                Stop();
            }
        }

        public void Stop()
        {
            if(serverSocket != null)
            {
                try
                {
                    serverSocket.Stop();
                }
                catch (SocketException e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }
            }
        }

        private void RunOnThread(ClientHandler clientHandler)
        {
            string messageFromClient;

            // current client connected 
            while (clientHandler.Socket.Connected)
            {
                try
                {
                    messageFromClient = 
                        clientHandler.Reader.ReadLine();

                    clientHandler.BroadcastMessage(messageFromClient);
                }
                catch (IOException )
                {
                    clientHandler.CloseEverything(clientHandler.Socket, clientHandler.Reader, clientHandler.Writer);
                }
            }
        }
    }
}
