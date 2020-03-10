using System;
using System.Linq;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketDemo1
{
    internal class Program
    {
        private static int _startPort = 2018;

        private static void Main(string[] args)
        {
            var appServer = Init();

            Console.WriteLine($"the server start port : {_startPort}");

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();

            Console.ReadLine();
        }

        private static SocketServer Init(int retrySetupNum = 0, int retryStartNum = 0)
        {
            var appServer = new SocketServer();

            if (!appServer.Setup(_startPort))
            {
                //失败则端口加1重新连接
                _startPort++;
                if (retrySetupNum < 10)
                    return Init(++retrySetupNum);
                throw new Exception("server setup error");
            }

            if (appServer.Start()) return appServer;

            //失败则端口加1重新连接
            _startPort++;
            if (retryStartNum < 10)
                return Init(retrySetupNum, ++retryStartNum);
            throw new Exception("server start error");
        }
    }

    public class ADD : CommandBase<SocketSession, StringRequestInfo>
    {
        public override void ExecuteCommand(SocketSession session, StringRequestInfo requestInfo)
        {
            session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
        }
    }

    public class MULT : CommandBase<SocketSession, StringRequestInfo>
    {
        public override void ExecuteCommand(SocketSession session, StringRequestInfo requestInfo)
        {
            var result = 1;

            foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
            {
                result *= factor;
            }

            session.Send(result.ToString());
        }
    }
}