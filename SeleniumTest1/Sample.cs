using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumTest1
{
    class Sample
    {
        IWebDriver driver = new ChromeDriver();
        public void Initialize()
        {
            driver.Navigate().GoToUrl("https://darshanums.in/Login.aspx/");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);
        }

        public void Execute()
        {
            IEnumerable<IWebElement> elements = driver.FindElements(By.XPath("//div[contains(@class,'auctions-list')]//div[@id]"));
            foreach(IWebElement element in elements)
            {
                element.FindElement
            }
        }
    }
}
