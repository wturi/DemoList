using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Newtonsoft.Json;

namespace BlockingCollection
{
    /// <summary>
    /// 一个基于BlockingCollection实现的多线程的处理队列
    /// </summary>
    public class ProcessQueue<T>
    {
        private readonly BlockingCollection<T> _queue;

        private readonly CancellationToken _canCellToken;

        //内部线程池
        private readonly List<Thread> _threadCollection;

        //队列是否正在处理数据
        private int _isProcessing;

        //有线程正在处理数据
        private const int Processing = 1;

        //没有线程处理数据
        private const int UnProcessing = 0;

        //队列是否可用
        private volatile bool _enabled = true;

        //内部处理线程数量
        private readonly int _internalThreadCount;

        public event Action<T> ProcessItemEvent;

        //处理异常，需要三个参数，当前队列实例，异常，当时处理的数据
        public event Action<dynamic, Exception, T> ProcessExceptionEvent;

        public ProcessQueue()
        {
            _queue = new BlockingCollection<T>();
            var cancellationTokenSource = new CancellationTokenSource();
            _internalThreadCount = 1;
            _canCellToken = cancellationTokenSource.Token;
            _threadCollection = new List<Thread>();
        }

        public ProcessQueue(int internalThreadCount) : this()
        {
            this._internalThreadCount = internalThreadCount;
        }

        /// <summary>
        /// 队列内部元素的数量
        /// </summary>
        public int GetInternalItemCount()
        {
            return _queue.Count;
        }

        public void Enqueue(T items)
        {
            if (items == null)
            {
                throw new ArgumentException("items");
            }

            _queue.Add(items, _canCellToken);
            DataAdded();
        }

        public void Flush()
        {
            Console.WriteLine(nameof(Flush));

            Console.WriteLine($"_queue.count -> {_queue.Count}");
            StopProcess();

            while (_queue.Count != 0)
            {
                if (!_queue.TryTake(out var item)) continue;
                try
                {
                    ProcessItemEvent?.Invoke(item);
                }
                catch (Exception ex)
                {
                    OnProcessException(ex, item);
                }
            }
        }

        private void DataAdded()
        {
            if (!_enabled) return;
            if (IsProcessingItem()) return;
            ProcessRangeItem();
            StartProcess();
        }

        //判断是否队列有线程正在处理
        private bool IsProcessingItem()
        {
            return Interlocked.CompareExchange(ref _isProcessing, Processing, UnProcessing) != UnProcessing;
        }

        private void ProcessRangeItem()
        {
            for (var i = 0; i < this._internalThreadCount; i++)
            {
                ProcessItem();
            }
        }

        private void ProcessItem()
        {
            var currentThread = new Thread((state) =>
            {
                var item = default(T);
                while (_enabled)
                {
                    try
                    {
                        try
                        {
                            item = _queue.Take(_canCellToken);
                            ProcessItemEvent?.Invoke(item);
                        }
                        catch (OperationCanceledException ex)
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(ex));
                        }
                    }
                    catch (Exception ex)
                    {
                        OnProcessException(ex, item);
                    }
                }
            });

            _threadCollection.Add(currentThread);
        }

        private void StartProcess()
        {
            foreach (var thread in _threadCollection)
            {
                thread.Start();
            }
        }

        private void StopProcess()
        {
            this._enabled = false;
            foreach (var thread in _threadCollection.Where(thread => thread.IsAlive))
            {
                thread.Join();
            }
            _threadCollection.Clear();
        }

        private void OnProcessException(Exception ex, T item)
        {
            var tempException = ProcessExceptionEvent;
            Interlocked.CompareExchange(ref ProcessExceptionEvent, null, null);

            if (tempException != null)
            {
                ProcessExceptionEvent?.Invoke(this, ex, item);
            }
        }
    }
}