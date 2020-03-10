using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlEncodeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = System.Web.HttpUtility.UrlEncode("\r\n", Encoding.UTF8);
            Console.WriteLine(text);

            var data = System.Web.HttpUtility.UrlDecode(text, Encoding.UTF8);
            Console.WriteLine(data);

            Console.ReadLine();
        }
    }
}
