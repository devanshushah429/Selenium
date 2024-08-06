using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumTest
{
    class Sample2
    {
        IWebDriver driver = new ChromeDriver();
        [SetUp]
        public void Initialize()
        {
            driver.Navigate().GoToUrl("https://chatgpt.com/");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);
        }
        [Test]
        public void ExecuteTest()
        {
           
            /*IWebElement stayLogOut = driver.FindElement(By.XPath("/html/body/div[1]/div/div/main/div[1]/div[1]/div/div[1]/div/div[2]/div[1]/span/button/svg"));
            stayLogOut.Click();
            Thread.Sleep(2000);*/

            IWebElement searchEle = driver.FindElement(By.Id("prompt-textarea"));
            searchEle.SendKeys("tell me a joke");
            Thread.Sleep(2000);

            IWebElement btnEle = driver.FindElement(By.XPath("//*[@id='__next']/div/div/main/div[1]/div[2]/div[1]/div/form/div/div[2]/div/div/button"));
            btnEle.Click();
            Thread.Sleep(10000);
        }
        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }
    }
}
