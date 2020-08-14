using System;

using Newtonsoft.Json;

namespace JsonDemo
{
    /*
     * JSON使用ReferenceLoopHandling忽略其引用循环值
     */

    /*
     * 首先创建一个对象Employee
     * 序列化其对象,注意Employee对象中的属性Manager类型是其对象本身类型.当实例化多个对象时,并指定其属性值时,即循环取值(迭代),json1未指定其ReferenceLoopHandling忽略,抛出异常
     */

    public class Employee
    {
        public string Name { get; set; }
        public Employee Manager { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            //Demo1();

            Demo2();

            Console.ReadLine();
        }

        private static void Demo1()
        {
            var gongHui = new Employee { Name = "Gong Hui" };
            var jack = new Employee { Name = "Jack" };

            gongHui.Manager = jack;
            jack.Manager = jack;

            //string json1 = JsonConvert.SerializeObject(gongHui, Formatting.Indented);
            //Console.WriteLine(json1);//抛出异常

            var json2 = JsonConvert.SerializeObject(gongHui, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Console.WriteLine(json2);

            var json3 = JsonConvert.SerializeObject(jack, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Console.WriteLine(json3);
        }


        private static void Demo2()
        {

        }

    }
}

