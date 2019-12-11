using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BindingDemo.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isRunningProcess = false;
        public bool IsRunningProcess
        {
            get => _isRunningProcess;
            set
            {
                if (_isRunningProcess == value) return;
                _isRunningProcess = value;
                RaisePropertyChanged("IsRunningProcess");
            }
        }

       
    }
}
