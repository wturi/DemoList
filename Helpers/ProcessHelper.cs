using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Helpers
{
    public static class ProcessHelper
    {
        /// <summary>
        /// 获取进程的父进程
        /// </summary>
        /// <param name="process">进程</param>
        /// <param name="ignoreName">需要忽略的父进程名称，如果有则忽略这一层，继续往上找</param>
        /// <returns></returns>
        public static Process Parent(Process process, string[] ignoreName)
        {
            try
            {
                var parentProcess = FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
                if (parentProcess == null) return null;
                if (ignoreName == null) return parentProcess;
                return ignoreName.Contains(parentProcess.ProcessName) ? Parent(parentProcess, ignoreName) : parentProcess;
            }
            catch (Exception)
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

        #region 获取子进程

        /// <summary>
        /// 获取子进程
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<Process> GetChildProcesses(int parentId)
        {
            var result = new List<Process>();

            var query = "Select * From Win32_Process Where ParentProcessId = " + parentId;
            var searcher = new ManagementObjectSearcher(query);
            var processList = searcher.Get();

            foreach (var process in processList)
            {
                result.Add(Process.GetProcessById(Convert.ToInt32(process.GetPropertyValue("ProcessId"))));
            }

            return result;
        }

        #endregion 获取子进程
    }
}