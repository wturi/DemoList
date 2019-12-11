using System.Windows;

using BindingDemo.ViewModel;

namespace BindingDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel = ModelFactory.Get(typeof(MainWindowViewModel), new object[] { }) as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;

            Update.UpdateVal();
        }
    }
}