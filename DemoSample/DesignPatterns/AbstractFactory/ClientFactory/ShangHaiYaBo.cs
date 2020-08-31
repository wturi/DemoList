using System;

namespace ClientFactory
{
    public class ShangHaiYaBo:YaBo
    {
        #region Overrides of YaBo

        /// <summary>
        /// 打印方法，用于输出信息
        /// </summary>
        public override void Print()
        {
            Console.WriteLine("上海的鸭脖");
        }

        #endregion
    }
}