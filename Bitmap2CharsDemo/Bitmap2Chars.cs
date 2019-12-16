using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap2CharsDemo
{
    class Bitmap2Chars
    {
        /// <summary>
        /// 通过计算rowSize*colSize区域的亮度平均值用一个字符替代
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rowSize"></param>
        /// <param name="colSize"></param>
        /// <returns></returns>
        public static String BitmapConvert(Bitmap bitmap, int rowSize, int colSize)
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', '0', 'V', '1', 'i', ':', '*', '|', '.', ' ' };
            int bitmapH = bitmap.Height;
            int bitmapW = bitmap.Width;
            for (int h = 0; h < bitmapH / rowSize; h++)
            {
                int offsetY = h * rowSize;
                for (int w = 0; w < bitmapW / colSize; w++)
                {
                    int offsetX = w * colSize;
                    float averBright = 0;
                    for (int j = 0; j < rowSize; j++)
                    {
                        for (int i = 0; i < colSize; i++)
                        {
                            try
                            {
                                Color color = bitmap.GetPixel(offsetX + i, offsetY + j);
                                averBright += color.GetBrightness();
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                averBright += 0;
                            }
                        }
                    }
                    averBright /= (rowSize * colSize);
                    int index = (int)(averBright * charset.Length);
                    if (index == charset.Length)
                        index--;
                    result.Append(charset[charset.Length - 1 - index]);
                }
                result.Append("\r\n");
            }
            return result.ToString();
        }


        /// <summary>
        /// 读取本地图片
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;//文件不存在
            }
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }
    }
}
