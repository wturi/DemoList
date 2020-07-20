using System;
using System.Threading;

using Newtonsoft.Json;

namespace BlockingCollection
{
    internal class Program
    {
        private static int _runningId = 0;

        private static void Main(string[] args)
        {
            new Thread(Demo).Start();

            new Thread(Demo).Start();

            Console.ReadLine();
        }

        private static void Demo()
        {
            for (var i = 0; i < 100; i++)
            {
                var ieCommandManage = new IeCommandMessage
                {
                    MethodName = "demo",
                    CommandMessage = new CommandMessage()
                    {
                        CustomId = "demoCustomId"
                    }
                };

                try
                {
                    var returnIe = Singleton.Instance.SendAndGet(ieCommandManage);

                    Console.WriteLine($"after send : {JsonConvert.SerializeObject(returnIe?.ResultMessage)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"return null {JsonConvert.SerializeObject(e)}");
                    break;
                }
            }
        }
    }
}