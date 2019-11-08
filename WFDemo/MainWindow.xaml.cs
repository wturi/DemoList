using System.Windows;
using WFHelper.ExtensionService;
using WFHelper.ViewModels;

namespace WFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly WdViewModel _wdViewModel = new WdViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _wdViewModel;
            this.Closing += _wdViewModel.ViewClosing;
            this.Closed += _wdViewModel.ViewClosed;

        }

        #region 方法

        private void TabItemGotFocusRefreshXamlBox(object sender, RoutedEventArgs e)
        {
            _wdViewModel.NotifyChanged("XAML");
        }

        #endregion 方法
    }
}