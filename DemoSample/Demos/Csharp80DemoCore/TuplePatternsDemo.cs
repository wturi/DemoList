using System;

namespace Csharp80DemoCore
{
    public static class TuplePatternsDemo
    {
        public static void Run()
        {
            Console.WriteLine(Foo("lin", "dexi"));
            Console.WriteLine(Foo("逗比", "dexi"));
            Console.WriteLine(Foo(null, null));
        }

        private static string Foo(string first, string second)
        {
            var str = (first, second) switch
            {
                ("lin", "dexi") => "林德熙是逗比",
                (_, "dexi") => "没错，这就是逗比",
                (_, _) => "不认识",
            };

            return str;
        }

        private static string Foo(Point point)
        {
            var (x, y) = point;
            return $"{x},{y}";
        }
    }

    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);

        public void Deconstruct(out int x, out int y) =>
            (x, y) = (X, Y);
    }
}