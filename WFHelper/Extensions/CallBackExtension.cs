using System;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFHelper.Extensions
{
   public  static class CallBackExtension
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
    }
}
