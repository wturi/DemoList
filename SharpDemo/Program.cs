using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

using Microsoft.CSharp;

namespace SharpDemo
{
    internal class Program
    {
        [Obsolete]
        private static void Main(string[] args)
        {
            // 1.CSharpCodePrivoder
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            // 2.ICodeComplier
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            // 3.CompilerParameters
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;

            // 4.CompilerResults
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode());

            if (cr.Errors.HasErrors)
            {
                Console.WriteLine("编译错误：");
                foreach (CompilerError err in cr.Errors)
                {
                    Console.WriteLine(err.ErrorText);
                }
            }
            else
            {
                // 通过反射，调用HelloWorld的实例
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");

                Console.WriteLine(objMI.Invoke(objHelloWorld, null));
            }

            Console.ReadLine();
        }

        private static string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("      public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append("      {");
            sb.Append(Environment.NewLine);
            sb.Append("          public string OutPut()");
            sb.Append(Environment.NewLine);
            sb.Append("          {");
            sb.Append(Environment.NewLine);
            sb.Append("               return \"Hello world!\";");
            sb.Append(Environment.NewLine);
            sb.Append("          }");
            sb.Append(Environment.NewLine);
            sb.Append("      }");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            string code = sb.ToString();
            Console.WriteLine(code);
            Console.WriteLine();

            return code;
        }
    }
}