using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MMQ;

namespace BrowserCommandMQ
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Memory Queue Test Start");
            //SingletonRun();

            new Task(() => { TestEnqueue("thread1"); }).Start();
            new Task(() => { TestEnqueue("thread2"); }).Start();

            Thread.Sleep(100);
            new Task(() => { TestDequeue("thread1"); }).Start();

            new Task(() => { TestDequeue("thread2"); }).Start();

            Console.ReadLine();
        }

        /// <summary>
        /// 单例运行
        /// </summary>
        private static void SingletonRun()
        {
            var mutex = new Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out var isAppRunning);

            if (!isAppRunning)
            {
                Environment.Exit(1);
            }
        }

        #region 测试

        private static void TestEnqueue(string threadNum)
        {
            using (var queue = MemoryMappedQueue.Create("UniqueName"))
            {
                using (var producer = queue.CreateProducer())
                {
                    var num = 1;
                    while (true)
                    {
                        var test = $"Hello,{threadNum}!{++num}";
                        var message = Encoding.UTF8.GetBytes(test);
                        producer.Enqueue(message);

                        Console.WriteLine($"{threadNum} enqueue to memory : {test}");

                        Thread.Sleep(10);
                    }
                }
            }
        }

        private static void TestDequeue(string threadNum)
        {
            using (var consumer = MemoryMappedQueue.CreateConsumer("UniqueName"))
            {
                while (true)
                {
                    var message = consumer.Dequeue();
                    var text = Encoding.UTF8.GetString(message);
                    Console.WriteLine($"\t\t\t\t\t\t {threadNum} read message from memory : {text}");
                }
            }
        }

        #endregion 测试
    }
}