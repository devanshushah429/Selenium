using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeleniumTest1
{
    class Program
    {
        public static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.sothebys.com/en/buy/luxury/watches/watch?locale=en");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Custom wait condition to ensure elements are present
            List<IWebElement> elements = wait.Until(d =>
            {
                return d.FindElements(By.XPath("//ul//a[contains(@class,'link_linkStyles__Flsyy')]")).ToList();
            });

            foreach (IWebElement element in elements)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine(element.GetAttribute("href"));
                Console.WriteLine("--------------------------------");
                string url = element.GetAttribute("href");

                // Ensure that navigating to the URL does not cause stale references
                GetTitle(url);
            }

            driver.Quit();
        }

        public static void GetTitle(string url)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            IWebDriver driver = new ChromeDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                driver.Navigate().GoToUrl(url);

                // Wait for the h1 element to be present
                IWebElement element1 = wait.Until(d =>
                {
                    return d.FindElement(By.XPath("//h1[contains(@class,'headline')]"));
                });

                Console.WriteLine(element1.Text);
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found: {ex.Message}");
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"Stale element reference: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                driver.Close();
            }
        }
    }
}