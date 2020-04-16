using System;
using System.Text;

namespace UrlEncodeDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var text = System.Web.HttpUtility.UrlEncode("\r\n", Encoding.UTF8);
            Console.WriteLine(text);

            var data = System.Web.HttpUtility.UrlDecode(text, Encoding.UTF8);
            Console.WriteLine(data);

            Console.ReadLine();
        }
    }
}