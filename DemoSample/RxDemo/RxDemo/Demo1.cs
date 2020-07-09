using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxDemo
{
    public static class Demo1
    {
        public static void Run1()
        {
            var observable = Enumerable.Range(1, 100).ToObservable(NewThreadScheduler.Default);//申明可观察序列
            Subject<int> subject = new Subject<int>();//申明Subject
            subject.Subscribe((temperature) => Console.WriteLine($"当前温度：{temperature}"));//订阅subject
            subject.Subscribe((temperature) => Console.WriteLine($"嘟嘟嘟，当前水温：{temperature}"));//订阅subject
            observable.Subscribe(subject);//订阅observable
        }
    }
}