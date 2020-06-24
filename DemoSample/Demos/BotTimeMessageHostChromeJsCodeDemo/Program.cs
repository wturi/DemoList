using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BotTimeMessageHostChromeJsCodeDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filePath = ReadAndJudgeVer("chrome", "1.0.0");

            Console.WriteLine(filePath);

            var dic = Unzip(filePath);

            Console.WriteLine(JsonConvert.SerializeObject(dic));

            Console.ReadLine();
        }

        /// <summary>
        /// 判断版本
        /// </summary>
        /// <param name="bowserType"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        private static string ReadAndJudgeVer(string bowserType, string ver)
        {
            //读取
            var filePath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\\ver";

            //判断版本库路径是否存在
            if (!Directory.Exists(filePath)) return string.Empty;

            //获取版本库中最新的代码
            var fileName = Directory.GetFiles(filePath).ToList()
                .Select(f => new FileInfo(f))
                .Where(f => f.Name.StartsWith(bowserType))
                .OrderByDescending(f => f.Name)
                .FirstOrDefault();

            var verInHost = fileName.Name.Replace(bowserType, string.Empty).Replace(".", string.Empty);
            var verInChrome = ver.Replace(".", string.Empty).PadRight(verInHost.Length, '0');

            var k2 = int.Parse(verInHost);
            var k = int.Parse(verInChrome);

            return k2 > k ? fileName.FullName : string.Empty;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="filePath"></param>
        private static Dictionary<string, string> Unzip(string filePath)
        {
            var dic = new Dictionary<string, string>();
            using (FileStream zipFileToOpen = new FileStream(filePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipFileToOpen, ZipArchiveMode.Read))
                {
                    foreach (var zipArciveEntry in archive.Entries)
                    {
                        dic.Add(zipArciveEntry.FullName, new StreamReader(zipArciveEntry.Open()).ReadToEnd());
                    }
                }
            }
            return dic;
        }
    }
}