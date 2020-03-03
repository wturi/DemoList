using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
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
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(out Point pt);
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        public static extern IntPtr WindowFromPoint(Point pt);


        //鼠标位置的坐标
        public static Point GetCursorPosPoint()
        {
            Point p = new Point();
            if (GetCursorPos(out p))
            {
                return p;
            }
            return default(Point);
        }


        /// </summary>
        /// 找到句柄
        /// <param name="p">指定位置坐标</param>
        /// <returns></returns>
        public static int GetHandle(Point p)
        {
            return (int)WindowFromPoint(p);
        }

        /// <summary>
        /// 得到鼠标指向的窗口句柄
        /// </summary>
        /// <returns>找不到则返回-1</returns>
        public static int GetMoustPointWindwsHwnd()
        {
            try
            {
                return GetHandle(GetCursorPosPoint());
            }
            catch (System.Exception ex)
            {

            }
            return -1;
        }



        private static void Main(string[] args)
        {
            while (true)
            {
                int Hwnd = GetMoustPointWindwsHwnd();

                Console.WriteLine($"鼠标指向句柄 : {Hwnd}");


                Thread.Sleep(1000);
            }
        }
    }
}