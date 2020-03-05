using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;

namespace SuperSocketDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 初始化Socket
            IBootstrap bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                Console.WriteLine(DateTime.Now + ":Socket初始化失败\r\n");
                return;
            }

            var result = bootstrap.Start();
            foreach (var server in bootstrap.AppServers)
            {
                if (server.State == ServerState.Running)
                {
                    Console.WriteLine(DateTime.Now + ":serverName为:" + server.Name + "Socket运行中\r\n");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine(DateTime.Now + ":serverName为:" + server.Name + "Socket启动失败\r\n");
                }
            }
            #endregion


            Console.ReadLine();

        }
    }
}
