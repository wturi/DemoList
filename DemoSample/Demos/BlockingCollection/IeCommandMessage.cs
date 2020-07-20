using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

using mshtml;

namespace BlockingCollection
{
    [StructLayout(LayoutKind.Sequential)]
    public class IeCommandMessage
    {
        private static int AllTimeMillisecond { set; get; }

        private static DateTime StartTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="allTimeMillisecondMillisecond">总超时时间(单位：毫秒)</param>
        /// <param name="startTime">开始时间节点</param>
        public IeCommandMessage(int allTimeMillisecondMillisecond, DateTime startTime)
        {
            AllTimeMillisecond = allTimeMillisecondMillisecond;
            StartTime = startTime;

            CreateTime = DateTime.Now;
            TimeoutMillisecond = AllTimeMillisecond - (int)(CreateTime - StartTime).TotalMilliseconds;
        }

        public IeCommandMessage()
        {
            TimeoutMillisecond = 10000;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 当前线程唯一标识符
        /// </summary>
        public int ManagedThreadId { get; set; } = Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// 执行的方法名：nameof(IeWebExecutor.Click)
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 传入的参数
        /// </summary>
        public CommandMessage CommandMessage { get; set; }

        /// <summary>
        /// 返回的参数
        /// </summary>
        public ResultMessage ResultMessage { get; set; }

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsTimeout { get; set; } = true;

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeoutMillisecond { get; set; }

        /// <summary>
        /// 读取返回参数的key
        /// </summary>
        public string ReturnDataDictionaryKey { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 得到结果时间
        /// </summary>
        public DateTime ResultTime { get; set; }
    }

    public class CommandMessage
    {
        public Point Point { get; set; }
        public Rectangle Rectangle { get; set; }
        public string CustomId { get; set; }
        public IHTMLDocument2 Document { get; set; }
        public bool ClearFirst { get; set; }
        public string Text { get; set; }
        public string PropertyName { get; set; }
        public string SessionId { get; set; }
        public string InputXml { get; set; }
        public string Selector { get; set; }
        public string Javascript { get; set; }
        public string Method { get; set; }
        public int Timeout { get; set; }
        public int ExecuteTimeout { get; set; }
        public object[] Args { get; set; }
        public Point Location { get; set; }
    }

    public class ResultMessage
    {
        public object ReturnParameters { get; set; }
        public dynamic OutParameters { get; set; }
    }
}