using Newtonsoft.Json;

using System;

namespace DynamicDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var returnObj = GetDynamicClassByDataTable();
            //Console.WriteLine(JsonConvert.SerializeObject(returnObj));

            Run1();


            Console.ReadLine();
        }

        private static void Run1()
        {
            dynamic foo = new DemoMethod();
            //可以直接调用方法
            foo.Run("dynamic demo run");

            dynamic msg = null;

            try
            {
                //直接调用方法
                msg = foo.Msg;
            }
            catch (Exception e)
            {
                msg = foo.Msg;
            }

            Console.WriteLine(msg);
            Console.ReadLine();
        }

        /// <summary>
        /// 使用dynamic根据DataTable的列名自动添加属性并赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Object GetDynamicClassByDataTable()
        {
            dynamic d = new System.Dynamic.ExpandoObject();
            (d as System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>).Add(new System.Collections.Generic.KeyValuePair<string, object>("numA", 1));

            return d;
        }
    }
}