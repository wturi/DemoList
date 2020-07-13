using System;

namespace Csharp80DemoCore
{
    public static class RangeDemo
    {
        public static void Run()
        {
            var foo = new[] { "1", "2", "3" };

            foreach (var s in foo[0..1])
            {
                Console.WriteLine(s);
            }
        }
    }
}