using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using Crypting;

namespace ChatProject
{
    public class ServerModel
    {
        private CrypterRSA crypterRSA = new CrypterRSA();
        static TcpListener tcpListener;
        List<ClientModel> clients = new List<ClientModel>();

        protected internal void AddConnection(ClientModel clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientModel client = clients.FirstOrDefault(c => c.Id == id);

            if (client != null)
                clients.Remove(client);
        }

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                Console.WriteLine("Server is running. Wait for connections...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientModel clientObject = new ClientModel(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        
        protected internal void DistributeMessage(string message, string id)
        {
            string encryptedMessage = message.Trim().Length != 3 ? crypterRSA.Encrypt(message) : message;
            byte[] data = Encoding.Unicode.GetBytes(encryptedMessage);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }

        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }
    }
}