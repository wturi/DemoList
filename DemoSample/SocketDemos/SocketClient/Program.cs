using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    internal class Program
    {
        private static Thread threadclient = null;
        private static Socket socketclient = null;

        private static void Main(string[] args)
        {
            Connect();

            Task.Run(() =>
            {
                Thread.Sleep(5000);
                ClientSendMsg("5000毫秒后发送的消息");
            });
            Console.ReadLine();
        }

        public static void Connect()
        {
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(address, 8098);
            try
            {
                socketclient.Connect(point);
            }
            catch (Exception)
            {
                return;
            }
            threadclient = new Thread(recv)
            {
                IsBackground = true
            };
            threadclient.Start();
        }

        private static void recv()
        {
            int x = 0;
            while (true)
            {
                try
                {
                    byte[] arrRecvmsg = new byte[1024 * 1024];
                    int length = socketclient.Receive(arrRecvmsg);
                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                    if (x == 1)
                    {
                        Console.WriteLine($"server msg : {strRevMsg}");
                    }
                    else
                    {
                        Console.Write($"server msg : {strRevMsg}");
                        ClientSendMsg($"client first msg");
                        x = 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"server is break {e.Message}");
                    break;
                }
            }
        }

        private static void ClientSendMsg(string sendMsg)
        {
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            socketclient.Send(arrClientSendMsg);
        }
    }
}