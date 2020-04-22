using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SetMouseState
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isWhile = false;
        private int _whileNum = 50;
        private string _mouseState = string.Empty;

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
            _whileNum = 50;


            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                do
                {
                    switch (_mouseState)
                    {
                        case "忙碌":
                            this.Cursor = Cursors.Wait; break;
                        default:
                            this.Cursor = Cursors.Wait; break;
                    }
                    Thread.Sleep(10);
                    this.Cursor = Cursors.Arrow;
                    Thread.Sleep(50);
                    _whileNum--;
                } while (_isWhile && _whileNum > 0);
            }));
        }

        private void MouseState_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbi = (sender as ComboBox)?.SelectedItem;
            if (cbi != null)
            {
                _mouseState = cbi.ToString();
            }
        }
    }
}