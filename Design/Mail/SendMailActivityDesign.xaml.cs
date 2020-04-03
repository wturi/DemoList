using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.Drawing;

using ActivityLibrary.Mail;

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