using SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ClientBase client = new ClientBase("192.168.56.1", 9729, ReceivedAction);

            for (int i = 1; i < 100; i++)
            {
                client.ClientSend(ContextType.Txt, Encoding.UTF8.GetBytes(i.ToString()));
                Thread.Sleep(1000);
            }


            Console.ReadKey();
        }

        private static void ReceivedAction(string receivedMsg)
        {
            Console.WriteLine(receivedMsg);
        }
    }
}
