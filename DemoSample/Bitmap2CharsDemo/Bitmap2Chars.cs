using Newtonsoft.Json;

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;

namespace Bitmap2CharsDemo
{
    internal class Bitmap2Chars
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
            GC.Collect();
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', '0', 'V', '1', 'i', ':', '*', '|', '.', ' ' };
            int bitmapH = bitmap.Height;
            int bitmapW = bitmap.Width;

            var tblDatas = new DataTable("Datas");

            DataColumn dc = null;
            dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
            dc.AutoIncrement = true;//自动增加
            dc.AutoIncrementSeed = 1;//起始为1
            dc.AutoIncrementStep = 1;//步长为1
            dc.AllowDBNull = false;//

            for (int i = 0; i < bitmapW / colSize; i++)
            {
                dc = tblDatas.Columns.Add("data" + i, Type.GetType("System.String"));
            }

            DataRow dataRow;

            for (int h = 0; h < bitmapH / rowSize; h++)
            {
                int offsetY = h * rowSize;
                dataRow = tblDatas.NewRow();
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
                    dataRow["data" + w] = charset[charset.Length - 1 - index];
                }
                result.Append("\r\n");
                tblDatas.Rows.Add(dataRow);
            }

            var ss = JsonConvert.SerializeObject(tblDatas);
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

        public static void BitmapConvert(Bitmap image, ref string[][] pixels1, ref string[] columnNames1, ref string temp)
        {
            var pixels = new string[image.Height][];
            var columnNames = new string[image.Width];

            Func<int, string> func = (colIndex) =>
            {
                int div = colIndex;
                string colLetter = String.Empty;
                while (div > 0)
                {
                    int mod = (div - 1) % 26;
                    colLetter = (char)(65 + mod) + colLetter;
                    div = (div - mod) / 26;
                }
                return colLetter;
            };

            for (int i = 0; i < image.Width; i++)
            {
                columnNames[i] = func(i + 1);
            }

            for (int i = 0; i < image.Height; i++)
            {
                pixels[i] = new string[image.Width];
                for (int j = 0; j < image.Width; j++)
                {
                    var c = image.GetPixel(j, i);
                    pixels[i][j] = string.Format("#{0:X6}", c.ToArgb() & 0x00FFFFFF);
                }
            }

            image.Dispose();

            pixels1 = pixels;
            columnNames1 = columnNames;

            StringBuilder sb = new StringBuilder();
            foreach (var row in pixels)
            {
                foreach (var item in row)
                {
                    sb.Append(item + " ");
                }
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
            }

            temp = sb.ToString();
        }
    }
}