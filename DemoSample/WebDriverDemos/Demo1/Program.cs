using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace Demo1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Run();

            Console.ReadLine();
        }

        static void Run()
        {
            var driverService = InternetExplorerDriverService.CreateDefaultService();
            
            driverService.HideCommandPromptWindow = true;

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;



            using (IWebDriver driver = new OpenQA.Selenium.IE.InternetExplorerDriver(driverService, new InternetExplorerOptions()))
            {
                driver.Navigate().GoToUrl("http://www.baidu.com");  //driver.Url = "http://www.baidu.com"是一样的

                var source = driver.PageSource;

                Console.WriteLine(source);
            }
        }
    }
}