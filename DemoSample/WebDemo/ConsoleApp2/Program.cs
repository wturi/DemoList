using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mshtml;

using SHDocVw;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
          ActiveIeTab("https://www.baidu.com/?tn=80035161_1_dg");
            Console.ReadLine();

        }

        private static void ActiveIeTab(string url)
        {
            ShellWindows shellWindows = new SHDocVw.ShellWindows();

            string filename;

            foreach (SHDocVw.InternetExplorer ie in shellWindows)

            {

                filename = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

                if (!filename.Equals("iexplore") || !ie.LocationURL.Contains(url)) continue;

                new TabActivator((IntPtr)ie.HWND).ActivateByTabsUrl(ie.LocationURL);

                break;

            }
        }
    }
}
