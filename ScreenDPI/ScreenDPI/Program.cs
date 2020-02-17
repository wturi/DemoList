using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ScreenDPI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Process[] vProcesses = Process.GetProcessesByName("notepad");

            var screenList = Screen.AllScreens;

            using (Graphics graphics = Graphics.FromHwnd(vProcesses.FirstOrDefault().MainWindowHandle))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
            }
        }
    }
}