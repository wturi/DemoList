using System.Threading;
using System.Windows;
using System.Windows.Controls;

using WinRing0.Helpers;

namespace WinRing0
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ShowTextBox.Text = string.Empty;

            var inputText = TextBox.Text;
            var millisecondText = Millisecond.Text;
            var millisecond = 1000;

            if (string.IsNullOrEmpty(inputText))
            {
                ShowTextBox.Text = "请输入键盘内容";
                return;
            }

            //是否跟随鼠标焦点
            if (MouseFocus?.IsChecked != null && !(bool)(MouseFocus.IsChecked))
            {
                ShowTextBox.Focus();
            }

            //延迟运行间隔
            if (!string.IsNullOrEmpty(millisecondText))
            {
                int.TryParse(Millisecond.Text, out millisecond);
            }

            Thread.Sleep(millisecond);

            RunWinRing0(inputText);
        }

        private static void RunWinRing0(string inputText)
        {
            WinRingHelper.init();

            foreach (var value in inputText)
            {
                WinRingHelper.Send(value);
            }
        }
    }
}