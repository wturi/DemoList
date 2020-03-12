using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenFileDialogDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "C# Corner Open File Dialog",
                InitialDirectory = @"c:\",
                Filter = "All files（*.*）|*.*|All files(*.*)|*.* ",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            //@是取消转义字符的意思

           // FilterIndex 属性用于选择了何种文件类型,缺省设置为0,系统取Filter属性设置第一项
           //,相当于FilterIndex 属性设置为1.如果你编了3个文件类型，当FilterIndex ＝2时是指第2个.
           //  如果值为false，那么下一次选择文件的初始目录是上一次你选择的那个目录，
           //  不固定；如果值为true，每次打开这个对话框初始目录不随你的选择而改变，是固定的  
             
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName));

            }

            Console.ReadLine();
        }
    }
}
