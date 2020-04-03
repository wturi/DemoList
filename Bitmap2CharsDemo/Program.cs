using System.IO;

namespace Bitmap2CharsDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var imagePath = @"D:\Project\Work\DemoList\Bitmap2CharsDemo\Images\View_081.jpg";
            var str = Bitmap2Chars.BitmapConvert(Bitmap2Chars.ReadImageFile(imagePath), 2, 2);
            File.WriteAllText("111.txt", str);
        }
    }
}