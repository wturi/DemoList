using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallDemo
{
    class Program
    {

        public static string TypeName = "System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        static void Main(string[] args)
        {
            var list = TypeName.Split('`');
            var type = list[1].MidStrEx("[[", "]]");
            var dd = type.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).Select(d => d.Split(',')[0]).StringJoin<string>(",");

            var ddd = $"{list[0]}<{dd}>";
        }
    }
}
