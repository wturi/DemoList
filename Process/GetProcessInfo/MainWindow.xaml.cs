using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using Newtonsoft.Json;

namespace GetProcessInfo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _killProcessNum = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var pidStr = Pid.Text;




                if (!int.TryParse(pidStr, out var pid) && string.IsNullOrEmpty(PName.Text))
                {
                    MessageBox.Show("请输入任意参数");
                    return;
                }

                var process = Process.GetProcessById(pid);

                if (BrotherProcess.IsChecked != null && (bool)BrotherProcess.IsChecked)
                {
                    //兄弟进程
                    var parentProcess = Helpers.ProcessHelper.Parent(process, null);
                    var processList = Helpers.ProcessHelper.GetChildProcesses(parentProcess.Id);
                    var result = processList.Select(p => new
                    {
                        p.ProcessName,
                        ProcessId = p.Id
                    });

                    ShowTextBox.Text = JsonConvert.SerializeObject(result);
                }
                else if (ChildrenProcess.IsChecked != null && (bool)ChildrenProcess.IsChecked)
                {
                    //子进程
                    var processList = Helpers.ProcessHelper.GetChildProcesses(process.Id);
                    var result = processList.Select(p => new
                    {
                        p.ProcessName,
                        ProcessId = p.Id
                    });

                    ShowTextBox.Text = JsonConvert.SerializeObject(result);
                }
                else if (ParentProcess.IsChecked != null && (bool)ParentProcess.IsChecked)
                {
                    //父进程
                    var parentProcess = Helpers.ProcessHelper.Parent(process, null);
                    var result = new
                    {
                        parentProcess?.ProcessName,
                        ProcessId = parentProcess?.Id
                    };

                    ShowTextBox.Text = JsonConvert.SerializeObject(result);
                }
                else if (KillProcess.IsChecked != null && (bool)KillProcess.IsChecked)
                {
                    //杀死进程
                    KillProcessMethod();
                    ShowTextBox.Text = $"kill process number ：{_killProcessNum}";
                }
            }
            catch (Exception exception)
            {
                ShowTextBox.Text = JsonConvert.SerializeObject(exception);
            }
        }

        private void KillProcessMethod()
        {
            var pName = PName.Text;

            if (!string.IsNullOrEmpty(pName))
            {
                var ps = Process.GetProcessesByName(pName);
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

            if (!int.TryParse(Pid.Text, out var pId)) return;

            try
            {
                var process = Process.GetProcessById(pId);
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