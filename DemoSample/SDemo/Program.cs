using System;
using System.IO;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;

namespace SDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] files = Directory.GetFiles("D:\\", "AipSdk.dll");
            foreach (string dir in files)
            {
                Console.WriteLine(dir);
            }
        }

        public static void Demo()
        {
            #region chrome 配置文档

            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();

            driverService.HideCommandPromptWindow = true;//关闭黑色cmd窗口

            ChromeOptions options = new ChromeOptions();
            // GPU加速可能会导致Chrome出现黑屏及CPU占用率过高,所以禁用
            options.AddArgument("--disable-gpu");
            // 伪装user-agent
            //options.AddArgument("user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1");
            // 设置chrome启动时size大小
            //options.AddArgument("--window-size=414,736");

            #endregion chrome 配置文档

            #region ie 配置文档

            InternetExplorerDriverService ieService = InternetExplorerDriverService.CreateDefaultService();

            ieService.HideCommandPromptWindow = true;
            //IWebDriver driver = new InternetExplorerDriver(ieService);

            #endregion ie 配置文档

            //chrome
            IWebDriver driver = new ChromeDriver(driverService, options);

            driver.Navigate().GoToUrl("https://rental.myfuwu.com.cn/fed/bill-reminder?_smp=Rental.BillReminder");

            Actions actionsObj = new Actions(driver);

            Thread.Sleep(1000);

            #region 鼠标悬停

            //点击
            IWebElement eles = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[1]/div/div[2]/section[1]/div[2]/div[1]/div[1]/section[2]/span[2]/span"));
            actionsObj.Click(eles).Perform();

            eles = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div[1]/div[1]/div[2]/div[2]/table/tbody/tr[2]/td[6]/div"));
            actionsObj.Click(eles).Perform();

            eles = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div[1]/div[2]/div[2]/div[2]/table/tbody/tr[2]/td[4]/div"));
            actionsObj.MoveToElement(eles).Perform();

            #endregion 鼠标悬停

            Console.ReadKey();
        }
    }
}