using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WFHelper.XamlClr
{
    public class XamlClrReferences
    {
        #region 构造函数和析构函数

        public XamlClrReferences(string file)
        {
            this.References = new List<XamlClrRef>();
            var doc = XElement.Load(file);
            var query = from attr in doc.Attributes() where attr.IsNamespaceDeclaration && attr.Value.ToLower().StartsWith("clr-namespace") select attr.Value;
            foreach (var assemblyName in query)
            {
                this.References.Add(new XamlClrRef(assemblyName));
            }
        }

        #endregion Constructors and Destructors

        #region 属性

        public List<XamlClrRef> References { get; set; }

        #endregion Properties

        public XamlClrReferences Load()
        {
            foreach (var xamlClrRef in References)
            {
                xamlClrRef.Load();
            }

            return this;
        }

        internal static XamlClrReferences Load(string name)
        {
            return new XamlClrReferences(name).Load();
        }
    }
}