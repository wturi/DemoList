using SuperSocket.SocketBase;
using System;
using System.Text;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketDemo1
{
    public class SocketServer : AppServer<SocketSession>
    {
        public SocketServer() : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser("|", ",")))
        {

        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Console.WriteLine("正在准备配置文件");
            return base.Setup(2012);
        }


        protected override void OnStarted()
        {
            Console.WriteLine("服务已开始");
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            Console.WriteLine("服务已停止");
            base.OnStopped();
        }
        protected override void OnNewSessionConnected(SocketSession session)
        {
            Console.WriteLine("新的连接地址为" + session.LocalEndPoint.Address + ",时间为" + DateTime.Now);
            base.OnNewSessionConnected(session);
        }
    }
}