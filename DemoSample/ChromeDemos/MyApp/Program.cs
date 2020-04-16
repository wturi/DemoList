using Newtonsoft.Json.Linq;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                    log2file("arg " + i.ToString() + args[i]);

                string chromeMessage = "";
                while (!string.IsNullOrEmpty(chromeMessage = OpenStandardStreamIn()))
                {
                    Write(chromeMessage);
                    log2file("--------------------My application starts with Chrome Extension message: " + chromeMessage + "---------------------------------");

                    Task.Run(() =>
                    {
                        Thread.Sleep(3000);
                        Write("111");
                    });
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

        public static void Write(JToken data)
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
    }
}