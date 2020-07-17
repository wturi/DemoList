using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingCollection
{
    internal class Program
    {
        private static int _runningId = 0;

        private static void Main(string[] args)
        {
            var processQueue = new ProcessQueue<int>();
            processQueue.ProcessExceptionEvent += ProcessQueue_ProcessExceptionEvent;
            processQueue.ProcessItemEvent += ProcessQueue_ProcessItemEvent;

            processQueue.Enqueue(1);
            processQueue.Enqueue(2);
            processQueue.Enqueue(3);

            Console.ReadLine();
        }

        /// <summary>
        /// 该方法对入队的每个元素进行处理
        /// </summary>
        /// <param name="value"></param>
        private static void ProcessQueue_ProcessItemEvent(int value)
        {
            Console.WriteLine($"{++_runningId}--{value}");
        }

        /// <summary>
        ///  处理异常
        /// </summary>
        /// <param name="obj">队列实例</param>
        /// <param name="ex">异常对象</param>
        /// <param name="value">出错的数据</param>
        private static void ProcessQueue_ProcessExceptionEvent(dynamic obj, Exception ex, int value)
        {
            Console.WriteLine(ex.ToString());
        }

        private static void Demo1()
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MMMM-dd HH:mm:ss.fff}");

            var tempStr = "aaa";

            new Task(() => { SimulationExecution($"{tempStr}.1"); }).Start();
            new Task(() => { SimulationExecution($"{tempStr}.2"); }).Start();
            new Task(() => { SimulationExecution($"{tempStr}.3"); }).Start();
            new Task(() => { SimulationExecution($"{tempStr}.4"); }).Start();
            new Task(() => { SimulationExecution($"{tempStr}.5"); }).Start();

            Console.WriteLine($"sleep 5000 ms");

            Thread.Sleep(5000);

            new Task(() => { SimulationExecution($"{tempStr}.6"); }).Start();
            new Task(() => { SimulationExecution($"{tempStr}.7"); }).Start();
            new Task(() => { SimulationExecution($"{tempStr}.8"); }).Start();
        }

        private static void SimulationExecution(string tempStr)
        {
            try
            {
                for (var i = 0; i < 100; i++)
                {
                    var resultData = Singleton.Instance.SendAndReturn($"{tempStr}.{Task.CurrentId}.{i}");

                    Console.WriteLine(resultData.ToString());
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"is cancel");
            }
        }
    }
}