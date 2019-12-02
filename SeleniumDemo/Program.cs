using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace SeleniumDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IWebDriver driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl("https://www.baidu.com");

            IWebElement searchText = driver.FindElement(By.Id("kw"));

            searchText.Clear();
            searchText.SendKeys("Selenium");

            IWebElement searchBtn = driver.FindElement(By.Id("su"));

            searchBtn.Click();
        }
    }
}