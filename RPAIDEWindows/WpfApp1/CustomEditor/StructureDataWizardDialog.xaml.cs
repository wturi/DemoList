using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.CustomEditor
{
    /// <summary>
    /// StructureDataWizardDialog.xaml 的交互逻辑
    /// </summary>
    public partial class StructureDataWizardDialog
    {
        /// <summary>
        /// 条件窗体的宽度
        /// </summary>
        private const double ConditionsWindowWidth = 366;

        /// <summary>
        /// 条件窗体的高度
        /// </summary>
        private const double ConditionsWindowHeight = 184;

        /// <summary>
        /// 数据展示页面宽度
        /// </summary>
        private const double DataShowWindowWidth = 516;

        /// <summary>
        /// 数据展示页面高度
        /// </summary>
        private const double DataShowWindowHeight = 590;

        /// <summary>
        /// 页面导航
        /// </summary>
        private readonly Stack<StructureDataWizardDialogPage> _pageLink = new Stack<StructureDataWizardDialogPage>();

        public StructureDataWizardDialog()
        {
            Width = ConditionsWindowWidth;
            Height = ConditionsWindowHeight;
            InitializeComponent();
        }

        /// <summary>
        /// 页面切换
        /// </summary>
        /// <param name="currentPage">当前页面</param>
        /// <param name="targetPage">目标页面</param>
        private void PageSwitch(StructureDataWizardDialogPage currentPage, StructureDataWizardDialogPage targetPage)
        {
            new List<Grid>
            {
                DataShow,
                IsGetTableData,
                FirstElement,
                SecondElement,
                ToSpecify,
                MoreData
            }.ForEach(p => p.Visibility = Visibility.Hidden);

            switch (targetPage)
            {
                case StructureDataWizardDialogPage.DataShow:
                    DataShow.Visibility = Visibility.Visible;
                    Width = DataShowWindowWidth;
                    Height = DataShowWindowHeight;
                    break;

                case StructureDataWizardDialogPage.IsGetTableData:
                    IsGetTableData.Visibility = Visibility.Visible;
                    Width = ConditionsWindowWidth;
                    Height = ConditionsWindowHeight;
                    break;

                case StructureDataWizardDialogPage.FirstElement:
                    FirstElement.Visibility = Visibility.Visible;
                    Width = ConditionsWindowWidth;
                    Height = ConditionsWindowHeight;
                    break;

                case StructureDataWizardDialogPage.SecondElement:
                    SecondElement.Visibility = Visibility.Visible;
                    Width = ConditionsWindowWidth;
                    Height = ConditionsWindowHeight;
                    break;

                case StructureDataWizardDialogPage.ToSpecify:
                    ToSpecify.Visibility = Visibility.Visible;
                    Width = ConditionsWindowWidth;
                    Height = ConditionsWindowHeight;
                    break;

                case StructureDataWizardDialogPage.MoreData:
                    MoreData.Visibility = Visibility.Visible;
                    Width = 406;
                    Height = 245;
                    break;
            }
            if (!_pageLink.Contains(targetPage))
                _pageLink.Push(currentPage);
        }

        /// <summary>
        /// 获取更多相关数据 跳转到第一个元素页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetMoreData_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.MoreData, StructureDataWizardDialogPage.FirstElement);
        }

        /// <summary>
        /// 数据展示页面 后退 跳转到上一步页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataShowBack_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.DataShow, _pageLink.Peek());
            _pageLink.Pop();
        }

        /// <summary>
        /// 数据展示页面 完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataShowComplete_OnClick(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// 是否获取整表全部数据取消按钮 跳转到第二元素页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsGetTableDataCancel_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.IsGetTableData, StructureDataWizardDialogPage.SecondElement);
        }

        /// <summary>
        /// 是否获取整表全部数据导入按钮 跳转到数据展示页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsGetTableDataImport_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.IsGetTableData, StructureDataWizardDialogPage.DataShow);
        }

        /// <summary>
        /// 第一个元素页面 下一步按钮 跳转到第二元素页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstElementNext_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.FirstElement, StructureDataWizardDialogPage.SecondElement);
        }

        /// <summary>
        /// 第二个元素页面 后退按钮 跳转到上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondElementBack_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.SecondElement, _pageLink.Peek());
            _pageLink.Pop();
        }

        /// <summary>
        /// 第二个元素页面 下一步按钮 跳转到数据展示页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondElementNext_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.SecondElement, StructureDataWizardDialogPage.MoreData);
        }

        /// <summary>
        /// 重新指定页面 确定按钮 跳转到上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToSpecifySure_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.ToSpecify, _pageLink.Peek());
            _pageLink.Pop();
        }

        /// <summary>
        /// 更多数据页面 后退按钮 跳转到上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreDataBack_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.MoreData, _pageLink.Peek());
            _pageLink.Pop();
        }

        /// <summary>
        /// 更多数据页面 下一步按钮 跳转到数据展示页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreDataNext_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.MoreData, StructureDataWizardDialogPage.DataShow);
        }

        /// <summary>
        /// 更多数据页面 新增按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreDataAdd_OnClick(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 无法识别第二个元素,返回到第二个元素页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnableToIdentifySure_OnClick(object sender, RoutedEventArgs e)
        {
            PageSwitch(StructureDataWizardDialogPage.UnableToIdentify, _pageLink.Peek());
            _pageLink.Pop();
        }
    }

    public enum StructureDataWizardDialogPage
    {
        /// <summary>
        /// 数据展示页面
        /// </summary>
        DataShow,

        /// <summary>
        /// 是否获取整表数据页面
        /// </summary>
        IsGetTableData,

        /// <summary>
        /// 第一个元素页面
        /// </summary>
        FirstElement,

        /// <summary>
        /// 第二个元素页面
        /// </summary>
        SecondElement,

        /// <summary>
        /// 重新指定页面
        /// </summary>
        ToSpecify,

        /// <summary>
        /// 获取更多页面
        /// </summary>
        MoreData,
        /// <summary>
        /// 无法识别第二个元素
        /// </summary>
        UnableToIdentify
    }
}