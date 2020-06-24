using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GetParentProcess
{
    internal class Program
    {
        private static List<Process> BeforeOpenBrowser { set; get; } = new List<Process>();

        private static void Main(string[] args)
        {
            GetNowProcessBeforeOpenBrowser("iexplore");

            //Parent(Process.GetCurrentProcess(), new[] { "cmd" });

            Console.ReadLine();
        }

        /// <summary>
        /// 获取打开浏览器之前的ie进程
        /// </summary>
        private static void GetNowProcessBeforeOpenBrowser(string processName)
        {
            var pp = Process.GetProcessesByName(processName);
            foreach (var process in pp)
            {
                if (process.ProcessName == processName)
                {
                    BeforeOpenBrowser.Add(process);
                }
            }
        }

        /// <summary>
        /// 获取进程的父进程
        /// </summary>
        /// <param name="process">进程</param>
        /// <param name="ignoreName">需要忽略的父进程名称，如果有则忽略这一层，继续网上找</param>
        /// <returns></returns>
        private static Process Parent(Process process, string[] ignoreName)
        {
            try
            {
                var parentProcess = FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
                if (parentProcess == null) return null;
                if (ignoreName == null) return parentProcess;
                return ignoreName.Contains(parentProcess.ProcessName) ? Parent(parentProcess, ignoreName) : parentProcess;
            }
            catch (Exception e)
            {
                //如果没有找到则返回null
                return null;
            }
        }

        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexedName = null;
            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexedName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexedName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexedName;
                }
            }
            return processIndexedName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }
    }
}