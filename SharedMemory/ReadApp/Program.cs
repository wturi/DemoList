using System;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace ReadApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            long capacity = 1 << 10 << 10;

            using (var mmf = MemoryMappedFile.OpenExisting("BotTimeNativeMessageHostSharedMemory"))
            {
                MemoryMappedViewAccessor viewAccessor = mmf.CreateViewAccessor(0, capacity);

                //循环刷新共享内存字符串的值
                while (true)
                {
                    //读取字符长度
                    int strLength = viewAccessor.ReadInt32(0);
                    char[] charsInMMf = new char[strLength];
                    //读取字符
                    viewAccessor.ReadArray<char>(4, charsInMMf, 0, strLength);
                    Console.Clear();
                    Console.Write(charsInMMf);
                    Console.Write("\r");
                    Thread.Sleep(200);
                }
            }
        }
    }
}