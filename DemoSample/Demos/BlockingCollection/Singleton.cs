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

        private int _runningId = 0;

        private int _sharedMemoryId = 0;

        private readonly ProcessQueue<IeCommandMessage> _processQueue;

        private readonly ConcurrentDictionary<string, IeCommandMessage> _returnData = new ConcurrentDictionary<string, IeCommandMessage>();

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
            _processQueue = new ProcessQueue<IeCommandMessage>();
            _processQueue.ProcessItemEvent += ProcessQueue_ProcessItemEvent;
            _processQueue.ProcessExceptionEvent += ProcessQueue_ProcessExceptionEvent;
        }

        /// <summary>
        /// 该方法对入队的每个元素进行处理
        /// </summary>
        /// <param name="value"></param>
        private void ProcessQueue_ProcessItemEvent(IeCommandMessage value)
        {
            Task.Factory.StartNew(() =>
            {
                do
                {
                    value.ResultMessage = new ResultMessage
                    {
                        ReturnParameters = new
                        {
                            RunningId = ++_runningId
                        }
                    };

                    if (_returnData.TryGetValue(value.ReturnDataDictionaryKey, out var resultIeCommand) && resultIeCommand == null)
                    {
                        _returnData[value.ReturnDataDictionaryKey] = value;
                    }

                    if (_runningId == 50) Thread.Sleep(value.TimeoutMillisecond + 1000);

                    Thread.Sleep(20);

                    value.IsTimeout = false;
                    value.ResultTime = DateTime.Now;
                } while (value.IsTimeout);
            }).Wait(value.TimeoutMillisecond);

            if (value.IsTimeout)
                throw new TimeoutException();
        }

        /// <summary>
        ///  处理异常
        /// </summary>
        /// <param name="obj">队列实例</param>
        /// <param name="ex">异常对象</param>
        /// <param name="value">出错的数据</param>
        private void ProcessQueue_ProcessExceptionEvent(ProcessQueue<IeCommandMessage> obj, Exception ex, IeCommandMessage value)
        {
            Console.WriteLine($"ProcessQueue_ProcessExceptionEvent -> before stop {obj.GetInternalItemCount()}");
            new Task(obj.StopAndClear).Start();
            Console.WriteLine($"ProcessQueue_ProcessExceptionEvent -> after stop {obj.GetInternalItemCount()}");
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="ieCommandMessage"></param>
        /// <param name="returnDataDictionaryKey"></param>
        /// <returns></returns>
        private bool Send(IeCommandMessage ieCommandMessage, out string returnDataDictionaryKey)
        {
            returnDataDictionaryKey = ieCommandMessage.ReturnDataDictionaryKey = $"BrowserCommand{ieCommandMessage.ManagedThreadId.ToString().PadLeft(3, '0')}{(++_sharedMemoryId).ToString().PadLeft(6, '0')}";

            if (!_returnData.TryAdd(ieCommandMessage.ReturnDataDictionaryKey, null)) return false;

            _processQueue.Enqueue(ieCommandMessage);
            return true;
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IeCommandMessage GetReturnValue(string key)
        {
            return _returnData.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// 发送消息，并根据消息中的超时时间等待数据返回
        /// </summary>
        /// <param name="ieCommandMessage"></param>
        /// <returns></returns>
        public IeCommandMessage SendAndGet(IeCommandMessage ieCommandMessage)
        {
            try
            {
                if (!Send(ieCommandMessage, out var key)) return null;

                var startTime = DateTime.Now;

                do
                {
                    var messageObj = GetReturnValue(key);

                    if (messageObj != null) return messageObj;
                } while (DateTime.Now - startTime < TimeSpan.FromMilliseconds(ieCommandMessage.TimeoutMillisecond));

                throw new TimeoutException();
            }
            catch (Exception e)
            {
                //_cancellationTokenSource.Cancel();
                throw e;
            }
        }
    }
}