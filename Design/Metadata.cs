using System;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
