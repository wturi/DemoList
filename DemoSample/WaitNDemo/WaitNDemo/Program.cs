using System;

using WatiN.Core;
using WatiN.Core.Native.Chrome;

namespace WaitNDemo
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var ie = Browser.AttachTo<IE>(Find.ByTitle("百度一下，你就知"), 2400);
            ie.Element(Find.ById("kw")).Focus();

            var element = ie.ActiveElement;

            Console.ReadLine();
        }
    }
}