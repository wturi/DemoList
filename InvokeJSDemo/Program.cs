using System;
using System.IO;

namespace InvokeJSDemo
{
    internal static class Program
    {
        private static void Main()
        {
            var jsPath = AppDomain.CurrentDomain.BaseDirectory + @"JSTestFiles\Test1.js";
            var str2 = File.ReadAllText(jsPath);

            var fun = $@"sayHello('{"张三"}')";
            var result = ExecuteScript(fun, str2);

            Console.WriteLine(result);
            Console.ReadLine();
        }

        private static string ExecuteScript(string sExpression, string sCode)
        {
            var scriptControl = new MSScriptControl.ScriptControl { UseSafeSubset = true, Language = "JScript" };
            scriptControl.AddCode(sCode);
            string str;
            try
            {
                str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
    }
}