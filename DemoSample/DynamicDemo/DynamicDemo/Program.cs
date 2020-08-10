using System;

using Newtonsoft.Json;

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
            var data = new
            {
                result = 1,
                response = new
                {
                    data = new
                    {
                        data = new
                        {
                            num = 1,
                        },
                        version = "2020",
                        result = 1
                    }
                },
            };

            var json = JsonConvert.SerializeObject(data);
            Console.WriteLine(json);

            var dynamic = JsonConvert.DeserializeObject<dynamic>(json);

            Console.WriteLine(dynamic.response.data.data.num);

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