using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MemoryMessageQueueTest
{
    public delegate void Consumer(IList<object> monitors);

    public enum ConsumeErrorAction
    {
        AbandonAndLogException
    }

    public enum NotReachBatchCountConsumeAction
    {
        ConsumeAllItems
    }

    public enum ReachMaxItemCountAction
    {
        AbandonOldItems
    }

    public class MyQueueConfig
    {
        private string queueName;
        public Consumer Consumer { get; private set; }

        public MyQueueConfig(string name, Consumer c)
        {
            queueName = name;
            Consumer = c;
        }

        public int MaxItemCount { get; set; }
        public ConsumeErrorAction ConsumeErrorAction { get; set; }
        public int ConsumeIntervalMilliseconds { get; set; }
        public int ConsumeItemCountInOneBatch { get; set; }
        public int ConsumeThreadCount { get; set; }
        public NotReachBatchCountConsumeAction NotReachBatchCountConsumeAction { get; set; }
        public ReachMaxItemCountAction ReachMaxItemCountAction { get; set; }
    }

    public class MyQueue
    {
        private MyQueueConfig c;
        private Queue<object> queue;
        private Dictionary<int, Queue<List<object>>> queueData;
        private Dictionary<int, Thread> threads;

        public MyQueue()
        {
            queue = new Queue<object>();
            queueData = new Dictionary<int, Queue<List<object>>>();
            threads = new Dictionary<int, Thread>();
        }

        public void Init(MyQueueConfig config)
        {
            c = config;
            for (int i = 0; i < c.ConsumeThreadCount; i++)
            {
                Thread thread = new Thread(Consumer);
                thread.Start(i);
                threads.Add(i, thread);
                queueData.Add(i, new Queue<List<object>>());
            }

            Thread threadBack = new Thread(Consumer);
            threadBack.Start(c.ConsumeThreadCount);
            queueData.Add(c.ConsumeThreadCount, new Queue<List<object>>());

            new Thread(SendConsumer).Start();
        }

        private void SendConsumer()
        {
            try
            {
                while (true)
                {
                    List<object> forConsumer = new List<object>();
                    int queueCount = queue.Count;
                    //队列空时
                    if (queueCount == 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    //队列过大时
                    if (queueCount > c.MaxItemCount - 1000)
                    {
                        queue.Clear();
                        continue;
                    }

                    var consumerCount = c.ConsumeItemCountInOneBatch;
                    //队列不满每次消费数量时
                    if (queueCount < consumerCount)
                    {
                        consumerCount = queueCount;
                    }

                    for (int i = 0; i < consumerCount; i++)
                    {
                        forConsumer.Add(queue.Dequeue());
                    }

                    List<object[]> batchs = forConsumer.BatchesOf(consumerCount / c.ConsumeThreadCount).ToList();
                    if (batchs.Count < c.ConsumeThreadCount)
                    {
                        batchs.ForEach(t => { queueData[0].Enqueue(t.ToList()); });
                    }
                    else
                    {
                        for (int i = 0; i < batchs.Count; i++)
                        {
                            queueData[i].Enqueue(batchs[i].ToList());
                        }
                    }

                    //获取大队列数据
                    //分发到线程数量的小队列中
                    Thread.Sleep(c.ConsumeIntervalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                //异常117
            }
        }

        private void Consumer(object index)
        {
            try
            {
                var queueIndex = Convert.ToInt32(index);
                while (true)
                {
                    if (queueData[queueIndex].Count > 0)
                    {
                        var forConsumerQueue = queueData[queueIndex].Dequeue();
                        if (forConsumerQueue.Count > 0)
                        {
                            c.Consumer(forConsumerQueue);
                        }
                        else
                        {
                            Thread.Sleep(c.ConsumeIntervalMilliseconds);
                        }
                    }
                    else
                    {
                        Thread.Sleep(c.ConsumeIntervalMilliseconds);
                    }

                    //Thread.Sleep(c.ConsumeIntervalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                //异常150
            }
        }

        public void Enqueue(object obj)
        {
            queue.Enqueue(obj);
        }
    }
}