using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WebBrowerDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string szTmp = "https://www.baidu.com";
            Uri uri = new Uri(szTmp);
            CamWeb.Navigate(uri);
        }

        private void CamWeb_LoadCompleted(object sender, NavigationEventArgs e)
        {

            mshtml.HTMLDocument dom = (mshtml.HTMLDocument)CamWeb.Document; //定义HTML
            //dom.documentElement.style.overflow = "hidden"; //隐藏浏览器的滚动条
            //dom.body.setAttribute("scroll", "no"); //禁用浏览器的滚动条
            //if (!dom.body.innerHTML.Contains("123456"))
            //{
            //    string szTmp = "https://www.baidu.com";
            //    Uri uri = new Uri(szTmp);
            //    CamWeb.Navigate(uri);
            //}
        }
    }
}
