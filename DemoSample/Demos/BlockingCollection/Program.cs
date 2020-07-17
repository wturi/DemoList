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
            var processQueue = new ProcessQueue<string>();
            processQueue.ProcessItemEvent += ProcessQueue_ProcessItemEvent;
            processQueue.ProcessExceptionEvent += ProcessQueue_ProcessExceptionEvent;

            new Thread(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    processQueue.Enqueue($"{Thread.CurrentThread.ManagedThreadId}.{i}");
                }
            }).Start();

            new Thread(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    processQueue.Enqueue($"{Thread.CurrentThread.ManagedThreadId}.{i}");
                }
            }).Start();

            new Thread(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    processQueue.Enqueue($"{Thread.CurrentThread.ManagedThreadId}.{i}");
                }
            }).Start();

            new Thread(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    processQueue.Enqueue($"{Thread.CurrentThread.ManagedThreadId}.{i}");
                }
            }).Start();

            new Thread(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    processQueue.Enqueue($"{Thread.CurrentThread.ManagedThreadId}.{i}");
                }
            }).Start();

            Console.ReadLine();
        }

        /// <summary>
        /// 该方法对入队的每个元素进行处理
        /// </summary>
        /// <param name="value"></param>
        private static void ProcessQueue_ProcessItemEvent(string value)
        {
            Console.WriteLine($"{++_runningId}--{value}");

            Thread.Sleep(10);

            if (_runningId == 50) throw new TimeoutException();
        }

        /// <summary>
        ///  处理异常
        /// </summary>
        /// <param name="obj">队列实例</param>
        /// <param name="ex">异常对象</param>
        /// <param name="value">出错的数据</param>
        private static void ProcessQueue_ProcessExceptionEvent(ProcessQueue<string> obj, Exception ex, string value)
        {
            Console.WriteLine(ex.ToString());
            obj.StopAndClear();
            Console.WriteLine($"ProcessQueue_ProcessExceptionEvent -> end {obj.GetInternalItemCount()}");
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