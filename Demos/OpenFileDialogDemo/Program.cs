using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenFileDialogDemo
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            // var openFileDialog = new OpenFileDialog
            // {
            //     Title = "C# Corner Open File Dialog",
            //     InitialDirectory = @"c:\",
            //     Filter = "All files（*.*）|*.*|All files(*.*)|*.* ",
            //     FilterIndex = 1,
            //     RestoreDirectory = true
            // };
            // //@是取消转义字符的意思

            //// FilterIndex 属性用于选择了何种文件类型,缺省设置为0,系统取Filter属性设置第一项
            ////,相当于FilterIndex 属性设置为1.如果你编了3个文件类型，当FilterIndex ＝2时是指第2个.
            ////  如果值为false，那么下一次选择文件的初始目录是上一次你选择的那个目录，
            ////  不固定；如果值为true，每次打开这个对话框初始目录不随你的选择而改变，是固定的

            // if (openFileDialog.ShowDialog() == DialogResult.OK)
            // {
            //     Console.WriteLine(System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName));

            // }

            //var Azmount = "1";

            //Azmount = Azmount.Substring(0, (Azmount.IndexOf(".") + 3) < Azmount.Length ? (Azmount.IndexOf(".") + 3) : Azmount.Length);

            //if (Azmount.IndexOf(".") >= 0)
            //{
            //    var poor = Azmount.Length - (Azmount.IndexOf(".") + 1);
            //   for (var i = 0; i < poor; i++)
            //   {
            //       Azmount += "0";
            //   }
            //}
            //else
            //{
            //    Azmount += ".00";
            //}

            ////Console.WriteLine(Azmount);

            //var str =
            //    "{\"nodeHierarchyInfo\":[{\"customId\":\"2|0|39\",\"isPresentInSelector\":1,\"selectorInfo\":{\"index\":0,\"attributes\":{\"tag\":\"A\",\"ancestorid\":\"1\",\"sinfo\":\"WPF中制作无边框窗体_祝紫山(大可山人)博客[GDI+,WPF..._CSDN博客\"}},\"otherAttributes\":{}}],\"retCode\":1,\"returnId\":73009839002501,\"codeVersion\":1027}";

            ////var listStr = RecvMsgProcess(str);

            //var n = "222";

            //Console.WriteLine("12\"121" + n + "22");


            while (true)
            {
                var point = GetCursorPosPoint();
                Console.WriteLine($"x:{point.X},y:{point.Y}");
                Thread.Sleep(1000);
            }




            Console.ReadLine();
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(out Point pt);


        //鼠标位置的坐标
        public static Point GetCursorPosPoint()
        {
            return GetCursorPos(out var p) ? p : default;
        }

        private static List<string> RecvMsgProcess(string message)
        {
            List<string> msgs = new List<string>();
            if (string.IsNullOrEmpty(message)) return msgs;
            bool inJsonMessage = false;
            int start = 0, end = 0;
            Stack<Char> jsonSynbolStack = new Stack<char>();
            for (var i = 0; i < message.Length; i++)
            {
                var c = message[i];
                // in the message from browser, sometimes there are some string before json content include [
                if (c == '[' && !inJsonMessage) continue;
                if (c == '{' || c == '[')
                {
                    if (!inJsonMessage)
                    {
                        start = i;
                        inJsonMessage = true;
                    }
                    jsonSynbolStack.Push(c);
                }
                else if (!inJsonMessage) continue;
                else if (c == '}' || c == ']')
                {
                    if (jsonSynbolStack.Count > 0)
                    {
                        var lastSymbol = jsonSynbolStack.Pop();
                        if ((c == '}' && lastSymbol == '{') || (c == ']' && lastSymbol == '['))
                        {
                            if (jsonSynbolStack.Count == 0)
                            {
                                end = i;
                                msgs.Add(message.Substring(start, end - start + 1));
                                inJsonMessage = false;
                            }
                        }
                        else // invalid json format
                        {
                            inJsonMessage = false;
                            jsonSynbolStack.Clear();
                        }
                    }
                }
            }
            return msgs;
        }
    }
}