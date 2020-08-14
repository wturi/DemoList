using System;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var onclick = "ContainerIno1(25429091,1003052,1003051,\"\",5)";

            var leftParenthesis = onclick.IndexOf("(", StringComparison.Ordinal) + 1;

            var comma1 = onclick.IndexOf(",", StringComparison.Ordinal);

            var comma2 = onclick.IndexOf(',', comma1 + 1);

            var comma3 = onclick.IndexOf(',', comma2 + 1);

            var cntId = onclick.Substring(leftParenthesis, comma1 - leftParenthesis);

            var terId = onclick.Substring(comma1 + 1, comma2 - comma1 - 1);

            var iId = onclick.Substring(comma2 + 1, comma3 - comma2 - 1);

            Console.WriteLine(cntId);

            Console.ReadLine();
        }
    }
}