using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;

namespace MyStore.Steps
{
    public class Driver_Step_Definitions
    {
        public static ChromeDriver driver;
        public static string dt = DateTime.Now.ToString("yyyMMddTHHmmssfff");
        private static string pointInTime;
        private readonly ScenarioContext context;
        private static ExtentReports _extent;
        private static ExtentTest TestFeature;
        private static ExtentTest TestScenario;
        public static string localDirectory = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop\\MyStore\\" + dt + "\\";


        public static IWebDriver Driver { get; set; }

        public Driver_Step_Definitions(ScenarioContext injectedContext)
        {
            context = injectedContext;
        }

        [BeforeTestRun]
        public static void Initialize()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            driver = new ChromeDriver(options);

            //Initialize extent reports
            var htmlReporter = new ExtentHtmlReporter(localDirectory + "Extentreport.html");
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
            htmlReporter.AppendExisting = true;
            _extent.CreateTest("phptravels");
        }
        public static ChromeDriver ChromeDriverInstance
        {
            get { return driver; }
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            TestFeature = _extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            TestScenario = TestFeature.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            pointInTime = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
            var StepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            string ScenarioName = ScenarioStepContext.Current.StepInfo.Text;
            if (ScenarioContext.Current.TestError == null)
            {
                if (StepType == "Given")
                {
                    TestScenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Pass"));
                }
                if (StepType == "When")
                {
                    TestScenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Pass"));
                }
                if (StepType == "Then")
                {
                    TestScenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Pass"));
                }
                if (StepType == "And")
                {
                    TestScenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Pass"));
                }
            }
            else if (ScenarioContext.Current.TestError != null)
            {
                if (StepType == "Given")
                {
                    TestScenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message + "Exception : " + ScenarioContext.Current.TestError.InnerException);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Fail"));
                }
                if (StepType == "When")
                {
                    TestScenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message + "Exception : " + ScenarioContext.Current.TestError.InnerException);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Fail"));
                }
                if (StepType == "Then")
                {
                    TestScenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message + "Exception : " + ScenarioContext.Current.TestError.InnerException);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Fail"));
                }
                if (StepType == "And")
                {
                    TestScenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message + "Exception : " + ScenarioContext.Current.TestError.InnerException);
                    TestScenario.CreateNode(ScenarioName, CaptureScreenShot(ScenarioName, pointInTime, "Fail"));
                }
            }
        }

        public static string CaptureScreenShot(string userfolder, string Datetime, string Status)
        {
            try
            {
                string path = localDirectory + pointInTime + ".jpg";
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot screenshot = ts.GetScreenshot();
                screenshot.SaveAsFile(path);
                if (Status == "Pass")
                {
                    TestScenario.Pass(userfolder, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
                }
                else if(Status == "Fail")
                {
                    TestScenario.Fail(userfolder, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
                }
                else
                {
                    TestScenario.Info(userfolder, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
                }

                return path;
            }
            catch (Exception e)
            {
                TestFeature.Fail("Failed to log: " + e.ToString());
                return e.ToString(); 
            }
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
