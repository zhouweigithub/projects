using SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IServer server = new ServerBase(string.Empty, 9729);
        }


    }
}
