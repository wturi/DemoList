using ActivityLibrary.Mail;
using System;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Design.Mail
{
    // SendMailActivityDesign.xaml 的交互逻辑
    public partial class SendMailActivityDesign
    {
        public SendMailActivityDesign()
        {
            InitializeComponent();
        }


        public static void RegisterMetadata(AttributeTableBuilder builder)
        {
            builder.AddCustomAttributes(
                typeof(SendMailActivity),
                new DesignerAttribute(typeof(SendMailActivityDesign)),
                new DescriptionAttribute("Send Mail"),
                new ToolboxBitmapAttribute(typeof(SendMailActivity), "../Icon/sendmail.png"));
        }
    }
}
