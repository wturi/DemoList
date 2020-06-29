using System.Windows;

namespace GetProcessInfo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainViewModel;
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            var pidStr = Pid.Text;

            if (!int.TryParse(pidStr, out var pid) && string.IsNullOrEmpty(PName.Text))
            {
                MessageBox.Show("请输入任意参数");
                return;
            }

            var processCommandEnum = ProcessCommandEnum.UnKnow;
            if (BrotherProcess.IsChecked != null && (bool)BrotherProcess.IsChecked)
            {
                processCommandEnum = ProcessCommandEnum.GetBrotherProcessInfo;
            }
            else if (ChildrenProcess.IsChecked != null && (bool)ChildrenProcess.IsChecked)
            {
                processCommandEnum = ProcessCommandEnum.GetChildrenProcessInfo;
            }
            else if (ParentProcess.IsChecked != null && (bool)ParentProcess.IsChecked)
            {
                processCommandEnum = ProcessCommandEnum.GetParentProcessInfo;
            }
            else if (KillProcess.IsChecked != null && (bool)KillProcess.IsChecked)
            {
                processCommandEnum = ProcessCommandEnum.KillProcess;
            }

            _mainViewModel.Run(processCommandEnum, PName.Text, pid);
        }
    }
}