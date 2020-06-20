using Newtonsoft.Json.Linq;

using System;
using System.IO;

namespace EncootechNativeMessage
{
    public static class StandardStreamHelper
    {
        public static void Write(JToken data)
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

        public static string OpenStandardStreamIn()
        {
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
    }
}