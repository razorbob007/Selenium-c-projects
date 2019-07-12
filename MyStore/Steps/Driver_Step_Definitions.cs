using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MyStore.Steps
{
    public static class Driver_Step_Definitions
    {
        public static ChromeDriver driver;
        public static IWebDriver Driver { get; set; }

        [BeforeTestRun]
        public static void Initialize()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            driver = new ChromeDriver(options);
            driver.Manage().Cookies.DeleteAllCookies();
        }
        public static ChromeDriver ChromeDriverInstance
        {
            get { return driver; }
        }


        [TearDown]
        public static void TearDown()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
        }

        [AfterTestRun]
        public static void AfterMethod()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
        }
    }
}
