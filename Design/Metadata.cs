using System.Activities.Presentation.Metadata;

using Design.Mail;

namespace Design
{
    public class Metadata : IRegisterMetadata
    {
        public static void RegisterAll()
        {
            var builder = new AttributeTableBuilder();
            SendMailActivityDesign.RegisterMetadata(builder);

            // TODO: 添加活动

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }

        public void Register()
        {
            RegisterAll();
        }
    }
}