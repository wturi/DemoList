using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EncootechNativeMessage
{
    public class SocketHelp
    {
        private int _maxConnect = 2;

        public List<ClientConnectionItem> clientConnectionItems = new List<ClientConnectionItem> { };

        public bool IsSocketStart = false;

        public SocketHelp()
        {
        }

        /// <summary>
        /// 启动socket服务
        /// </summary>
        public void StartSocket()
        {
            int port = 8881;
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(1);
                IsSocketStart = true;
                StandardStreamHelper.Write("等待客户端连接....");
                while (true)
                {
                    Socket sc = listener.Accept();
                    Thread t = new Thread(new ThreadStart(() => ReceiveData(sc)))
                    {
                        IsBackground = true
                    };
                    t.Start();
                }
            }
            catch (Exception e)
            {
                StandardStreamHelper.Write(e.ToString());
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sc"></param>
        private void ReceiveData(Socket sc)
        {
            if (!VerifyAllConnectionOnline())
            {
                sc.Close();
                return;
            }
            clientConnectionItems.Add(new ClientConnectionItem { SocketItem = sc });
            byte[] buffer = new byte[1024 * 1024];
            StandardStreamHelper.Write("接受到了客户端：" + sc.RemoteEndPoint.ToString() + "连接....");
            int length;
            try
            {
                length = sc.Receive(buffer);//接受客户端握手信息
                sc.Send(PackData(VerifyConnection(AnalyticData(buffer, length), sc)));
            }
            catch
            {
            }
            while (true)
            {
                try
                {
                    length = sc.Receive(buffer);//接受客户端信息
                    string clientMsg = AnalyticData(buffer, length);
                    StandardStreamHelper.Write("接受到客户端数据：" + clientMsg);
                    //发送数据
                    string sendMsg = clientMsg;
                    sc.Send(PackData(sendMsg));
                }
                catch
                {
                    clientConnectionItems.Remove(clientConnectionItems.FirstOrDefault(i => i.SocketItem == sc));
                    StandardStreamHelper.Write(sc.RemoteEndPoint.ToString() + "客户端已经断开连接!");
                    return;
                }
            }
        }

        /// <summary>
        /// 验证连接,的首次消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sc"></param>
        public string VerifyConnection(string message, Socket sc)
        {
            if (string.IsNullOrEmpty(message))
            {
                clientConnectionItems.Remove(clientConnectionItems.FirstOrDefault(item => item.SocketItem == sc));
                sc.Close();
                return "";
            }
            else
            {
                clientConnectionItems.FirstOrDefault(item => item.SocketItem == sc).IsVerify = true;
            }
            return "connect success";
        }

        /// <summary>
        /// 解析客户端数据包
        /// </summary>
        /// <param name="recBytes">服务器接收的数据包</param>
        /// <param name="recByteLength">有效数据长度</param>
        /// <returns></returns>
        public string AnalyticData(byte[] recBytes, int recByteLength)
        {
            string handShakeText = Encoding.UTF8.GetString(recBytes, 0, recByteLength);
            return handShakeText;
        }

        /// <summary>
        /// 打包服务器数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>数据包</returns>
        public byte[] PackData(string message)
        {
            byte[] temp = Encoding.UTF8.GetBytes(message);
            return temp;
        }

        /// <summary>
        /// 验证所有连接有效性
        /// </summary>
        public bool VerifyAllConnectionOnline()
        {
            return clientConnectionItems.Count() < _maxConnect;
        }
    }

    public class ClientConnectionItem
    {
        public Socket SocketItem { get; set; }
        public bool IsVerify { get; set; } = false;
    }
}