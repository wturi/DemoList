using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace UnitTestProject_WebDriverDemos
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver _driver;
        private string _homeUrl;

        [TestMethod]
        [Description("Check SauceLabs Homepage for Login Link")]
        public void Login_is_on_home_page()
        {
            //初始化数据
            _driver = new ChromeDriver();
            _homeUrl = "https://www.SauceLabs.com";

            //跳转到home url
            _driver.Navigate().GoToUrl(_homeUrl);

            var wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(15));
            wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/beta/login']")));

            var element = _driver.FindElement(By.XPath("//a[@href='/beta/login']"));

            Assert.AreEqual("Sign In", element.GetAttribute("text"));
        }
    }
}