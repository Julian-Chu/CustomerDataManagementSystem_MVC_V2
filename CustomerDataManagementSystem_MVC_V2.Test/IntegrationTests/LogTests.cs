using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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
            Assert.AreEqual("The Username field is required.", driver.FindElement(By.CssSelector("span#UsernameError.text-danger.field-validation-error > span")).Text);
            Assert.AreEqual("The Password field is required.", driver.FindElement(By.CssSelector("span#PasswordError.text-danger.field-validation-error > span")).Text);
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("loginlink")).Click();
            driver.FindElement(By.Id("Username")).Clear();
            driver.FindElement(By.Id("Username")).SendKeys("Incorrect");
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("Incorrect");
            driver.FindElement(By.Id("submitBtn")).Click();

            Assert.AreEqual("Incorrect account or password!", driver.FindElement(By.Id("errorMessage")).Text);
            driver.FindElement(By.Id("Username")).Clear();
            driver.FindElement(By.Id("Username")).SendKeys("admin");
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("12345678");
            driver.FindElement(By.Id("submitBtn")).Click();

            driver.Navigate().GoToUrl(baseURL + "/");
            Assert.AreEqual("admin", driver.FindElement(By.LinkText("admin")).Text);
            driver.FindElement(By.CssSelector("a.btn.dropdown-toggle")).Click();
            //driver.FindElement(By.LinkText("Log Out")).Click();
            driver.FindElement(By.Id("logout")).Click();
            Assert.AreEqual("Login", driver.FindElement(By.Id("loginlink")).Text);
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
