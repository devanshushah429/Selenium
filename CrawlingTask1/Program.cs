using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlingTask1
{
    class Program
    {
        public static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://ineichen.com/auctions/past/";
            driver.Navigate();
            driver.Manage().Window.Maximize();
            var elements = driver.FindElements(By.XPath("//h2[contains(@class,'auction-item__name')]/a"));
            Thread.Sleep(10000);
            foreach (var element in elements)
            {
                Console.WriteLine("Element Text: " + element.Text);
            }
            Console.ReadLine();

        }
    }
}
