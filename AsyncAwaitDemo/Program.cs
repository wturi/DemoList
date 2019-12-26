using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    internal class Program
    {
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private static Dictionary<string, object> _demo = new Dictionary<string, object>();

        private static void Main(string[] args)
        {
            for (int i = 1; i < 6; i++)
            {
               var a=  GoGoGo("a" + i, "a" + i, "a" + i);
            }
            for (int i = 1; i < 6; i++)
            {
                var a = GoGoGo("a" + i, "a" + i, "a" + i);
            }

            Console.ReadLine();
        }

        private static async Task<string> GoGoGo(string arg1, string arg2, string arg3)
        {
            return await Task.Run(() =>
            {
                Console.WriteLine(arg1 + "开始排队...");
                Console.WriteLine(arg1 + "开始执行...");
                _semaphore.Wait();
                Thread.Sleep(1000);
                if (_demo.ContainsKey(arg1))
                {
                    _semaphore.Release();
                }
                else
                {
                    _demo.Add(arg1, arg1);
                    _semaphore.Release();
                }
                Console.WriteLine();
                Console.WriteLine(arg1 + "执行完毕，离开！");
                return "结束";
            });
        }

        public static object Get(string name, object obj)
        {
            _semaphore.Wait();
            Thread.Sleep(1000);
            if (_demo.ContainsKey(name))
            {
                _semaphore.Release();
                return _demo[name];
            }
            else
            {
                _demo.Add(name, obj);
                _semaphore.Release();
                return obj;
            }
        }
    }
}