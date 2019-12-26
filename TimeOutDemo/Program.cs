using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace TimeOutDemo
{
    internal class Program
    {
        private static int count = 0;
        private static void Main(string[] args)
        {
            try
            {
                var message = string.Empty;
                var dd = string.Empty;
                var ishave = false;


                CallWithTimeout(delegate
                {
                    while (string.IsNullOrEmpty(message))
                    {
                        FiveSecondMethod(111, 222, ref message, ref dd);
                    }
                    ishave = true;
                }, 10000);

                Console.WriteLine(ishave);
                Console.WriteLine(message);
                Console.ReadLine();

            }
            catch (Exception)
            {
                Console.WriteLine("Time Out");
            }

            Console.ReadKey();
        }

        private static string FiveSecondMethod(int arg1, int arg2, ref string message, ref string ddd)
        {
            count++;
            Console.WriteLine(count);
            //Thread.Sleep(1000);
            message = count>20?( "FiveSecondMethod" + arg1 + arg2):null;

            return count > 20 ? message : null;
        }


        private static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
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
                throw new TimeoutException();
            }
        }
    }
}