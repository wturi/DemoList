﻿using System;

namespace SimpleFactoryDemo
{
    /// <summary>
    /// 简单工厂模式
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 客户想点一个西红柿炒蛋
            Food food1 = FoodSimpleFactory.CreateFood("西红柿炒蛋");
            food1.Print();

            // 客户想点一个土豆肉丝
            Food food2 = FoodSimpleFactory.CreateFood("土豆肉丝");
            food2.Print();

            Console.Read();
        }
    }

    /// <summary>
    /// 菜抽象类
    /// </summary>
    public abstract class Food
    {
        // 输出点了什么菜
        public abstract void Print();
    }

    /// <summary>
    /// 西红柿炒鸡蛋这道菜
    /// </summary>
    public class TomatoScrambledEggs : Food
    {
        public override void Print()
        {
            Console.WriteLine("一份西红柿炒蛋！");
        }
    }

    /// <summary>
    /// 土豆肉丝这道菜
    /// </summary>
    public class ShreddedPorkWithPotatoes : Food
    {
        public override void Print()
        {
            Console.WriteLine("一份土豆肉丝");
        }
    }

    /// <summary>
    /// 简单工厂类, 负责 炒菜
    /// </summary>
    public class FoodSimpleFactory
    {
        public static Food CreateFood(string type)
        {
            Food food = null;
            if (type.Equals("土豆肉丝"))
            {
                food = new ShreddedPorkWithPotatoes();
            }
            else if (type.Equals("西红柿炒蛋"))
            {
                food = new TomatoScrambledEggs();
            }

            return food;
        }
    }
}