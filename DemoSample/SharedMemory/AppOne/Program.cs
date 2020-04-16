using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AppOne
{
    internal class Program
    {
        /// <summary>
        /// 无效句柄
        /// </summary>
        private const int INVALID_HANDLE_VALUE = -1;

        /// <summary>
        /// 页面读写
        /// </summary>
        private const int PAGE_PEADWRITE = 0x04;

        /// <summary>
        /// 显示单个窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>
        /// 设置窗口句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 共享内存
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="lpAttributes"></param>
        /// <param name="flProtect"></param>
        /// <param name="dwMaximumSizeHigh"></param>
        /// <param name="dwMaximumSizeLow"></param>
        /// <param name="lpName"></param>
        /// <returns></returns>

        [DllImport("Kernel32.dll", EntryPoint = "CreateFileMapping", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile, //HANDLE hFile,
         UInt32 lpAttributes,//LPSECURITY_ATTRIBUTES lpAttributes,  //0
         UInt32 flProtect,//DWORD flProtect
         UInt32 dwMaximumSizeHigh,//DWORD dwMaximumSizeHigh,
         UInt32 dwMaximumSizeLow,//DWORD dwMaximumSizeLow,
         string lpName//LPCTSTR lpName
         );

        [DllImport("Kernel32.dll", EntryPoint = "OpenFileMapping")]
        private static extern IntPtr OpenFileMapping(
         UInt32 dwDesiredAccess,//DWORD dwDesiredAccess,
         int bInheritHandle,//BOOL bInheritHandle,
         string lpName//LPCTSTR lpName
         );

        private const int FILE_MAP_ALL_ACCESS = 0x0002;

        private const int FILE_MAP_WRITE = 0x0002;

        [DllImport("Kernel32.dll", EntryPoint = "MapViewOfFile")]
        private static extern IntPtr MapViewOfFile(
         IntPtr hFileMappingObject,//HANDLE hFileMappingObject,
         UInt32 dwDesiredAccess,//DWORD dwDesiredAccess
         UInt32 dwFileOffsetHight,//DWORD dwFileOffsetHigh,
         UInt32 dwFileOffsetLow,//DWORD dwFileOffsetLow,
         UInt32 dwNumberOfBytesToMap//SIZE_T dwNumberOfBytesToMap
         );

        [DllImport("Kernel32.dll", EntryPoint = "UnmapViewOfFile")]
        private static extern int UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("Kernel32.dll", EntryPoint = "CloseHandle")]
        private static extern int CloseHandle(IntPtr hObject);

        private static Semaphore m_Write;  //可写的信号
        private static Semaphore m_Read;  //可读的信号
        private static IntPtr handle;     //文件句柄
        private static IntPtr addr;       //共享内存地址
        private static uint mapLength;            //共享内存长

        /// <summary>
        /// 线程用来读取数据
        /// </summary>
        private static Thread threadRed;

        private static void Main(string[] args)
        {
            init();
        }

        // 初始化共享内存数据 创建一个共享内存

        private static void init()

        {
            m_Write = new Semaphore(1, 1, "WriteMap");//开始的时候有一个可以写

            m_Read = new Semaphore(0, 1, "ReadMap");//没有数据可读

            mapLength = 1024;

            IntPtr hFile = new IntPtr(INVALID_HANDLE_VALUE);

            handle = CreateFileMapping(hFile, 0, PAGE_READWRITE, 0, mapLength, "shareMemory");

            addr = MapViewOfFile(handle, FILE_MAP_ALL_ACCESS, 0, 0, 0);

            //handle = OpenFileMapping(0x0002, 0, "shareMemory");

            //addr = MapViewOfFile(handle, FILE_MAP_ALL_ACCESS, 0, 0, 0);

            threadRed = new Thread(new ThreadStart(ThreadReceive));

            threadRed.Start();
        }

        // 线程启动从共享内存中获取数据信息

        private static  void ThreadReceive()

        {
            myDelegate myI = new myDelegate(changeTxt);

            while (true)

            {
                try

                {
                    //m_Write = Semaphore.OpenExisting("WriteMap");

                    //m_Read = Semaphore.OpenExisting("ReadMap");

                    //handle = OpenFileMapping(FILE_MAP_WRITE, 0, "shareMemory");

                    //读取共享内存中的数据：

                    //是否有数据写过来

                    m_Read.WaitOne();

                    //IntPtr m_Sender = MapViewOfFile(handle, FILE_MAP_ALL_ACCESS, 0, 0, 0);

                    byte[] byteStr = new byte[100];

                    byteCopy(byteStr, addr);

                    string str = Encoding.Default.GetString(byteStr, 0, byteStr.Length);

                    /////调用数据处理方法 处理读取到的数据

                    m_Write.Release();
                }
                catch (WaitHandleCannotBeOpenedException)

                {
                    continue;

                    //Thread.Sleep(0);
                }
            }

            //不安全的代码在项目生成的选项中选中允许不安全代码

            unsafe void byteCopy(byte[] dst, IntPtr src)
            {
                fixed (byte* pDst = dst)
                {
                    byte* pdst = pDst;
                    byte* psrc = (byte*)src;
                    while ((*pdst++ = *psrc++) != '\0')
                        ;
                }
            }
        }
    }
}