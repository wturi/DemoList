using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReadSharedMemory
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

        /// <summary>
        /// 滚动条自动滚动
        /// </summary>
        private void AutoScroll()
        {
            try
            {
                var decorator = (Decorator)VisualTreeHelper.GetChild(SharedMemoryInfo, 0);
                var scrollViewer = (ScrollViewer)decorator.Child;
                scrollViewer.ScrollToEnd();
            }
            catch (Exception)
            {
            }
        }

        private void Btn_OnClick(object sender, RoutedEventArgs e)
        {
            var path = SharedMemoryName.Text;
            Task.Run(() => { _mainViewModel.Run(path); });

        }
    }
}
