using System;
using System.Text;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketDemo1
{
    public class SocketSession : AppSession<SocketSession>
    {
        public override void Send(string message)
        {
            Console.WriteLine("发送消息:" + message);
            base.Send(message);
        }

        protected override void OnSessionStarted()
        {
            Console.WriteLine("Session已启动");
            base.OnSessionStarted();
        }

        protected override void OnInit()
        {
            this.Charset = Encoding.GetEncoding("gb2312");
            base.OnInit();
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Console.WriteLine("遇到未知的请求");
            base.HandleUnknownRequest(requestInfo);
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("Session已断开");
            base.OnSessionStarted();
        }
    }
}