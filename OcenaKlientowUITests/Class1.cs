using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace OcenaKlientowUITests
{
        
    public class Class1
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static RemoteWebDriver CalculatorSession;
        protected static RemoteWebElement CalculatorResult;
        protected static string OriginalCalculatorMode;
        
        
        public static void Setup()
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            CalculatorSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(CalculatorSession);
            CalculatorSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

            // Make sure we're in standard mode
            CalculatorSession.FindElementByName("Menu").Click();
            CalculatorSession.FindElementByXPath("//Button[starts-with(@Name, \"Menu\")]").Click();
            OriginalCalculatorMode = CalculatorSession.FindElementByXPath("//List[@AutomationId=\"FlyoutNav\"]//ListItem[@IsSelected=\"True\"]").Text;
            CalculatorSession.FindElementByXPath("//ListItem[@Name=\"Standard Calculator\"]").Click();

            // Use series of operation to locate the calculator result text element as a workaround
            // We currently cannot query element by automationId without using modified appium dot net driver
            // TODO: Use a proper appium/webdriver nuget package that allow us to query based on automationId
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Clear\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Seven\"]").Click();
            CalculatorResult = CalculatorSession.FindElementByName("Display is  7 ") as RemoteWebElement;
            Assert.IsNotNull(CalculatorResult);
        }
        

        public static void TearDown()
        {
            // Restore original mode before closing down
            CalculatorSession.FindElementByXPath("//Button[starts-with(@Name, \"Menu\")]").Click();
            CalculatorSession.FindElementByXPath("//ListItem[@Name=\"" + OriginalCalculatorMode + "\"]").Click();

            CalculatorResult = null;
            CalculatorSession.Dispose();
            CalculatorSession = null;
        }

        

        [Test]
        public void Addition()
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "5ec31022-368f-4ba6-a474-fc3a7d7f2a69!App");
            CalculatorSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(CalculatorSession);
            CalculatorSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            CalculatorResult = CalculatorSession.FindElementByName("Wyświetlane: 0 ") as RemoteWebElement;


            CalculatorSession.FindElementByName("Jeden").Click();
            CalculatorSession.FindElementByName("Plus").Click();
            CalculatorSession.FindElementByName("Siedem").Click();
            CalculatorSession.FindElementByName("Równa się").Click();
            Assert.AreEqual("Wyświetlane: 8 ", CalculatorResult.Text);
        }

        [Test]
        public void Test1()
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "C:\\Users\\Mateusz Łopusiński\\Desktop\\Studia\\Semestr 5\\PO\\1.0.2.1\\1.0.2\\OcenaKlientow\\OcenaKlientow\\bin\\x86\\Release\\AppX");
            CalculatorSession = new IOSDriver<IOSElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(CalculatorSession);
            CalculatorSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            CalculatorResult = CalculatorSession.FindElementByName("Wyświetlane: 0 ") as RemoteWebElement;


            CalculatorSession.FindElementByName("Jeden").Click();
            CalculatorSession.FindElementByName("Plus").Click();
            CalculatorSession.FindElementByName("Siedem").Click();
            CalculatorSession.FindElementByName("Równa się").Click();
            Assert.AreEqual("Wyświetlane: 8 ", CalculatorResult.Text);
        }

        [Test]
        public void Combination()
        {
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Seven\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Multiply by\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Nine\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Plus\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"One\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Equals\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Divide by\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Eight\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Equals\"]").Click();
            Assert.AreEqual("Display is  8 ", CalculatorResult.Text);
        }

        [Test]
        public void Division()
        {
            CalculatorSession.FindElementByName("Eight").Click();
            CalculatorSession.FindElementByName("Eight").Click();
            CalculatorSession.FindElementByName("Divide by").Click();
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("Display is  8 ", CalculatorResult.Text);
        }

        [Test]
        public void Multiplication()
        {
            CalculatorSession.FindElementByName("Nine").Click();
            CalculatorSession.FindElementByName("Multiply by").Click();
            CalculatorSession.FindElementByName("Nine").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("Display is  81 ", CalculatorResult.Text);
        }

        [Test]
        public void Subtraction()
        {
            CalculatorSession.FindElementByName("Nine").Click();
            CalculatorSession.FindElementByName("Minus").Click();
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("Display is  8 ", CalculatorResult.Text);
        }
    }
}
