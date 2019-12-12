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
            foreach (var k in (WinRingHelper.Key[])Enum.GetValues(typeof(WinRingHelper.Key)))
            {
                WinRingHelper.init();
                Console.WriteLine($"key={k.ToString()}");
                WinRingHelper.KeyDown(k);
                Thread.CurrentThread.Join(100);
                WinRingHelper.KeyUp(k);
                Thread.CurrentThread.Join(100);
            }


            WinRingHelper.init();
            //WinRingHelper.KeyDown(WinRingHelper.Key.VK_SHIFT);
            WinRingHelper.KeyDown(WinRingHelper.Key.);
            Thread.CurrentThread.Join(100);
            WinRingHelper.KeyUp(WinRingHelper.Key.VK_NUM1);
            //WinRingHelper.KeyUp(WinRingHelper.Key.VK_SHIFT);


            //string val = "*/-+.";

            //foreach (char chr in val)
            //{
            //    WinRingHelper.init();
            //    if (chr >= 'A' && chr <= 'Z')
            //        WinRingHelper.KeyDown(WinRingHelper.Key.VK_SHIFT);
            //    WinRingHelper.KeyDown(chr);
            //    Thread.CurrentThread.Join(100);
            //    WinRingHelper.KeyUp(chr);
            //    if (chr >= 'A' && chr <= 'Z')
            //        WinRingHelper.KeyUp(WinRingHelper.Key.VK_SHIFT);
            //    Thread.CurrentThread.Join(100);
            //}


            Console.ReadLine();
        }
    }
}
