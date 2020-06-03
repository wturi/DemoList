using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            long firstNum = 0;

            long lastNum = 0;

            while (true)
            {
                try
                {
                    var p = Process.GetProcessesByName("Encoo.Executor").FirstOrDefault();

                    var counter = new PerformanceCounter("Process", "Working Set - Private", p.ProcessName);

                    var raw = counter.RawValue / 1024;

                    if (firstNum == 0)
                    {
                        firstNum = raw;
                    }

                    Console.WriteLine($"{raw}   {raw - firstNum}   {raw - lastNum}");

                    lastNum = raw;

                    using (var file = new System.IO.StreamWriter(@"C:\BVTResult\raw.txt", true))
                    {
                        file.WriteLine(raw.ToString());// 直接追加文件末尾，换行
                    }

                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}