using System;

using Newtonsoft.Json;

namespace DynamicDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var returnObj = GetDynamicClassBydt();
            Console.WriteLine(JsonConvert.SerializeObject(returnObj));

            Console.ReadLine();
        }

        private static void Run1()
        {
            dynamic foo = new DemoMethod();
            //可以直接调用方法
            foo.Run("dynamic demo run");

            //直接调用方法
            var msg = foo.Msg;

            Console.WriteLine(msg);
            Console.ReadLine();
        }

        /// <summary>
        /// 使用dynamic根据DataTable的列名自动添加属性并赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Object GetDynamicClassBydt()
        {
            dynamic d = new System.Dynamic.ExpandoObject();
            (d as System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>).Add(new System.Collections.Generic.KeyValuePair<string, object>("numA", 1));

            return d;
        }
    }
}