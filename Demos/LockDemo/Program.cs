using System;
using System.Threading.Tasks;

namespace LockDemo
{
    internal class Program
    {
        private static Student s1 = new Student() { name = "Tim", age = 8 };

        private static void Main(string[] args)
        {
            for (var i = 0; i < 10; i++)
            {
                Task.Run(UpdateAge);
            }

            Console.ReadLine();
        }

        private static void UpdateAge()
        {
            lock (s1)
            {
                s1.age--;
                if (s1.age > 0)
                    Console.WriteLine($"s1.name:{s1.name}, s1.age: {s1.age}");
            }
        }
    }

    public class Student
    {
        public int age { get; set; }

        public string name { get; set; }
    }
}