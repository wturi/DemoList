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
                        KillProcessAndChildren(p.Id);
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
                KillProcessAndChildren(pId);
            }
            catch
            {
                // ignored
            }
        }

        private void KillProcessAndChildren(int pId)
        {
            var children = Helpers.ProcessHelper.GetChildProcesses(pId);
            foreach (var child in children)
            {
                child.Kill();
            }

            Process.GetProcessById(pId)?.Kill();
        }
    }
}