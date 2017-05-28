using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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
        public void LinksWithReturnedUrl_WhenNoLogIn()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            string Customerslink = "http://localhost:53257/Account/Login?ReturnUrl=%2f%E5%AE%A2%E6%88%B6%E8%B3%87%E6%96%99";
            driver.FindElement(By.LinkText("Customers")).Click();
            Assert.IsTrue(Customerslink.Equals(driver.Url, StringComparison.InvariantCultureIgnoreCase));

            driver.FindElement(By.LinkText("Customer Data Management")).Click();
            string ContactPersonslink = "http://localhost:53257/%E5%AE%A2%E6%88%B6%E8%81%AF%E7%B5%A1%E4%BA%BA";
            driver.FindElement(By.LinkText("Contact Persons")).Click();
            Assert.IsTrue(ContactPersonslink.Equals(driver.Url, StringComparison.InvariantCultureIgnoreCase));
            driver.FindElement(By.LinkText("Customer Data Management")).Click();
            string BanksLinks = "http://localhost:53257/%E5%AE%A2%E6%88%B6%E9%8A%80%E8%A1%8C%E8%B3%87%E8%A8%8A";
            driver.FindElement(By.LinkText("Banks")).Click();
            //Assert.AreEqual("http://localhost:53257/%E5%AE%A2%E6%88%B6%E9%8A%80%E8%A1%8C%E8%B3%87%E8%A8%8A", driver.Url);
            Assert.IsTrue(BanksLinks.Equals(driver.Url, StringComparison.InvariantCultureIgnoreCase));
            driver.FindElement(By.LinkText("Customer Data Management")).Click();
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

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
