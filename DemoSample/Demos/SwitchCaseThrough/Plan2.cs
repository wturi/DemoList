using System.Collections.Generic;
using System.Drawing;

namespace SwitchCaseThrough
{
    /// <summary>
    /// 通过 dictionay 和委托的方式重构 switch case
    /// </summary>
    public class Plan2
    {
        /// <summary>
        /// 定义一个委托
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private delegate Point actionByMain(Point point);

        /// <summary>
        /// 定义一个dictionary
        /// </summary>
        private static Dictionary<ClickLocation, actionByMain> mainList = new Dictionary<ClickLocation, actionByMain>();

        public static void RunPlan()
        {
            AddMainList();

            var requestPoint = new Point { X = 200, Y = 300 };

            var e = mainList[ClickLocation.Center](requestPoint);
        }

        private static void AddMainList()
        {
            mainList.Add(ClickLocation.Center, ClickCenter);
            mainList.Add(ClickLocation.LeftTop, ClickLeftTop);
        }

        #region 具体操作

        public static Point ClickCenter(Point point)
        {
            var returnPoint = new Point
            {
                X = point.X / 2,
                Y = point.Y / 2
            };
            return returnPoint;
        }

        public static Point ClickLeftTop(Point point)
        {
            var returnPoint = new Point
            {
                X = 1,
                Y = 1
            };
            return returnPoint;
        }

        public static Point ClickLeftBottom(Point point)
        {
            var returnPoint = new Point
            {
                X = 1,
                Y = point.Y
            };
            return returnPoint;
        }

        public static Point ClickRightTop(Point point)
        {
            var returnPoint = new Point
            {
                X = point.X,
                Y = 1
            };
            return returnPoint;
        }

        public static Point ClickRightBottom(Point point)
        {
            var returnPoint = new Point
            {
                X = point.X,
                Y = point.Y
            };
            return returnPoint;
        }

        #endregion 具体操作
    }

    public enum ClickLocation
    {
        Center,
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom
    }
}