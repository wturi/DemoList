using System;

using SocketExtension;

namespace SocketServerDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SocketServer.Init();
            Console.ReadLine();
        }
    }
}