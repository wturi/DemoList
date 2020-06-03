using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = Process.GetProcessesByName("Encoo.Executor").FirstOrDefault();

            var counter = new PerformanceCounter("Process", "Working Set - Private", p.ProcessName);

            var raw = counter.RawValue / 1024;
        }
    }
}
