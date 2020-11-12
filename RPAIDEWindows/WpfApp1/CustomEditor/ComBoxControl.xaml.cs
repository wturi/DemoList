using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.CustomEditor
{
    /// <summary>
    /// ComBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class ComBoxControl : UserControl
    {
        public ComBoxControl()
        {
            InitializeComponent();
        }

        public List<string> listKey;

        public delegate void comboxHeand(object sender, EventArgs e);

        public event comboxHeand UserControlComBoxClick;

        public ObservableCollection<DataInfo> ItemsSource
        {
            get { return (ObservableCollection<DataInfo>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
                SetText();
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ComBoxControl), new FrameworkPropertyMetadata("", TextPropertyChangedCallback));
        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(ComBoxControl), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ComBoxControl), new FrameworkPropertyMetadata(null, ItemsSourcePropertyChangedCallback));

        private static void ItemsSourcePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var multioleCheckbox = dependencyObject as ComBoxControl;
            if (multioleCheckbox == null) return;
            multioleCheckbox.CheckableCombo.ItemsSource = multioleCheckbox.ItemsSource;
        }
        private static void TextPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var multioleCheckbox = dependencyObject as ComBoxControl;
            if (multioleCheckbox == null) return;
            else
            {
                string newVal = dependencyPropertyChangedEventArgs.NewValue as string;
                if (newVal == null)
                    return;
              
            }
        }
        private void CheckableCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null) return;
            SetText();
            if (UserControlComBoxClick != null)
                UserControlComBoxClick(sender, new EventArgs());
        }

        private void SetText()
        {
            StringBuilder sb = new StringBuilder();
            listKey = new List<string>();
            foreach (var item in ItemsSource)
            {
                if (item.IsSelect)
                {
                    if (item.Name.Contains(",") || item.Name.Contains("，") || item.Name.Contains("\""))
                    {
                        sb.Append("\"");
                        sb.Append(item.Name.Replace("\"", "\"\""));
                        sb.Append("\",");
                    }
                    else
                    {
                        sb.Append(item.Name);
                        sb.Append(",");
                    }
                    listKey.Add(item.Key);
                }
            }
            Text = string.IsNullOrEmpty(sb.ToString()) ? DefaultText : sb.ToString().TrimEnd(new[] { ',' });
        }
    }

    public class DataInfo : INotifyPropertyChanged
    {
        private string key;

        public string Key
        {
            get => key;
            set { key = value; NotifyPropertyChanged("IsSelect"); }
        }

        private string name;

        public string Name
        {
            get => name;
            set { name = value; NotifyPropertyChanged("IsSelect"); }
        }

        private bool _isSelect;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsSelect
        {
            get { return _isSelect; }
            set { _isSelect = value; NotifyPropertyChanged("IsSelect"); }
        }
    }
}