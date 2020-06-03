using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {

                    var p = Process.GetProcessesByName("Encoo.Executor").FirstOrDefault();

                    var counter = new PerformanceCounter("Process", "Working Set - Private", p.ProcessName);

                    var raw = counter.RawValue / 1024;

                    using (var file = new System.IO.StreamWriter(@"C:\BVTResult\raw.txt", true))
                    {
                        file.WriteLine(raw.ToString());// 直接追加文件末尾，换行 
                    }

                    Console.WriteLine(raw);

                    Thread.Sleep(5000);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
