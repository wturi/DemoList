using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication1
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.FindControl<Button>("btn1").Click += Btn1_Click;
            this.FindControl<Button>("btn2").Click += Btn2_Click;
            this.FindControl<Button>("btn3").Click += Btn3_Click;
        }
        private void Btn1_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var txt = this.FindControl<TextBox>("myText");
            txt.FontFamily = "微软雅黑";
            txt.Text = "LineZero 按钮1";
        }
        private void Btn2_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var txt = this.FindControl<TextBox>("myText");
            txt.FontFamily = "微软雅黑";
            txt.Text = "LineZero 按钮2";
        }
        private void Btn3_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var txt = this.FindControl<TextBox>("myText");
            txt.FontFamily = "微软雅黑";
            txt.Text = "LineZero 按钮3";
        }

    }
}
