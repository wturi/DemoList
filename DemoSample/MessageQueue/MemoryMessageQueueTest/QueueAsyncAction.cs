using System;
using System.Collections.Generic;

namespace MemoryMessageQueueTest
{
    public class QueueAsyncAction
    {
        private static readonly MyQueue queueService = new MyQueue();
        private static bool _enable;
        private static int _threadCount = 2;
        private static int _mSecond = 1000;
        private static int _oneBatch = 500;

        private static object _root = new object();

        private static QueueAsyncAction _instance;

        public static QueueAsyncAction Instance
        {
            get
            {
                if (_instance == null)
                    lock (_root)
                        if (_instance == null)
                            _instance = new QueueAsyncAction();

                return _instance;
            }
        }

        private QueueAsyncAction()
        {
            InitMemoryQueueService();
        }

        private void InitMemoryQueueService()
        {
            try
            {
                queueService.Init(new MyQueueConfig("Member_AsyncAction", Consumer)
                {
                    ConsumeIntervalMilliseconds = _mSecond,
                    ConsumeItemCountInOneBatch = _oneBatch,
                    ConsumeThreadCount = _threadCount,
                    MaxItemCount = 100000,
                    NotReachBatchCountConsumeAction = NotReachBatchCountConsumeAction.ConsumeAllItems,
                    ReachMaxItemCountAction = ReachMaxItemCountAction.AbandonOldItems,
                });
            }
            catch (Exception ex)
            {
                //异常处理
            }
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="monitors"></param>
        private void Consumer(IList<object> monitors)
        {
            try
            {
                foreach (var item in monitors)
                {
                    //消费方法
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex)
            {
                //异常处理
            }
        }

        /// <summary>
        /// 添加队列项
        /// </summary>
        /// <param name="action"></param>
        public void AddItem(object action)
        {
            try
            {
                queueService.Enqueue(action);
            }
            catch (Exception ex)
            {
                //异常处理
            }
        }
    }
}