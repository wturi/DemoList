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

using WpfApp1.CustomEditor;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            var dataDialog = new StructureDataWizardDialog();
            dataDialog.Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataDialog = new StructureDataWizardDialog();
            dataDialog.Show();
        }
    }
}
