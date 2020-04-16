using Newtonsoft.Json.Linq;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotTimeNativeMessage
{
    internal class Program
    {
        private static bool _isSocketStart = false;
        private static List<Socket> Sockets = new List<Socket>();
        static SocketHelp sh = new SocketHelp(false);

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                    log2file("arg " + i.ToString() + args[i]);

                string chromeMessage = "";
                //StartSocket();
                while (!string.IsNullOrEmpty(chromeMessage = OpenStandardStreamIn()))
                {
                    Write(chromeMessage + "返回的");
                    Sockets.ForEach(s => s.Send(sh.PackData(chromeMessage)));
                    log2file("--------------------My application starts with Chrome Extension message: " + chromeMessage + "---------------------------------");
                    if (!_isSocketStart)
                    {
                        Task.Run(() =>
                        {
                            StartSocketV2();
                        });
                    }
                }
            }
            log2file("--------------------program end at " + DateTime.Now.ToString() + "--------------------");
        }

        private static string OpenStandardStreamIn()
        {
            //// We need to read first 4 bytes for length information
            Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);

            string input = "";
            for (int i = 0; i < length; i++)
            {
                input += (char)stdin.ReadByte();
            }

            return input;
        }

        private static void Write(JToken data)
        {
            var json = new JObject();
            json["data"] = data;

            var bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString());

            var stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((bytes.Length >> 0) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 8) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 16) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 24) & 0xFF));
            stdout.Write(bytes, 0, bytes.Length);
            stdout.Flush();
        }

        private static void log2file(string s)
        {
            var fileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            FileStream fs = new FileStream($"{fileName}\\test.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Close();
            fs.Close();
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
                    Thread t = new Thread(new ThreadStart(() => ReceiveData(sc))); //开启当前Socket线程, 去执行获取数据的动作,与客户端通信
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch (Exception e)
            {
                Write(e.ToString());
            }
        }

        public static void ReceiveData(Socket sc)
        {
            byte[] buffer = new byte[1024];
            Write("接受到了客户端：" + sc.RemoteEndPoint.ToString() + "连接....");
            //握手
            int length = sc.Receive(buffer);//接受客户端握手信息
            sc.Send(sh.PackHandShakeData(sh.GetSecKeyAccetp(buffer, length)));
            while (true)
            {
                try
                {
                    //接受客户端数据
                    Write("等待客户端数据....");
                    length = sc.Receive(buffer);//接受客户端信息
                    string clientMsg = sh.GetSecKeyAccetp(buffer, length);
                    Write("接受到客户端数据：" + clientMsg);
                    //发送数据
                    string sendMsg = "服务端返回信息:" + clientMsg;
                    sc.Send(sh.PackData(sendMsg));
                }
                catch (Exception ex)
                {
                    Sockets.Remove(sc);  //如果接收的过程中,断开, 那么内存中移除当前Socket对象, 并且退出当前线程
                    Write("客户端已经断开连接!");
                    return;
                }
            }
        }
    }
}