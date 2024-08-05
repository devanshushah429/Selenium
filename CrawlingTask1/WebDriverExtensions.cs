using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CrawlingTask1
{
    public static class WebDriverExtensions
    {
        public static IWebElement WaitForElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static void WaitForElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(drv => drv.FindElements(by).Count > 0);
            }
        }
    }

}
