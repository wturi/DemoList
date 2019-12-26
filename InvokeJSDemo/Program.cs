using System;
using System.Collections.Generic;
using System.IO;
using MSScriptControl;
using Newtonsoft.Json;

namespace InvokeJSDemo
{
    internal static class Program
    {
        private static void Main()
        {
            var myScriptEngine = new RunScriptHelper.ScriptEngine(RunScriptHelper.ScriptLanguage.JavaScript);

            try
            {


                //var ss = JsonConvert.SerializeObject(new List<string> {"李四","王五" });

                //var jsMethodRunValue = myScriptEngine.Run("add", new object[] { "张三", ss, }, jsStrText);
                //Console.WriteLine(jsMethodRunValue.ToString());

                JsRun();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }




        public static void JsRun()
        {
            ScriptControlClass sc = new ScriptControlClass();
            sc.UseSafeSubset = true;
            sc.Language = "JavaScript";

            var jsStrText = @"function add(name){
nameList.Push(one);
    return one;
}";

            var ddd = new object[] { "张三", "李四" };
            sc.AddCode(jsStrText);
            sc.AddObject("nameList", ddd, false);

            string str = sc.Run("add", new object[] { "dsds" }).ToString();
        }

    }
}