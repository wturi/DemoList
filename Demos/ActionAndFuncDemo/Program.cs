using System;

namespace ActionAndFuncDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine("-------action demo-------");

            //var ArguemntAction = new Action<string, string>(ActionDemo.Argument);

            //ArguemntAction("哈哈哈哈", "哭哭哭哭");

            //Console.WriteLine("-------func demo-------");

            //var ArgumentFunc = new Func<int, int, int>(FuncDemo.Add);

            //Console.WriteLine("func demo add return num" + ArgumentFunc(1, 2));

            A.func d = new A.func(A.B);
            A.Call(d, 88);

            Console.ReadLine();
        }
    }

    public class ActionDemo
    {
        public static void Argument(string argumentName1, string argumentName2)
        {
            Console.WriteLine($"参数1是:{argumentName1},参数2是:{argumentName2}");
        }
    }

    public class FuncDemo
    {
        public static int Add(int num1, int num2)
        {
            return num1 + num2;
        }
    }

    public static class A
    {
        public delegate void func(int a);

        public static void B(int n)
        {
            Console.WriteLine(n);
        }

        public static void Call(func f, int k)
        {
            f(k);
        }
    }
}