using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;

namespace SeleniumDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IWebDriver driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl("http://www.google.co.uk");
            IWebElement queryBox = driver.FindElement(By.Name("q"));
            queryBox.SendKeys("The Automated Tester");
            queryBox.SendKeys(Keys.ArrowDown);
            queryBox.Submit();
        }
    }
}