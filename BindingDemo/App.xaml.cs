using System.Collections.Generic;
using System.Windows;

namespace BindingDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.Properties[nameof(ModelFactory)] = new Dictionary<string, object>();

            base.OnStartup(e);
        }
    }
}