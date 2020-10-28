using System;

using SHDocVw;

namespace ActiveIETab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var url = "https://encootest.chinacloudsites.cn/";

            ShellWindows shellWindows = new SHDocVw.ShellWindows();

            string filename;

            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

                if (filename.Equals("iexplore") && ie.LocationURL.Contains(url))

                {
                    new TabActivator((IntPtr)ie.HWND).ActivateByTabsUrl(ie.LocationURL);

                    break;
                }
            }
        }
    }
}