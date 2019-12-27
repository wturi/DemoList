using SocketExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                SocketServer.Init();
            }
            catch (Exception e)
            {

                throw;
            }

            Console.ReadLine();
        }
    }
}
