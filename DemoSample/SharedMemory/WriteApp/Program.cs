using System;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace WriteApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            long capacity = 1 << 10 << 10;
            //创建或者打开共享内存
            using (var mmf = MemoryMappedFile.CreateOrOpen("BotTimeNativeMessageHostSharedMemory", capacity, MemoryMappedFileAccess.ReadWrite))
            {
                //通过MemoryMappedFile的CreateViewAccssor方法获得共享内存的访问器
                var viewAccessor = mmf.CreateViewAccessor(0, capacity);
                //循环写入，使在这个进程中可以向共享内存中写入不同的字符串值
                while (true)
                {
                    Console.WriteLine("请输入一行要写入共享内存的文字：");

                    string input = Console.ReadLine();

                    //向共享内存开始位置写入字符串的长度
                    viewAccessor.Write(0, input.Length);

                    //向共享内存4位置写入字符
                    viewAccessor.WriteArray<char>(4, input.ToArray(), 0, input.Length);
                }
            }
        }
    }
}