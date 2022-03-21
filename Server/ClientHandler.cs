using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Server
{
    public class ClientHandler 
    {
        public static List<ClientHandler> handlers = 
            new List<ClientHandler>();
        private TcpClient socket;
        private StreamReader reader;
        private StreamWriter writer;
        private string clientUsername;

        public ClientHandler(TcpClient socket)
        {
            try
            {
                this.socket = socket;
                this.reader = new StreamReader(socket.GetStream());
                this.writer = new StreamWriter(socket.GetStream());
                this.clientUsername = Reader.ReadLine(); // wait for the username

                handlers.Add(this);
                BroadcastMessage($"CHAT: {clientUsername} has " +
                    $"entered the chat! ");
            }
            catch (IOException)
            {
                CloseEverything(socket, Reader, Writer);
            }
            catch (Exception e)
            {

            }
            
            

        }

        public TcpClient Socket { get => socket; }
        public StreamReader Reader { get => reader;  }
        public StreamWriter Writer { get => writer;  }
        public string ClientUsername { get => clientUsername; }


        public void BroadcastMessage(string messageToSend)
        {
            foreach (ClientHandler client in handlers)
            {
                try
                {
                    if (!client.ClientUsername.Equals(clientUsername))
                    {
                        client.Writer.WriteLine(messageToSend);
                        client.Writer.Flush();
                    }
                }
                catch (IOException)
                {
                    CloseEverything(socket, Reader, Writer);
                }
            }
        }

        public void RemoveClientHandler()
        {
            handlers.Remove(this);
            BroadcastMessage($"CHAT: {ClientUsername} has left the chat");
        }

        public void CloseEverything(TcpClient socket, StreamReader reader, StreamWriter writer)
        {
             RemoveClientHandler();
            try
            {
                if (socket != null)
                {
                    socket.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);              
            }
            
        }



    }



}