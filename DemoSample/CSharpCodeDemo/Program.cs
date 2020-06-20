using Microsoft.CSharp;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpCodeDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string code = @"
                            using System;
                            using System.Collections.Generic;

                            namespace Dynamic1
                            {
                                public class Test
                                {
                                    private static List<string> CSharpCodeLogOutputStr = new List<string>();

                                     public static void BotTimeWriteLine(string message)
                                    {
                                        CSharpCodeLogOutputStr.Add(message);
                                    }

                                    public static void Test1(ref List<string> outStr)
                                    {
                                       for(int i=0;i<=5;i++)
                                        {
                                           BotTimeWriteLine(i.ToString());
                                        }
                                        outStr=CSharpCodeLogOutputStr;
                                    }
                                }
                            }
                        ";
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("Dynamic1.Test");
            MethodInfo main = program.GetMethod("Test1");
            var arg = new object[] { new List<string>(), };

            main.Invoke(null, arg);

            foreach (var o in arg)
            {
                (o as List<string>).ForEach(Console.WriteLine);
            }

            Console.ReadLine();
        }
    }
}