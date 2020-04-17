using System.Windows;
using System.Windows.Controls;

namespace WpfDemoSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _windowName;

        private readonly MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            this.DataContext = _mainWindowViewModel;
            InitializeComponent();
            MessageTextBlock.Text = "必要环境：.net core 3.1\r\n";
            MessageTextBlock.Text += "下载地址：https://dotnet.microsoft.com/download/dotnet-core/3.1";
        }

        /// <summary>
        /// 启动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.RunWindow(_windowName);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbi = (sender as ComboBox)?.SelectedItem;
            if (cbi != null)
            {
                _windowName = cbi.ToString();
            }
        }
    }
}