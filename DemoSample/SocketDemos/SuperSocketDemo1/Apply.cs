using System;

using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketDemo1
{
    public class Apply : CommandBase<SocketSession, StringRequestInfo>
    {
        public override void ExecuteCommand(SocketSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine($"接受到的参数:{requestInfo.Body}---{Environment.NewLine}");
            //根据参数个数或者其他条件判断，来进行一些自己的操作
            if (requestInfo.Parameters.Length == 2)
            {
                Console.WriteLine("调用成功");
                session.Send("已经成功接收到你的请求\r\n");
            }
            else
            {
                session.Send("参数不正确\r\n");
            }
        }
    }
}