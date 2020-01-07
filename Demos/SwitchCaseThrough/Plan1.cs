using System;

namespace SwitchCaseThrough
{
    internal class Plan1
    {
        private static string ActionInTable(Week week)
        {
            string[] methods = { "Cleaning", "CleanCloset", "Quarrel", "Shopping", "Temp", "Temp", "Temp" };
            return methods[(int)week];
        }

        public static void RunPlan()
        {
            SampleClass sample = new SampleClass();
            var addMethod = typeof(SampleClass).GetMethod(ActionInTable(Week.Monday));
            addMethod.Invoke(sample, null);
        }
    }

    internal class SampleClass
    {
        public void Cleaning()
        {
            Console.WriteLine("打扫");
        }

        public void CleanCloset()
        {
            Console.WriteLine("整理衣橱");
        }

        public void Quarrel()
        {
            Console.WriteLine("吵架");
        }

        public void Shopping()
        {
            Console.WriteLine("购物");
        }

        public void Temp()
        {
            Console.WriteLine("临时安排");
        }
    }

    internal enum Week
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
}