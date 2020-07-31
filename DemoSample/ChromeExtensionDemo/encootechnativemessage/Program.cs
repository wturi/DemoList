using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace EncootechNativeMessage
{
    internal class Program
    {
        private static SocketHelp sh = new SocketHelp();

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string chromeMessage = "";
                while (!string.IsNullOrEmpty(chromeMessage = StandardStreamHelper.OpenStandardStreamIn()))
                {
                    StandardStreamHelper.Write(chromeMessage + "返回的");
                    sh.clientConnectionItems.ForEach(s => s.SocketItem.Send(sh.PackData(chromeMessage)));
                    if (!sh.IsSocketStart)
                    {
                        Task.Run(() =>
                        {
                            sh.StartSocket();
                        });
                    }
                }
            }
        }

        private static string OpenStandardStreamIn()
        {
            //// We need to read first 4 bytes for length information
            Stream stdin = Console.OpenStandardInput();
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            int length = BitConverter.ToInt32(bytes, 0);

            string input = "";
            for (int i = 0; i < length; i++)
            {
                input += (char)stdin.ReadByte();
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
    }
}