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
    public class MainLinksTest
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
        public void TheMainLinksTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.LinkText("客戶資料")).Click();
            Assert.AreEqual("http://localhost:53257/%E5%AE%A2%E6%88%B6%E8%B3%87%E6%96%99", driver.Url);
            driver.FindElement(By.LinkText("Home")).Click();
            driver.FindElement(By.LinkText("客戶聯絡人")).Click();
            Assert.AreEqual("http://localhost:53257/%E5%AE%A2%E6%88%B6%E8%81%AF%E7%B5%A1%E4%BA%BA", driver.Url);
            driver.FindElement(By.LinkText("Home")).Click();
            driver.FindElement(By.LinkText("客戶銀行資訊")).Click();
            Assert.AreEqual("http://localhost:53257/%E5%AE%A2%E6%88%B6%E9%8A%80%E8%A1%8C%E8%B3%87%E8%A8%8A", driver.Url);
            driver.FindElement(By.LinkText("Home")).Click();
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
