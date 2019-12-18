using System;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(ConfigFileExtension ="config", Watch = true)]

namespace MyApp
{
    internal class Program
    {
        public static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string chromeMessage = OpenStandardStreamIn();

                log.Info("--------------------My application starts with Chrome Extension message: " + chromeMessage + "---------------------------------");
            }
        }

        private static string OpenStandardStreamIn()
        {
            Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);

            string input = "";
            for (int i = 0; i < length; i++)
            {
                input += (char)stdin.ReadByte();
            }

            return input;
        }
    }
}