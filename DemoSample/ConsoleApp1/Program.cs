using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var processName = "";

            Console.WriteLine("输入process name(默认为：EncooNativeMessageHost)：");
            processName = Console.ReadLine();

            if (string.IsNullOrEmpty(processName))
            {
                processName = "EncooNativeMessageHost";
            }

            while (true)
            {
                try
                {
                    var ps = Process.GetProcessesByName(processName);
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  process num :{ps.Length}");
                    Thread.Sleep(200);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}