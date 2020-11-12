using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.CustomEditor
{
    /// <summary>
    /// TableControl.xaml 的交互逻辑
    /// </summary>
    public partial class TableControl : UserControl
    {
        private DataTable _dt = new DataTable();

        public TableControl()
        {
            InitializeComponent();
            _dt.Columns.Add(new DataColumn("ParamKey", typeof(string)));
            _dt.Columns.Add(new DataColumn("ParamValue", typeof(string)));
            this.dgData.ItemsSource = null;
            this.dgData.ItemsSource = _dt.DefaultView;
        }

        #region 自定义依赖项属性

        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable DataSource
        {
            get => (DataTable)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        public bool IsEdit
        {
            get => (bool)GetValue(IsEditProperty);
            set => SetValue(IsEditProperty, value);
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(DataTable), typeof(TableControl), new PropertyMetadata(new DataTable(), DataSourceChanged));

        private static void DataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TableControl control = d as TableControl;
            if (e.NewValue == e.OldValue) return;
            var dt = e.NewValue as DataTable;
            if (control == null) return;
            control._dt = dt;
            control.dgData.ItemsSource = null;
            if (control._dt != null) control.dgData.ItemsSource = control._dt.DefaultView;
        }

        public static readonly DependencyProperty IsEditProperty =
            DependencyProperty.Register("IsEdit", typeof(bool), typeof(TableControl), new PropertyMetadata(true, IsEditChanged));

        private static void IsEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TableControl control = d as TableControl;
            bool isEdit = Convert.ToBoolean(e.NewValue);
            if (!isEdit)
            {
                int len = control.dgData.Columns.Count;
                control.dgData.Columns[len - 1].Visibility = Visibility.Collapsed;
                control.dgData.IsReadOnly = true;
                control.BtnAdd.Visibility = Visibility.Collapsed;
            }
        }

        #endregion 自定义依赖项属性

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            ((DataRowView)this.dgData.SelectedItem).Row.Delete();
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            _dt = ((DataView)this.dgData.ItemsSource).Table;
            DataRow dr = _dt.NewRow();
            _dt.Rows.Add(dr);
            this.dgData.ItemsSource = _dt.DefaultView;
        }
    }
}