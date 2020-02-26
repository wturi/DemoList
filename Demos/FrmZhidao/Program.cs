using System;
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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]

        [StructLayout(LayoutKind.Sequential)]//定义与API相兼容结构体，实际上是一种内存转换
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]//获取鼠标坐标
        public static extern int GetCursorPos(
            ref POINTAPI lpPoint
        );

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        public static extern int WindowFromPoint(
            int xPoint,
            int yPoint
        );

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            int hWnd,
            StringBuilder lpString,
            int nMaxCount
        );

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            int hWnd,
            StringBuilder lpString,
            int nMaxCont
        );



        private static void Main(string[] args)
        {
            while (true)
            {
                POINTAPI point = new POINTAPI();//必须用与之相兼容的结构体，类也可以
                                                //add some wait time
               
                GetCursorPos(ref point);//获取当前鼠标坐标

                int hwnd = WindowFromPoint(point.X, point.Y);//获取指定坐标处窗口的句柄
                StringBuilder name = new StringBuilder(256);
                GetWindowText(hwnd, name, 256);
                GetClassName(hwnd, name, 256);


                Console.WriteLine(hwnd);

                Thread.Sleep(1000);
            }
        }
    }
}