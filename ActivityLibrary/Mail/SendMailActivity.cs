using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Net.Mail;

namespace ActivityLibrary.Mail
{

    [ToolboxBitmap(typeof(SendMailActivity), "../Icon/sendmail.png")]
    public sealed class SendMailActivity : NativeActivity
    {
        #region Input

        [RequiredArgument]
        [Description("发件人")]
        [DisplayName("发件人")]
        public InArgument<string> From { get; set; }

        [RequiredArgument]
        public InArgument<string> To { get; set; }

        [RequiredArgument]
        public InArgument<string> Subject { get; set; }

        [RequiredArgument]
        public string Body { get; set; }

        #endregion Input

        protected override void Execute(NativeActivityContext context)
        {
            using (var client = new SmtpClient())
            {
                var message = new MailMessage(From.Get(context), To.Get(context), Subject.Get(context),
                    Body)
                { IsBodyHtml = false };
                client.Send(message);
            }
        }
    }
}