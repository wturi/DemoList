using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace RxDemo
{
    public static class Demo1
    {
        public static void Run1()
        {
            Thread.CurrentThread.Name = "Main";

            Console.WriteLine($"CurrentThreadId:{Thread.CurrentThread.Name}");

            IScheduler thread1 = new NewThreadScheduler(x => new Thread(x) { Name = "Thread1" });
            IScheduler thread2 = new NewThreadScheduler(x => new Thread(x) { Name = "Thread2" });

            Observable.Create<int>(o =>
                {
                    Console.WriteLine("Subscribing on " + Thread.CurrentThread.Name);
                    for (var i = 0; i < 5; i++)
                    {
                        o.OnNext(i);
                    }
                    return Disposable.Create(() => { });
                })
                .SubscribeOn(thread1)
                .ObserveOn(thread2)
                .Subscribe(x => Console.WriteLine("Observing '" + x + "' on " + Thread.CurrentThread.Name));
        }
    }
}