using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingCollection
{
    public class Singleton
    {
        private static Singleton _singleton;

        private static readonly object Locker = new object();

        private static readonly BlockingCollection<object> BlockingCollection = new BlockingCollection<object>();

        private readonly ConcurrentDictionary<string, object> _returnData = new ConcurrentDictionary<string, object>();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private static int _executionOrder = 0;

        public static Singleton Instance
        {
            get
            {
                if (_singleton != null) return _singleton;
                lock (Locker)
                {
                    if (_singleton != null) return _singleton;
                    // ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
                    _singleton = new Singleton();
                }

                return _singleton;
            }
        }

        private Singleton()
        {
            new Task(RaiseDataEvent).Start();
        }

        /// <summary>
        /// 发送消息并等会返回值
        /// </summary>
        /// <param name="msg"></param>
        public object SendAndReturn(string msg)
        {
            try
            {
                if (!Send(msg, _cancellationTokenSource.Token)) throw new SendAndReturnException();

                object resultValue = null;

                var isTimeout = !Task.Factory.StartNew(() =>
                {
                    do
                    {
                        resultValue = GetResultValue(msg);
                    } while (resultValue == null);
                }, _cancellationTokenSource.Token).Wait(5000);

                if (isTimeout)
                {
                    Console.WriteLine($"timeout -> SendAndReturn - {DateTime.Now:yyyy-mm-dd HH:mm:ss.fff}");
                }

                return isTimeout ? throw new TimeoutException() : resultValue;
            }
            catch (AggregateException aggregateException)
            {
                return aggregateException.Message;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cancellationToken"></param>
        private bool Send(string msg, CancellationToken cancellationToken)
        {
            try
            {
                return _returnData.TryAdd(msg, null) && BlockingCollection.TryAdd(msg, 1000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"cancellation token is cancel");
                BlockingCollection.CompleteAdding();
                return false;
            }
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        private object GetResultValue(string key)
        {
            return _returnData.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// 消费数据
        /// </summary>
        private void RaiseDataEvent()
        {
            foreach (var obj in BlockingCollection.GetConsumingEnumerable())
            {
                if (BlockingCollection.IsAddingCompleted) return;
                ExecuteDataEvent(obj, obj.ToString());
            }


            Console.WriteLine($"blocking collection is dispose");
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        private void ExecuteDataEvent(object obj, string key)
        {
            string resultMsg = null;
            var isTimeout = !Task.Factory.StartNew(() =>
             {
                 if (_executionOrder == 100)
                 {
                     Thread.Sleep(3000);
                 }
                 else
                 {
                     resultMsg = $"{++_executionOrder} ---- {DateTime.Now:yyyy-mm-dd HH:mm:ss.fff} ---- {obj}";

                     Thread.Sleep(20);
                 }
             }).Wait(2000);

            if (isTimeout)
            {
                BlockingCollection.CompleteAdding();

                _cancellationTokenSource.Cancel();

                Console.WriteLine($"timeout -> complete adding");

                return;
            }

            if (_returnData.ContainsKey(key))
            {
                _returnData[key] = resultMsg;
            }
        }
    }
}