using System.Windows;

namespace WpfApp1.AttachedProperties
{
    public class MoreProps
    {
        public static readonly DependencyProperty MarginRightProperty = DependencyProperty.RegisterAttached(
            "MarginRight",
            typeof(string),
            typeof(MoreProps),
            new UIPropertyMetadata(OnMarginRightPropertyChanged));

        public static string GetMarginRight(FrameworkElement element)
        {
            return (string)element.GetValue(MarginRightProperty);
        }

        public static void SetMarginRight(FrameworkElement element, string value)
        {
            element.SetValue(MarginRightProperty, value);
        }

        private static void OnMarginRightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is FrameworkElement element)) return;

            if (!int.TryParse((string)args.NewValue, out var value)) return;

            var margin = element.Margin;
            margin.Right = value;
            element.Margin = margin;
        }
    }
}