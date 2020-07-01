using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace RxDemo
{
    public static class Demo1
    {
        public static void Run1()
        {
            Console.WriteLine($"CurrentThreadId:{Thread.CurrentThread.ManagedThreadId}");

            var observable = Enumerable.Range(1, 50).ToObservable(NewThreadScheduler.Default);//申明可观察序列

            var subject = new Subject<int>();//申明Subject

            //订阅subject
            subject.Subscribe((temperature) =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"当前温度：{temperature},ThreadId:{Thread.CurrentThread.ManagedThreadId}");
            });

            observable.Subscribe(subject);//订阅observable

            subject.Subscribe(Run2(subject));
        }

        private static Subject<int> Run2(IObservable<int> subject)
        {
            var subject1 = new Subject<int>();

            //订阅subject
            subject1.Subscribe((temperature) => Console.WriteLine($"嘟嘟嘟，当前水温：{temperature},ThreadId:{Thread.CurrentThread.ManagedThreadId}"));

            return subject1;
        }
    }
}