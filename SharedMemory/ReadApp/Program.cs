using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace ReadApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("输入对应地址，输入 c 为默认chrome地址：BotTimeNativeMessageHostSharedMemory");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path) || path == "c")
            {
                path = "BotTimeNativeMessageHostSharedMemory";
            }
            const long capacity = 1 << 10 << 10;

            var oldMessage = string.Empty;
            using (var mmf = MemoryMappedFile.CreateOrOpen(path, capacity))
            {
                var viewAccessor = mmf.CreateViewAccessor(0, capacity);

                //循环刷新共享内存字符串的值
                while (true)
                {
                    //读取字符长度
                    var strLength = viewAccessor.ReadInt32(0);
                    var charsInMMf = new char[strLength];
                    //读取字符
                    viewAccessor.ReadArray(4, charsInMMf, 0, strLength);
                    var sb = new StringBuilder();
                    sb.Append(charsInMMf);

                    //if (sb.ToString().Equals(oldMessage)) continue;
                    //oldMessage = sb.ToString();
                    Console.WriteLine("数据：" + sb);
                    Thread.Sleep(500);
                }
            }
        }
    }
}