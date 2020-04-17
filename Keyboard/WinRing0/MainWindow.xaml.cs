using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            if (string.IsNullOrEmpty(inputText))
            {
                ShowTextBox.Text = "请输入键盘内容";
                return;
            }

            ShowTextBox.Focus();

            Thread.Sleep(1000);

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
