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

            string str = "abc";

           Console.WriteLine( str[0]);

            //Add();
            Console.ReadLine();
        }

        private static void Add(int num = 0)
        {
            Console.WriteLine(num);
            if (num < 10) Add(++num);
        }
    }
}