using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using Crypting;
using UserProcessing;

namespace ChatClient
{
    class Program
    {
        private static UserProcessor userProcessor = new UserProcessor();
        private static CrypterRSA crypterRSA = new CrypterRSA();
        public static string username;
        private static string password;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            Console.WriteLine("(Put \"logup\" for registration.)");
            string toRegistration = Console.ReadLine();
            if (toRegistration == "logup")
                Registration();
            Console.WriteLine("Log in:");
            Login();
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();

                string message = username;
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                Console.WriteLine("Welcome, {0}", username.ToUpper());
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        static private void Login()
        {
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();
                if (!userProcessor.IsUserValid(username, password))
                    Console.WriteLine("Invalid data. Try again.");
                else
                    return;
            }
            while (true);
        }

        static private void Registration()
        {
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();
                if (!userProcessor.IsUsernameFree(username))
                    Console.WriteLine("Username already taken.");
                else
                {
                    userProcessor.RegistrateUser(username, password);
                    return;
                }
            }
            while (true);
        }

        static void SendMessage()
        {
            while (true)
            {
                string message = Console.ReadLine();
                if (message == "quit")
                {
                    Console.WriteLine("Goodbye.");
                    Thread.Sleep(300);
                    Environment.Exit(0);
                }
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = crypterRSA.Decrypt(builder.ToString());
                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Connection interrupted.");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        public static void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }
    }
}