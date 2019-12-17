using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap2CharsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var imagePath = @"D:\Project\Work\DemoList\Bitmap2CharsDemo\Images\View_081.jpg";
            var str = Bitmap2Chars.BitmapConvert(Bitmap2Chars.ReadImageFile(imagePath), 2, 2);
            File.WriteAllText("111.txt", str);
        }
    }
}
