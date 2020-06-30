using System;
using System.Threading;

using Fleck;

namespace BrowserCommandMQ
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SingletonRun();

            WebSocketServerStart(args);

            while (true)
            {
                
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        private static void WebSocketServerStart(string[] args)
        {
            var port = 8021;
            var wsServer = new WebSocketServer($"ws://127.0.0.1:{port}");
            var clientManager = new ClientConnectionManager();
            wsServer.Start(connection =>
            {
                var conn = connection.ConnectionInfo;
                connection.OnOpen = () => clientManager.AddConnection(conn.ClientPort, connection);
                connection.OnMessage = clientMessage => { };
                connection.OnError = error => { clientManager.RemoveConnection(conn.ClientPort); };
                connection.OnClose = () => clientManager.RemoveConnection(conn.ClientPort);
            });
        }

        /// <summary>
        /// 单例运行
        /// </summary>
        private static void SingletonRun()
        {
            var mutex = new Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out var isAppRunning);

            if (!isAppRunning)
            {
                Environment.Exit(1);
            }
        }
    }
}