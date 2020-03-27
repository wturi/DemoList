using System;
using System.Diagnostics;

using Microsoft.Win32;

namespace CheckSoftwareInstallationState
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var returnStr = CheckInstallState("chrome");
            const string arguments = " -new-window ";
            var process = OpenChromeBrowser(returnStr, arguments, "https://www.baidu.com", "chrome");

            if (process == null)
            {
                Console.WriteLine("open browser failure");
                Console.WriteLine(string.IsNullOrEmpty(returnStr) ? "没有找到" : returnStr);
            }
            Console.ReadLine();
        }

        private static string CheckInstallState(string browserName)
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
        /// 打开chrome浏览器
        /// </summary>
        /// <param name="installationPath"></param>
        /// <param name="arguments"></param>
        /// <param name="url"></param>
        /// <param name="browserName"></param>
        /// <returns></returns>
        private static Process OpenChromeBrowser(string installationPath, string arguments, string url, string browserName)
        {
            try
            {
                var pr = new Process
                {
                    StartInfo =
                    {
                        FileName =string.IsNullOrEmpty(installationPath)?browserName: $"{installationPath}.exe",
                        Arguments = arguments + url,
                        CreateNoWindow = true
                    }
                };
                pr.Start();
                pr.WaitForInputIdle(2000);
                var e = Process.GetProcessById(pr.Id);
                return e;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}