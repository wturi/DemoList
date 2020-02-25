using System;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;

namespace FrmZhidao
{
    public struct POINT
    {
        private int x;
        private int y;
    }

    public static class APIMethod
    {
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);

        public static POINT GetCursorPos()
        {
            POINT p;
            if (GetCursorPos(out p))
            {
                return p;
            }
            throw new Exception();
        }

        public static IntPtr WindowFromPoint()
        {
            POINT p = GetCursorPos();
            return WindowFromPoint(p);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                var returnIntPrt = APIMethod.WindowFromPoint();

                Console.WriteLine(JsonConvert.SerializeObject(returnIntPrt));
                Thread.Sleep(1000);
            }
        }
    }
}