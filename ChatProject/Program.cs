using System;
using System.Threading;

namespace ChatProject
{
    class Program
    {
        static ServerModel server;
        static Thread listenThread; 
        static void Main(string[] args)
        {
            try
            {
                server = new ServerModel();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}