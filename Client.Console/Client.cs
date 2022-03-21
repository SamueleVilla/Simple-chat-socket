using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client.ConsoleUI
{
    public class Client
    {
        private TcpClient socket;
        private StreamReader reader;
        private StreamWriter writer;
        private string username;


        public Client(TcpClient socket, string username)
        {
            try
            {
                this.socket = socket;
                this.username = username;
                this.reader = new StreamReader(socket.GetStream());
                this.writer = new StreamWriter(socket.GetStream());


            }
            catch (Exception)
            {
                CloseEverything(socket, reader, writer);
            }
        }

        public void SendMessage()
        {
            try
            {
                writer.WriteLine(username);
                writer.Flush();

                while (socket.Connected)
                {
                    string messageToSend = Console.ReadLine();
                    writer.WriteLine($"{username}: {messageToSend}");
                    writer.Flush();
                }
            }
            catch (IOException)
            {
                CloseEverything(socket, reader, writer);
            }
        }

        public void ListenForMessage()
        {
            new Thread(() =>
            {
                string messageFromGrop;

                while (socket.Connected)
                {
                    try
                    {
                        messageFromGrop = reader.ReadLine();
                        Console.WriteLine(messageFromGrop);
                    }
                    catch (IOException)
                    {
                        CloseEverything(socket, reader, writer);
                        break;
                    }
                }

            }).Start();
        }

        private void CloseEverything(TcpClient socket, StreamReader reader, StreamWriter writer)
        {
            try
            {
                if (socket != null)
                {
                    socket.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }
    }
}
