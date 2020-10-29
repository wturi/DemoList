using System;

namespace ActiveIETab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dd = 1;
            Console.WriteLine(dd);
            var timer = new System.Timers.Timer
            {
                Interval = 5000
            };

            timer.Elapsed += (obj, e) =>
            {
                dd = 2;
                timer.Stop();
            };

            timer.Start();

            Console.WriteLine(dd);

            Console.ReadLine();
        }

        //private static void Demo()
        //{
        //    var url = "https://encootest.chinacloudsites.cn/";

        //    ShellWindows shellWindows = new SHDocVw.ShellWindows();

        //    string filename;

        //    foreach (SHDocVw.InternetExplorer ie in shellWindows)
        //    {
        //        filename = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

        //        if (filename.Equals("iexplore") && ie.LocationURL.Contains(url))

        //        {
        //            new TabActivator((IntPtr)ie.HWND).ActivateByTabsUrl(ie.LocationURL);

        //            break;
        //        }
        //    }
        //}
    }
}