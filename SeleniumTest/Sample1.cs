using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace SeleniumTest
{
    [TestFixture]
    class Sample1
    {
        IWebDriver driver = new ChromeDriver();
        [SetUp]
        public void Initialize()
        {
            driver.Navigate().GoToUrl("https://darshanums.in/Login.aspx/");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);
        }
        [Test]
        public void ExecuteTest()
        {
            IWebElement roleElement = driver.FindElement(By.Id("rblRole_1"));
            roleElement.Click();
            Thread.Sleep(2000);
            IWebElement emailOrUserNameElement = driver.FindElement(By.Id("txtUsername"));
            emailOrUserNameElement.SendKeys("21010101179");
            Thread.Sleep(2000);
            Console.WriteLine("username value is entered");
            IWebElement passwordElement = driver.FindElement(By.Id("txtPassword"));
            passwordElement.SendKeys("");
            Thread.Sleep(2000);
            Console.WriteLine("Password Entered");
            IWebElement loginButtonElement = driver.FindElement(By.Id("btnLogin"));
            loginButtonElement.Click();
            Thread.Sleep(3000);
            Console.WriteLine("Login Button Clicked");
        }
        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }
    }
}
