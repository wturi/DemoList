using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using ClassBase;

using Newtonsoft.Json;

namespace GetProcessInfo
{
    public class MainViewModel : ViewModelBase
    {
        #region 参数

        private int _killProcessNum = 0;

        private ObservableCollection<string> _processInfoList;

        public ObservableCollection<string> ProcessInfoList
        {
            get => _processInfoList;
            set
            {
                if (_processInfoList == value) return;
                _processInfoList = value;
                RaisePropertyChanged("ProcessInfoList");
            }
        }

        private string _processInfo;

        public string ProcessInfo
        {
            get => _processInfo;
            set
            {
                if (_processInfo == value) return;
                _processInfo = value;
                RaisePropertyChanged("ProcessInfo");
            }
        }

        #endregion 参数

        public MainViewModel()
        {
            _processInfoList = new ObservableCollection<string>();
            _processInfo = string.Empty;
        }

        public void Run(ProcessCommandEnum processCommandEnum, string processName, int processId)
        {
            _processInfo = string.Empty;
            try
            {
                switch (processCommandEnum)
                {
                    case ProcessCommandEnum.GetBrotherProcessInfo:
                        ProcessInfo = BrotherProcessInfo(processId);
                        break;

                    case ProcessCommandEnum.GetChildrenProcessInfo:
                        ProcessInfo = ChildrenProcessInfo(processId);
                        break;

                    case ProcessCommandEnum.GetParentProcessInfo:
                        ProcessInfo = ParentProcessInfo(processId);
                        break;

                    case ProcessCommandEnum.KillProcess:
                        ProcessInfo = KillProcessResult(processName, processId);
                        break;

                    default: break;
                }
            }
            catch (Exception e)
            {
                ProcessInfo = JsonConvert.SerializeObject(e);
            }
        }

        /// <summary>
        /// 兄弟进程
        /// </summary>
        /// <returns></returns>
        private string BrotherProcessInfo(int processId)
        {
            var process = Process.GetProcessById(processId);
            var parentProcess = Helpers.ProcessHelper.Parent(process, null);
            var processList = Helpers.ProcessHelper.GetChildProcesses(parentProcess.Id);
            var result = processList.Select(p => new
            {
                p.ProcessName,
                ProcessId = p.Id
            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 子进程
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        private string ChildrenProcessInfo(int processId)
        {
            var processList = Helpers.ProcessHelper.GetChildProcesses(processId);
            var result = processList.Select(p => new
            {
                p.ProcessName,
                ProcessId = p.Id
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 父进程
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        private string ParentProcessInfo(int processId)
        {
            var process = Process.GetProcessById(processId);
            var parentProcess = Helpers.ProcessHelper.Parent(process, null);
            var result = new
            {
                parentProcess?.ProcessName,
                ProcessId = parentProcess?.Id
            };

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 杀死进程
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        private string KillProcessResult(string processName, int processId)
        {
            _killProcessNum = 0;
            KillProcessMethod(processName, processId);
            return $"kill process number ：{_killProcessNum}";
        }

        private void KillProcessMethod(string processName, int processId)
        {
            if (!string.IsNullOrEmpty(processName))
            {
                var ps = Process.GetProcessesByName(processName);
                foreach (var p in ps)
                {
                    try
                    {
                        KillProcessAndChildren(p);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            try
            {
                var process = Process.GetProcessById(processId);
                KillProcessAndChildren(process);
            }
            catch
            {
                // ignored
            }
        }

        private void KillProcessAndChildren(Process process)
        {
            var children = Helpers.ProcessHelper.GetChildProcesses(process.Id);
            if (children.Any())
            {
                children.ForEach(KillProcessAndChildren);
            }

            _killProcessNum++;
            process.Kill();
        }
    }
}