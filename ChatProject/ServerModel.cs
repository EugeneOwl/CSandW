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
        List<string> usernames = new List<string>();

        protected internal void AddConnection(ClientModel clientModel)
        {
            clients.Add(clientModel);
        }

        private bool IsClientOnline(string username)
        {
            return usernames.Contains(username);
        }

        public void RegistrateClient(string username, string id)
        {
            if (!IsClientOnline(username))
                usernames.Add(username);
            else
            {
                RemoveConnection(id, username);
                throw new Exception("Client with username " + username.ToUpper() + " is already online.");
            }
        }

        protected internal void RemoveConnection(string id, string username = "")
        {
            if (usernames.Contains(username))
                usernames.Remove(username);
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
                Console.WriteLine("Server is running.");

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
            string encryptedMessage = crypterRSA.Encrypt(message);
            byte[] data = Encoding.UTF8.GetBytes(encryptedMessage);
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