﻿using System;

namespace BindingDemo.ViewModel
{
    public class Window1ViewModel : ViewModelBase
    {
        private string _name = "张三";

        public string Name
        {
            get => _name;
            set
            {
                if (value != null && _name == value) return;
                _name = value;
            }
        }

        public object IsRunningProcess => throw new NotImplementedException();
    }
}