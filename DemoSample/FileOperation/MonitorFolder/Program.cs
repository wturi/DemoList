using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace MonitorFolder
{
    internal class Program
    {
        private static List<string> _sourceDirectories = new List<string>();

        private static string _targetDirectory;

        private static int _intervalTime = 5000;

        private static void Main(string[] args)
        {
            _sourceDirectories = ConfigurationManager.AppSettings["SourceDirectories"]?.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            _targetDirectory = ConfigurationManager.AppSettings["TargetDirectory"];

            int.TryParse(ConfigurationManager.AppSettings["IntervalTime"], out _intervalTime);

            if (_sourceDirectories != null) 
                foreach (var sourceDirectory in _sourceDirectories)
                {
                    new Thread(() =>
                    {
                        while (true)
                        {
                            var startTime = DateTime.Now;
                            Console.WriteLine($"------{sourceDirectory} move start------");
                            CopyOldLabFilesToNewLab(sourceDirectory, _targetDirectory);
                            Console.WriteLine($"------{sourceDirectory} move finish , time spent(ms) : {(DateTime.Now - startTime).TotalMilliseconds}------");
                            Console.WriteLine();
                            Thread.Sleep(_intervalTime);
                        }
                    }).Start();
                }

            Console.ReadLine();
        }

        private static void CopyOldLabFilesToNewLab(string sourcePath, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            try
            {
                var labDirs = Directory.GetDirectories(sourcePath);//目录
                var labFiles = Directory.GetFiles(sourcePath);//文件
                if (labFiles.Length > 0)
                {
                    foreach (var t in labFiles)
                    {
                        File.Copy(sourcePath + "\\" + Path.GetFileName(t), savePath + "\\" + Path.GetFileName(t), true);
                        File.Delete(sourcePath + "\\" + Path.GetFileName(t));
                    }
                }

                if (labDirs.Length <= 0) return;

                foreach (var t in labDirs)
                {
                    Directory.GetDirectories(sourcePath + "\\" + Path.GetFileName(t));

                    //递归调用
                    CopyOldLabFilesToNewLab(sourcePath + "\\" + Path.GetFileName(t), savePath + "\\" + Path.GetFileName(t));
                }

            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}