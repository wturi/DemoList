using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

using ClassBase;

namespace WpfDemoSample
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

            _demoDictionary.TryAdd("获取鼠标坐标", nameof(GetMouseInfo));
            _windowNameList.Add("获取鼠标坐标");
        }


        public void RunWindow(string windowName)
        {
            if (_demoDictionary.TryGetValue(windowName, out var value))
            {
                GetInvokeMethod<string>("GetMouseInfo", "GetMouseInfo", "MainWindow", "Show", null);
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
        private T GetInvokeMethod<T>(string assemblyName, string nameSpace, string className, string methodName, object[] paras)
        {
            try
            {
                //命名空间.类名,程序集
                string path = nameSpace + "." + className + "," + assemblyName;
                //加载类型
                Type type = Type.GetType(path);
                //根据类型创建实例
                object obj = Activator.CreateInstance(type, true);
                //加载方法参数类型及方法
                MethodInfo method = null;
                if (paras != null && paras.Length > 0)
                {
                    //加载方法参数类型
                    Type[] paratypes = new Type[paras.Length];
                    for (int i = 0; i < paras.Length; i++)
                    {
                        paratypes[i] = paras[i].GetType();
                    }
                    //加载有参方法
                    method = type.GetMethod(methodName, paratypes);
                }
                else
                {
                    //加载无参方法
                    method = type.GetMethod(methodName);
                }
                //类型转换并返回
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