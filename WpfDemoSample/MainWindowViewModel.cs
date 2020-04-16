using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;

using ClassBase;

namespace WpfDemoSample
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region 公共函数

        private ConcurrentDictionary<string, Window> _demoDictionary;

        public ConcurrentDictionary<string, Window> DemoDictionary
        {
            get => _demoDictionary;
            set
            {
                if (_demoDictionary == value) return;
                _demoDictionary = value;
                RaisePropertyChanged(nameof(DemoDictionary));
            }
        }

        private List<string> _windowNameList;

        public List<string> WindowNameList
        {
            get => _windowNameList;
            set
            {
                if (_windowNameList == value) return;
                _windowNameList = value;
                RaisePropertyChanged(nameof(WindowNameList));
            }
        }

        #endregion 公共函数

        public MainWindowViewModel()
        {
            _demoDictionary = new ConcurrentDictionary<string, Window>();
            _windowNameList = new List<string>();

            _demoDictionary.TryAdd("获取鼠标坐标", new Window());
            _windowNameList.Add("获取鼠标坐标");
        }
    }
}