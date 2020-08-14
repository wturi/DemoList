using System.Windows;

using BindingDemo.ViewModel;

namespace BindingDemo
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public MainWindowViewModel ViewModel = ModelFactory.Get(typeof(MainWindowViewModel), new object[] { }) as MainWindowViewModel;

        public Window1()
        {
            InitializeComponent();

            DataContext = new Window1ViewModel();

            Name.DataContext = ViewModel;
        }
    }
}