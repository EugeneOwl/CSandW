﻿using System;
using System.Net.Sockets;
using System.Text;
using Crypting;

namespace ChatProject
{
    public class ClientModel
    {
        private CrypterRSA crypterRSA = new CrypterRSA();
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerModel server;
        string key;

        public ClientModel(TcpClient tcpClient, ServerModel serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                string message = GetMessage();
                userName = message;

                message = userName + " has just joined.";
                server.DistributeMessage(message, this.Id);
                Console.WriteLine(message);
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        server.DistributeMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: has just leaved.", userName);
                        Console.WriteLine(message);
                        server.DistributeMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.RemoveConnection(this.Id);
                Close();
            }
        }
        
        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            string encryptedMessage = builder.ToString();

            return encryptedMessage.Trim().Length != 3 ? crypterRSA.Decrypt(encryptedMessage) : encryptedMessage;
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}