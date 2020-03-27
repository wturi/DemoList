using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BotTimeNativeMessage
{
    internal class Program
    {
        private static bool _isSocketStart = false;
        private static readonly List<Socket> Sockets = new List<Socket>();
        private static readonly SocketHelp Sh = new SocketHelp(false);

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0) return;
            string chromeMessage = "";
            //StartSocket();
            while (!string.IsNullOrEmpty(chromeMessage = OpenStandardStreamIn()))
            {
                Write(chromeMessage + "返回的");
                Sockets.ForEach(s => s.Send(Sh.PackData(chromeMessage)));
                if (!_isSocketStart)
                {
                    Task.Run(StartSocketV2);
                }
            }
        }

        private static string OpenStandardStreamIn()
        {
            //// We need to read first 4 bytes for length information
            var standard = Console.OpenStandardInput();
            var bytes = new byte[4];
            standard.Read(bytes, 0, 4);
            var length = BitConverter.ToInt32(bytes, 0);

            var input = "";
            for (var i = 0; i < length; i++)
            {
                input += (char)standard.ReadByte();
            }

            return input;
        }

        private static void Write(JToken data)
        {
            var json = new JObject
            {
                ["data"] = data
            };

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString());

            var stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((bytes.Length >> 0) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 8) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 16) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 24) & 0xFF));
            stdout.Write(bytes, 0, bytes.Length);
            stdout.Flush();
        }

        private static void StartSocketV2()
        {
            int port = 8881;
            byte[] buffer = new byte[1024];

            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(2);
                _isSocketStart = true;
                Write("等待客户端连接....");
                while (true) //该操作用于多个客户端连接
                {
                    Socket sc = listener.Accept();//接受一个连接
                    Sockets.Add(sc); //将连接的客户端, 添加到内存当中
                    var t = new Thread(() => ReceiveData(sc))
                    {
                        IsBackground = true
                    }; //开启当前Socket线程, 去执行获取数据的动作,与客户端通信
                    t.Start();
                }
            }
            catch (Exception e)
            {
                Write(e.ToString());
            }
        }

        private static void ReceiveData(Socket sc)
        {
            var buffer = new byte[1024];
            Write("接受到了客户端：" + sc.RemoteEndPoint.ToString() + "连接....");
            //握手
            var length = sc.Receive(buffer);//接受客户端握手信息
            sc.Send(Sh.PackHandShakeData(Sh.GetSecKeyAccetp(buffer, length)));
            while (true)
            {
                try
                {
                    //接受客户端数据
                    Write("等待客户端数据....");
                    length = sc.Receive(buffer);//接受客户端信息
                    var clientMsg = Sh.GetSecKeyAccetp(buffer, length);
                    Write("接受到客户端数据：" + clientMsg);
                    //发送数据
                    var sendMsg = "服务端返回信息:" + clientMsg;
                    sc.Send(Sh.PackData(sendMsg));
                }
                catch (Exception)
                {
                    Sockets.Remove(sc);  //如果接收的过程中,断开, 那么内存中移除当前Socket对象, 并且退出当前线程
                    Write("客户端已经断开连接!");
                    return;
                }
            }
        }
    }
}