using System;
using System.Threading;

namespace TimeOutDemo
{
    internal class Program
    {
        private static int count = 0;

        private static void Main(string[] args)
        {

            var tokenSource1 = new CancellationTokenSource();
            try
            {
                var message = string.Empty;
                var dd = string.Empty;
                var shave = false;



                CallWithTimeout(delegate
                {

                    Children(message, dd, tokenSource1);


                    shave = true;
                }, 2000, tokenSource1);

                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Time Out");
            }

            Console.ReadKey();
        }


        private static void Children(string message, string dd, CancellationTokenSource cancellationTokenSource)
        {
            CallWithTimeout(delegate
            {
                while (string.IsNullOrEmpty(message))
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        throw new TimeoutException();
                    }
                    FiveSecondMethod(111, 222, ref message, ref dd);
                }

            }, 5000, cancellationTokenSource);
        }

        private static string FiveSecondMethod(int arg1, int arg2, ref string message, ref string ddd)
        {
            count++;
            Console.WriteLine(count);
            Thread.Sleep(500);
            message = count > 20 ? ("FiveSecondMethod" + arg1 + arg2) : null;

            return count > 20 ? message : null;
        }

        private static void CallWithTimeout(Action action, int timeoutMilliseconds, CancellationTokenSource cancellationTokenSource)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                threadToKill.IsBackground = true;
                action();
            };
            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                cancellationTokenSource.Cancel();
                throw new TimeoutException();
            }
        }
    }
}