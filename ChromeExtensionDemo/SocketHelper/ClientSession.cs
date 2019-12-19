using System;
using System.Net;
using System.Net.Sockets;

namespace SocketHelper
{
    public class ClientSession
    {
        public Socket ClientSocket { get; set; }
        public string IP;

        public ClientSession(Socket clientSocket)
        {
            this.ClientSocket = clientSocket;
            this.IP = GetIPStr();
        }

        public string GetIPStr()
        {
            string resStr = ((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString();
            return resStr;
        }
    }

    public class SocketConnection : IDisposable
    {
        public Byte[] msgBuffer = new byte[1024];
        private Socket _clientSocket = null;

        public Socket ClientSocket
        {
            get { return this._clientSocket; }
        }

        #region 构造

        public SocketConnection(Socket sock)
        {
            this._clientSocket = sock;
        }

        #endregion 构造

        #region 连接

        public void Connect(IPAddress ip, int port)
        {
            this.ClientSocket.BeginConnect(ip, port, ConnectCallback, this.ClientSocket);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndConnect(ar);
            }
            catch (SocketException ex)
            {
            }
        }

        #endregion 连接

        #region 发送数据

        public void Send(string data)
        {
            Send(System.Text.Encoding.UTF8.GetBytes(data));
        }

        private void Send(byte[] byteData)
        {
            try
            {
                this.ClientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), this.ClientSocket);
            }
            catch (SocketException ex)
            {
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
            }
            catch (SocketException ex)
            {
            }
        }

        #endregion 发送数据

        #region 接收数据

        public void ReceiveData()
        {
            ClientSocket.BeginReceive(msgBuffer, 0, msgBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int REnd = ClientSocket.EndReceive(ar);
                if (REnd > 0)
                {
                    byte[] data = new byte[REnd];
                    Array.Copy(msgBuffer, 0, data, 0, REnd);

                    //在此处对数据进行处理

                    ClientSocket.BeginReceive(msgBuffer, 0, msgBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                }
                else
                {
                    Dispose();
                }
            }
            catch (SocketException ex)
            {
            }
        }

        public void Dispose()
        {
            try
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion 接收数据
    }
}