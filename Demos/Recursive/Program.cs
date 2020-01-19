using System;

namespace Recursive
{
    /// <summary>
    /// 递归
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Add();
            Console.ReadLine();
        }

        private static void Add(int num = 0)
        {
            Console.WriteLine(num);
            if (num < 10) Add(++num);
        }
    }
}