using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SetMouseState
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isWhile = false;
        private int _whileNum = 20;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsWhile?.IsChecked != null && (bool)(IsWhile.IsChecked))
            {
                _isWhile = true;
            }
            SetMouseState();
        }

        private void Stop_OnClick(object sender, RoutedEventArgs e)
        {
            _isWhile = false;
        }

        private void SetMouseState()
        {
            _whileNum = 20;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                do
                {
                    this.Cursor = Cursors.Wait;
                    Thread.Sleep(10);
                    this.Cursor = Cursors.Arrow;
                    Thread.Sleep(50);
                    _whileNum--;
                } while (_isWhile && _whileNum > 0);
            }));
        }
    }
}