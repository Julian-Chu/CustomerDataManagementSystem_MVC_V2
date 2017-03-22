using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestClass]
    public class LogTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        
        [TestInitialize]
        public void SetupTest()
        {
            //driver = new FirefoxDriver();
            driver = new ChromeDriver();
            baseURL = "http://localhost:53257/";
            verificationErrors = new StringBuilder();
        }
        
        [TestCleanup]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [TestMethod]
        public void TheTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("loginlink")).Click();
            driver.FindElement(By.Id("submitBtn")).Click();
            Assert.AreEqual("The Username field is required.", driver.FindElement(By.CssSelector("span.text-danger.field-validation-error > span")).Text);
            Assert.AreEqual("The Password field is required.", driver.FindElement(By.XPath("//div[2]/div/span/span")).Text);
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("loginlink")).Click();
            //driver.FindElement(By.Id("Username")).Clear();
            driver.FindElement(By.Id("Username")).SendKeys("test1");
            //driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("123");
            //driver.FindElement(By.CssSelector("input.btn.btn-default")).Click();
            driver.FindElement(By.Id("submitBtn")).Click();

            Assert.AreEqual("帳戶錯誤", driver.FindElement(By.CssSelector("span.field-validation-error.text-danger")).Text);
            Assert.AreEqual("密碼錯誤", driver.FindElement(By.XPath("//div[3]/div/span")).Text);
            driver.FindElement(By.Id("Username")).Clear();
            driver.FindElement(By.Id("Username")).SendKeys("test");
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("1234");
            driver.FindElement(By.CssSelector("input.btn.btn-default")).Click();
            driver.Navigate().GoToUrl(baseURL + "/");
            Assert.AreEqual("Hello test", driver.FindElement(By.LinkText("Hello test")).Text);
            driver.FindElement(By.LinkText("登出")).Click();
            Assert.AreEqual("登入", driver.FindElement(By.Id("loginlink")).Text);
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
