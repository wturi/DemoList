using SuperSocket.SocketBase;
using System;
using System.Text;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketDemo1
{
    public class SocketServer : AppServer<SocketSession>
    {
        protected override void OnNewSessionConnected(SocketSession session)
        {
            Console.WriteLine("新的连接地址为" + session.LocalEndPoint.Address + ",时间为" + DateTime.Now);
            base.OnNewSessionConnected(session);
        }
    }
}