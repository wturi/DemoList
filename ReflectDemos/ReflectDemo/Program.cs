using System;
using System.Reflection;

namespace ReflectDemo
{
    /// <summary>
    /// C#反射遍历对象属性
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss-ffff"));
            AddressInfo model = new AddressInfo();
            ForeachClassProperties(model); 
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss-ffff"));
            Console.ReadLine();
        }

        private static void ForeachClassProperties<T>(T model)
        {
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                object value = item.GetValue(model, null);
            }
        }
    }

    public class AddressInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserTel { get; set; }
        public string Addressdetail { get; set; }
        public int IsMoren { get; set; }

        public AddressInfo()
        {
            Id = 1;
            UserName = "陈卧龙";
            UserTel = "1813707015*";
            Addressdetail = "江苏省苏州市工业园区国际科技园";
            IsMoren = 1;
        }
    }
}