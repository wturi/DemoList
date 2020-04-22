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
            var resultStr = string.Empty;

            var pidStr = Pid.Text;

            if (!int.TryParse(pidStr, out var pid))
            {
                MessageBox.Show("请输入Pid");
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
            else
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
        }
    }
}