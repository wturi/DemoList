using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.IO;

namespace MyApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            JObject data;
            while ((data = Read()) != null)
            {
                var processed = ProcessMessage(data);
                Write(processed);
                if (processed == "exit")
                {
                    return;
                }
            }
        }

        public static string ProcessMessage(JObject data)
        {
            var message = data["text"].Value<string>();
            log2file(message);
            switch (message)
            {
                case "test":
                    return "testing!";

                case "exit":
                    return "exit";

                default:
                    return "MyApp echo: " + message;
            }
        }

        public static JObject Read()
        {
            var stdin = Console.OpenStandardInput();
            var length = 0;

            var lengthBytes = new byte[4];
            stdin.Read(lengthBytes, 0, 4);
            length = BitConverter.ToInt32(lengthBytes, 0);

            var buffer = new char[length];
            using (var reader = new StreamReader(stdin))
            {
                while (reader.Peek() >= 0)
                {
                    reader.Read(buffer, 0, buffer.Length);
                }
            }

            return (JObject)JsonConvert.DeserializeObject<JObject>(new string(buffer));
        }

        public static void Write(JToken data)
        {
            var json = new JObject();
            json["data"] = data;

            var bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString(Formatting.None));

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
            var fileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileStream fs = new FileStream($"{fileName}\\test.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Close();
            fs.Close();
        }
    }
}