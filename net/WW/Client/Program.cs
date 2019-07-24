using SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IClient client = new ClientBase("192.168.56.1", 9729, ReceivedAction);
            //IClient client = new ClientBase("10.254.0.221", 9729, ReceivedAction);

            for (int i = 1; i < 5; i++)
            {
                client.ClientSend(ContextType.Txt, Encoding.UTF8.GetBytes(i.ToString()));
                Thread.Sleep(1000);
            }


            string fileName = "D:\\soft\\virtualdrivemaster.exe";
            //string fileName = "C:\\Users\\Sandy\\Desktop\\DVDMaker.exe";

            client.ClientSendFileLoop(fileName);

            client.ClientSendFile(fileName);


            //client.ClientSend(ContextType.Close, new byte[] { 0 });

            Console.ReadKey();
        }

        private static void ReceivedAction(string receivedMsg)
        {
            Console.WriteLine(receivedMsg);
        }
    }
}
