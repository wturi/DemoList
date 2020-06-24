using SocketExtension;

using System;

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