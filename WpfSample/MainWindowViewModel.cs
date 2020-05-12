using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ClassBase;

namespace WpfSample
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region 公共函数

        private ConcurrentDictionary<string, string> _demoDictionary;

        public ConcurrentDictionary<string, string> DemoDictionary
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
            _demoDictionary = new ConcurrentDictionary<string, string>();
            _windowNameList = new List<string>();

            _demoDictionary.TryAdd("获取鼠标信息", "GetMouseInfo");
            _demoDictionary.TryAdd("设置鼠标状态", "SetMouseState");

            _demoDictionary.TryAdd("读取内存信息", "ReadSharedMemory");
            _demoDictionary.TryAdd("模拟键盘（WinRing0）", "WinRing0");
            _demoDictionary.TryAdd("获取进程信息", "GetProcessInfo");

            _windowNameList = _demoDictionary.Keys.ToList();
        }

        public void RunWindow(string windowName)
        {
            if (_demoDictionary.TryGetValue(windowName, out var value))
            {
                GetInvokeMethod<string>(value, value, "MainWindow", "Show", null);
            }
        }

        /// <summary>
        /// 调用方法实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        private static T GetInvokeMethod<T>(string assemblyName, string nameSpace, string className, string methodName, object[] paras)
        {
            try
            {
                //命名空间.类名,程序集
                var path = nameSpace + "." + className + "," + assemblyName;
                //加载类型
                var type = Type.GetType(path);
                //根据类型创建实例
                if (type == null) throw new Exception();
                var obj = Activator.CreateInstance(type, true);
                //加载方法参数类型及方法
                MethodInfo method;
                if (paras != null && paras.Length > 0)
                {
                    //加载方法参数类型
                    var paraTypes = new Type[paras.Length];
                    for (var i = 0; i < paras.Length; i++)
                    {
                        paraTypes[i] = paras[i].GetType();
                    }
                    //加载有参方法
                    method = type.GetMethod(methodName, paraTypes);
                }
                else
                {
                    //加载无参方法
                    method = type.GetMethod(methodName);
                }
                //类型转换并返回
                if (method == null) throw new Exception();
                return (T)method.Invoke(obj, paras);
            }
            catch
            {
                //发生异常时，返回类型的默认值。
                return default(T);
            }
        }
    }
}