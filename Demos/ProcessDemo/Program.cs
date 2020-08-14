using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

using Microsoft.Win32;

using Newtonsoft.Json;

namespace ProcessDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int afterProcessIdList;

            var beforeProcessIdList = GetBrowserWindowHandleList("chrome");
            var beforeTime = DateTime.Now;

            Console.WriteLine($"now have chrome process ids : {JsonConvert.SerializeObject(beforeProcessIdList)} , time : {beforeTime.ToString(CultureInfo.CurrentCulture)}");

            OpenBrowserToJumpUrl("www.baidu.com", "", "chrome");

            var isFirst = true;
            do
            {
                if (!isFirst) Thread.Sleep(100);

                afterProcessIdList = GetBrowserWindowHandleList("chrome");
                var afterTime = DateTime.Now;
                Console.WriteLine($"now have chrome process ids : {JsonConvert.SerializeObject(afterProcessIdList)} , time : {(afterTime - beforeTime).Milliseconds}");

                isFirst = false;
            } while (afterProcessIdList == beforeProcessIdList);

            var latestProcessId = GetNewBrowserWindowHandle("chrome");

            Console.WriteLine($"the latest process id is : {latestProcessId}");

            Console.ReadLine();
        }

        private static List<int> GetProcessIdList(string processName)
        {
            var processIdList = Process.GetProcessesByName(processName).Select(p => p.Id).ToList();
            return processIdList;
        }

        /// <summary>
        /// 打开浏览器并跳转网址
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="arguments"></param>
        /// <param name="browserName"></param>
        /// <returns></returns>
        private static void OpenBrowserToJumpUrl(string url, string arguments, string browserName)
        {
            arguments += " -new-window ";
            var installPath = CheckBrowserInstallState(browserName)?.Trim();
            if (string.IsNullOrEmpty(installPath)) throw new Exception();
            OpenChromeBrowser(installPath, arguments, url, browserName);
        }

        /// <summary>
        /// 打开chrome浏览器
        /// </summary>
        /// <param name="installationPath"></param>
        /// <returns></returns>
        private static void OpenChromeBrowser(string installationPath, string arguments, string url, string browserName)
        {
            try
            {
                var pr = new Process
                {
                    StartInfo =
                    {
                        FileName = installationPath,
                        Arguments = arguments + url,
                        CreateNoWindow = true
                    }
                };
                pr.Start();
                pr.WaitForInputIdle(2000);
            }
            catch (Exception e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
            }
        }

        /// <summary>
        /// 检查chrome的安装状态
        /// </summary>
        /// <returns>如果有则返回安装路径,没有则返回null</returns>
        private static string CheckBrowserInstallState(string browserName)
        {
            const string softPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";
            var strKeyName = string.Empty;
            var localMachineRegKey = Registry.LocalMachine;
            var localMachineRegSubKey = localMachineRegKey.OpenSubKey($"{softPath}{browserName}.exe", false);

            var currentUserRegKey = Registry.CurrentUser;
            var currentUserRegSubKey = currentUserRegKey.OpenSubKey($"{softPath}{browserName}.exe", false);
            if (localMachineRegSubKey != null)
            {
                var objResult = localMachineRegSubKey.GetValue(strKeyName);
                var registryValueKind = localMachineRegSubKey.GetValueKind(strKeyName);
                if (registryValueKind == RegistryValueKind.String)
                {
                    return objResult.ToString();
                }
            }
            else if (currentUserRegSubKey != null)
            {
                var objResult = currentUserRegSubKey.GetValue(strKeyName);
                var registryValueKind = currentUserRegSubKey.GetValueKind(strKeyName);
                if (registryValueKind == RegistryValueKind.String)
                {
                    return objResult.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取新chrome window的句柄
        /// </summary>
        private static int GetNewBrowserWindowHandle(string browserType)
        {
            var processName = browserType;
            if (browserType == "Web360")
            {
                processName = "360se";
            }
            var process = (IList<int>)Process.GetProcessesByName(processName).Where(p => p.MainWindowHandle.ToInt32() > 0).Select(x => x.MainWindowHandle.ToInt32()).ToList();
            return process.Any() ? process[0] : 0;
        }

        private static int GetBrowserWindowHandleList(string browserType)
        {
            var processName = browserType;
            if (browserType == "Web360")
            {
                processName = "360se";
            }
            var process = Process.GetProcessesByName(processName).Where(p => p.MainWindowHandle.ToInt32() > 0).Select(x => x.MainWindowHandle.ToInt32()).ToList();
            return process.Any() ? process[0] : 0;
        }
    }
}