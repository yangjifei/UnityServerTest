using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketServer socketServer = new SocketServer();
            socketServer.Listen();
            Console.ReadKey();
        }
    }
}
