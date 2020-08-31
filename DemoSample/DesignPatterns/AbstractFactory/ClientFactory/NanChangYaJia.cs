using System;

namespace ClientFactory
{
    public class NanChangYaJia : YaJia
    {
        #region Overrides of YaJia

        /// <summary>
        /// 打印方法，用于输出信息
        /// </summary>
        public override void Print()
        {
            Console.WriteLine("南昌的鸭架子");
        }

        #endregion Overrides of YaJia
    }
}