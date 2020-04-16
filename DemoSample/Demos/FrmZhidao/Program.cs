using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

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

        private static POINT GetCursorPos()
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
            var p = GetCursorPos();
            return WindowFromPoint(p);
        }
    }

    internal class Program
    {
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        private static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        //鼠标位置的坐标
        private static Point GetCursorPosPoint()
        {
            var p = new Point();
            return GetCursorPos(out p) ? p : default(Point);
        }

        /// <summary>
        /// 找到句柄
        /// </summary>
        /// <param name="p">指定位置坐标</param>
        /// <returns></returns>
        private static int GetHandle(Point p)
        {
            return (int)WindowFromPoint(p);
        }

        /// <summary>
        /// 得到鼠标指向的窗口句柄
        /// </summary>
        /// <returns>找不到则返回-1</returns>
        private static int GetMousePointWindowsHandle()
        {
            try
            {
                return GetHandle(GetCursorPosPoint());
            }
            catch (Exception)
            {
                // ignored
            }

            return -1;
        }

        private static void Main(string[] args)
        {

            var dd = Assembly.Load("System");

            //while (true)
            //{
            //    var hWnd = GetMousePointWindowsHandle();
            //    Console.Write($"鼠标指向句柄 : {hWnd}");

            //    GetParentHandle((IntPtr)hWnd);

            //    Console.WriteLine();

            //    Thread.Sleep(1000);
            //}
        }

        private static void GetParentHandle(IntPtr hWnd)
        {
            var parentHandle = GetParent((IntPtr)hWnd);
            if (((int)parentHandle) == 0)
            {
                Console.WriteLine(" 到顶了 ");
                return;
            }

            Console.Write($"  上层句柄:{parentHandle}");

            GetParentHandle(parentHandle);
        }
    }
}