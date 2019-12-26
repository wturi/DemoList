using System;

namespace DynamicDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            dynamic foo = new DemoMethod();
            //可以直接调用方法
            foo.Run("dynamic demo run");

            //直接调用方法
            var msg = foo.Msg;

            Console.WriteLine(msg);
            Console.ReadLine();
        }
    }
}