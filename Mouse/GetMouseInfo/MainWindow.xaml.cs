using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GetMouseInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainViewModel;
            _mainViewModel.FocusLastItem += AutoScroll;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { _mainViewModel.Run(); });
        }

        /// <summary>
        /// 滚动条自动滚动
        /// </summary>
        private void AutoScroll()
        {
            try
            {
                var decorator = (Decorator)VisualTreeHelper.GetChild(MouseInfo, 0);
                var scrollViewer = (ScrollViewer)decorator.Child;
                scrollViewer.ScrollToEnd();
            }
            catch (Exception)
            {
            }
        }
    }
}