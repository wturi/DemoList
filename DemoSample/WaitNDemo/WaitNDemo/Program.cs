using System;

using WatiN.Core;

namespace WaitNDemo
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using (var ie = Browser.AttachTo<IE>(Find.ByTitle("百度一下，你就知"), 2400))
            {
                ie.TextField(Find.ById("kw")).Focus();

                var element = ie.ActiveElement;
            }


            Console.ReadLine();
        }
    }
}