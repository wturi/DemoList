using CefSharp;
using CefSharp.Wpf;

using System.Windows;

namespace CefSharpDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChromiumWebBrowser browser;

        public MainWindow()
        {
            InitializeComponent();
            browser = new ChromiumWebBrowser(Url.Text);
            browser.Title = "Simple Page";
            grid.Children.Add(browser);
        }

        private void jump_Click(object sender, RoutedEventArgs e)
        {
            browser = new ChromiumWebBrowser(Url.Text);
            browser.Title = "jump Page";
            grid.Children.Clear();
            grid.Children.Add(browser);
        }

        private void jsInvoke_Click(object sender, RoutedEventArgs e)
        {
            browser.ExecuteScriptAsync(JsStr.Text);
        }


    }
}