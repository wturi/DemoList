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
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                do
                {
                    this.Cursor = Cursors.Wait;
                    Thread.Sleep(50);
                    this.Cursor = Cursors.Arrow;
                    Thread.Sleep(200);
                } while (_isWhile);
            }));
        }
    }
}