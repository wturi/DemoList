using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.View;
using System.Text;

namespace WFHelper.Extensions
{
    public static class CallBackExtension
    {
        public static void SelectionChanged(Selection selection)
        {
            var modelItem = selection.PrimarySelection;
            var sb = new StringBuilder();

            while (modelItem != null)
            {
                var displayName = modelItem.Properties["DisplayName"];

                if (displayName != null)
                {
                    if (sb.Length > 0)
                        sb.Insert(0, " - ");
                    sb.Insert(0, displayName.ComputedValue);
                }
                modelItem = modelItem.Parent;
            }
        }

        public static void SelectionChanged(AssemblyContextControlItem assemblyContext)
        {
            var nameSpace = assemblyContext.AllAssemblyNamesInContext;
            while (nameSpace != null)
            {
                var name = string.Join(";", nameSpace);
            }
        }
    }
}