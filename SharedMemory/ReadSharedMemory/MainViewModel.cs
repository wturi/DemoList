using System;
using System.Collections.ObjectModel;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Windows.Threading;

using ClassBase;

namespace ReadSharedMemory
{
    public class MainViewModel : ViewModelBase
    {
        private const long Capacity = 1 << 10 << 10;
        private MemoryMappedFile _mmf;

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private ObservableCollection<string> _readSharedMemoryInfo;

        public ObservableCollection<string> ReadSharedMemoryInfo
        {
            get => _readSharedMemoryInfo;
            set
            {
                if (_readSharedMemoryInfo == value) return;
                _readSharedMemoryInfo = value;
                RaisePropertyChanged("ReadSharedMemoryInfo");
            }
        }

        public MainViewModel()
        {
            _readSharedMemoryInfo = new ObservableCollection<string>();
        }

        public void Run(string path)
        {
            _mmf = MemoryMappedFile.CreateOrOpen(path, Capacity);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var viewAccessor = _mmf.CreateViewAccessor(0, Capacity);
            //读取字符长度
            var strLength = viewAccessor.ReadInt32(0);
            var charsInMMf = new char[strLength];
            //读取字符
            viewAccessor.ReadArray(4, charsInMMf, 0, strLength);
            var sb = new StringBuilder();
            sb.Append(charsInMMf);
            ReadSharedMemoryInfo.Add("数据：" + sb);
            FocusLastItem();
        }

        /// <summary>
        /// 委托定义，用于控制界面元素
        /// </summary>
        public delegate void ScrollToEnd();

        public ScrollToEnd FocusLastItem = null;
    }
}