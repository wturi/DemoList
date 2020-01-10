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
            long capacity = 1 << 10 << 10;


            string oldMessage = string.Empty;
            try
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen("BotTimeStudioMemory", capacity))
                {
                    MemoryMappedViewAccessor viewAccessor = mmf.CreateViewAccessor(0, capacity);

                    //循环刷新共享内存字符串的值
                    while (true)
                    {
                        //读取字符长度
                        int strLength = viewAccessor.ReadInt32(0);
                        char[] charsInMMf = new char[strLength];
                        //读取字符
                        viewAccessor.ReadArray(4, charsInMMf, 0, strLength);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(charsInMMf);

                        if (!sb.ToString().Equals(oldMessage) )
                        {
                            oldMessage = sb.ToString();
                            Console.WriteLine(sb.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}