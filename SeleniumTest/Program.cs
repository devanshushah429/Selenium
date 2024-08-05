using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;


namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Sample1 sample = new Sample1();
            /*s2.Method1();*/
            /*BasicTutorial();*/
            /*Sample1 sample = new Sample1();
            */
            sample.Initialize();
            sample.ExecuteTest();
            sample.EndTest();
        }

        private static void BasicTutorial()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.google.com/");
            Thread.Sleep(2000);
            IWebElement element = driver.FindElement(By.Name("q"));
            Thread.Sleep(2000);
            element.SendKeys("javatpoint tutorials");
            Thread.Sleep(2000);
            IWebElement element1 = driver.FindElement(By.Name("btnK"));
            element1.Click();
            Thread.Sleep(3000);
            driver.Close();
            Console.WriteLine("ended");
        }
    }
}
