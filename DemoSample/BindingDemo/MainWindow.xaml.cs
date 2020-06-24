using BindingDemo.ViewModel;

using System.Windows;

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

            var window1 = new Window1();
            window1.Show();

            Update.UpdateVal();
        }
    }
}