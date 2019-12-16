using System;
using System.Threading;

namespace WinRing0Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.Sleep(3000);
            Console.WriteLine("开始打印");

            string val = "!@#$abcd%^&*()_+>ABCD";

            WinRingHelper.init();
            foreach (char chr in val)
            {
                WinRingHelper.Send(chr);
            }

            Console.WriteLine("打印完成");
            Console.ReadLine();
        }
    }
}