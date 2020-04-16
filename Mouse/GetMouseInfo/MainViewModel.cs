using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Threading;
using System.Windows.Threading;
using ClassBase;

namespace GetMouseInfo
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private ObservableCollection<string> _info;

        public ObservableCollection<string> Info
        {
            get => _info;
            set
            {
                if (_info == value) return;
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        public MainViewModel()
        {
            _info = new ObservableCollection<string>();
            _timer.Interval = new TimeSpan(0, 0, 0, 0,500);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var hWnd = GetMousePointWindowsHandle();

            var msg = GetParentHandle((IntPtr)hWnd, $"鼠标指向句柄 : {hWnd}");

            Info.Add(msg);

            FocusLastItem();
        }

        public void Run()
        {
            _timer.Start();
        }

        private string GetParentHandle(IntPtr hWnd, string msg)
        {
            var parentHandle = GetParent(hWnd);
            if (((int)parentHandle) == 0)
            {
                msg += " 到顶了";
                return msg;
            }

            msg += $"  上层句柄:{parentHandle}";

            return GetParentHandle(parentHandle, msg);
        }

        /// <summary>
        /// 委托定义，用于控制界面元素
        /// </summary>
        public delegate void ScrollToEnd();
        public ScrollToEnd FocusLastItem = null;



        #region method

        private int GetMousePointWindowsHandle()
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

        /// <summary>
        /// 找到句柄
        /// </summary>
        /// <param name="p">指定位置坐标</param>
        /// <returns></returns>
        private int GetHandle(Point p)
        {
            return (int)WindowFromPoint(p);
        }

        //鼠标位置的坐标
        private Point GetCursorPosPoint()
        {
            _ = new Point();
            Point p;
            return GetCursorPos(out p) ? p : default;
        }

        #endregion method

        #region win32 method

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        private static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        #endregion win32 method
    }
}