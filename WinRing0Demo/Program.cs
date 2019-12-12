using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinRing0Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(3000);
            Console.WriteLine("开始打印");
            //foreach (var k in (WinRingHelper.Key[])Enum.GetValues(typeof(WinRingHelper.Key)))
            //{
            //    WinRingHelper.init();
            //    Console.WriteLine($"key={k.ToString()}");
            //    WinRingHelper.KeyDown(k);
            //    Thread.CurrentThread.Join(100);
            //    WinRingHelper.KeyUp(k);
            //    Thread.CurrentThread.Join(100);
            //}


            // WinRingHelper.init();
            // WinRingHelper.KeyDown(WinRingHelper.Key.KC_CONTROL_Shift);
            // WinRingHelper.KeyDown(WinRingHelper.Key.KC_N);
            // Thread.CurrentThread.Join(100);
            //WinRingHelper.KeyUp(WinRingHelper.Key.KC_CONTROL_Shift);
            // WinRingHelper.KeyUp(WinRingHelper.Key.KC_N);


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
