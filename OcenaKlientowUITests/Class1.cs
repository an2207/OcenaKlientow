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
        protected static RemoteWebDriver OcenaKlientowSession;
        protected static RemoteWebElement CalculatorResult;
        protected static string OriginalCalculatorMode;
        
        
        public static void Setup()
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(OcenaKlientowSession);
            OcenaKlientowSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

            // Make sure we're in standard mode
            OcenaKlientowSession.FindElementByName("Menu").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[starts-with(@Name, \"Menu\")]").Click();
            OriginalCalculatorMode = OcenaKlientowSession.FindElementByXPath("//List[@AutomationId=\"FlyoutNav\"]//ListItem[@IsSelected=\"True\"]").Text;
            OcenaKlientowSession.FindElementByXPath("//ListItem[@Name=\"Standard Calculator\"]").Click();

            // Use series of operation to locate the calculator result text element as a workaround
            // We currently cannot query element by automationId without using modified appium dot net driver
            // TODO: Use a proper appium/webdriver nuget package that allow us to query based on automationId
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Clear\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Seven\"]").Click();
            CalculatorResult = OcenaKlientowSession.FindElementByName("Display is  7 ") as RemoteWebElement;
            Assert.IsNotNull(CalculatorResult);
        }
        

        public static void TearDown()
        {
            // Restore original mode before closing down
            OcenaKlientowSession.FindElementByXPath("//Button[starts-with(@Name, \"Menu\")]").Click();
            OcenaKlientowSession.FindElementByXPath("//ListItem[@Name=\"" + OriginalCalculatorMode + "\"]").Click();

            CalculatorResult = null;
            OcenaKlientowSession.Dispose();
            OcenaKlientowSession = null;
        }
        [Test]
        public void ApplicationIsLaunched()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "5ec31022-368f-4ba6-a474-fc3a7d7f2a69_w76ajrt0wdmmw!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(OcenaKlientowSession); 
        }


        [Test]
        public void PrzegladanieBenefitowTestFilter()
        {
            // Launch the calculator app
            SetupApp();

            OcenaKlientowSession.FindElementByName("Przeglądanie benefitów").Click();
            //OcenaKlientowSession.FindElementByName("OcenaKlientow.View.ListItems.BenefitView").Click();
            OcenaKlientowSession.FindElementByName("ID").Click();
            //OcenaKlientowSession.FindElementByName("edit");
            WpiszWTextBox("ID", "1");
            OcenaKlientowSession.FindElementByName("Szukaj").Click();
        }

        [Test]
        public void PositionInSetList()
        {
            SetupApp();

            OcenaKlientowSession.FindElementByName("Przeglądanie benefitów").Click();
            
            var list = OcenaKlientowSession.FindElements(By.XPath("//label/input"));
            var list1 = OcenaKlientowSession.FindElements(By.XPath("OcenaKlientow.View.ListItems.BenefitView//IdBen"));

            var list2 = OcenaKlientowSession.FindElements(By.XPath(".//TextBlock//IdBen"));
            var listGrid = OcenaKlientowSession.FindElements(By.XPath("//Grid"));


            Assert.AreEqual(1,1);


            
        }

        public void SetupApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "5ec31022-368f-4ba6-a474-fc3a7d7f2a69_w76ajrt0wdmmw!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            OcenaKlientowSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
        }

        public void WpiszWTextBox(string nazwa, string wartosc)
        {
            IWebElement e = OcenaKlientowSession.FindElementByName(nazwa);
            e.SendKeys(wartosc);
            // zdjęcie focusa z inputa...
        }

        [Test]
        public void Test1()
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(OcenaKlientowSession);
            OcenaKlientowSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            CalculatorResult = OcenaKlientowSession.FindElementByName("Wyświetlane: 0 ") as RemoteWebElement;


            OcenaKlientowSession.FindElementByName("Jeden").Click();
            OcenaKlientowSession.FindElementByName("Plus").Click();
            OcenaKlientowSession.FindElementByName("Siedem").Click();
            OcenaKlientowSession.FindElementByName("Równa się").Click();
            Assert.AreEqual("Wyświetlane: 8 ", CalculatorResult.Text);
        }

        [Test]
        public void CalcTest()
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "C:\\Users\\Mateusz Łopusiński\\Desktop\\Studia\\Semestr 5\\PO\\1.0.2.1\\1.0.2\\OcenaKlientow\\OcenaKlientow\\bin\\x86\\Release\\AppX\\OcenaKlientow.exe");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(OcenaKlientowSession);
            OcenaKlientowSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            CalculatorResult = OcenaKlientowSession.FindElementByName("Wyświetlane: 0 ") as RemoteWebElement;


            OcenaKlientowSession.FindElementByName("Jeden").Click();
            OcenaKlientowSession.FindElementByName("Plus").Click();
            OcenaKlientowSession.FindElementByName("Siedem").Click();
            OcenaKlientowSession.FindElementByName("Równa się").Click();
            Assert.AreEqual("Wyświetlane: 8 ", CalculatorResult.Text);
        }

        [Test]
        public void Combination()
        {
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Seven\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Multiply by\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Nine\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Plus\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"One\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Equals\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Divide by\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Eight\"]").Click();
            OcenaKlientowSession.FindElementByXPath("//Button[@Name=\"Equals\"]").Click();
            Assert.AreEqual("Display is  8 ", CalculatorResult.Text);
        }

        [Test]
        public void Division()
        {
            OcenaKlientowSession.FindElementByName("Eight").Click();
            OcenaKlientowSession.FindElementByName("Eight").Click();
            OcenaKlientowSession.FindElementByName("Divide by").Click();
            OcenaKlientowSession.FindElementByName("One").Click();
            OcenaKlientowSession.FindElementByName("One").Click();
            OcenaKlientowSession.FindElementByName("Equals").Click();
            Assert.AreEqual("Display is  8 ", CalculatorResult.Text);
        }

        [Test]
        public void Multiplication()
        {
            OcenaKlientowSession.FindElementByName("Nine").Click();
            OcenaKlientowSession.FindElementByName("Multiply by").Click();
            OcenaKlientowSession.FindElementByName("Nine").Click();
            OcenaKlientowSession.FindElementByName("Equals").Click();
            Assert.AreEqual("Display is  81 ", CalculatorResult.Text);
        }

        [Test]
        public void Subtraction()
        {
            OcenaKlientowSession.FindElementByName("Nine").Click();
            OcenaKlientowSession.FindElementByName("Minus").Click();
            OcenaKlientowSession.FindElementByName("One").Click();
            OcenaKlientowSession.FindElementByName("Equals").Click();
            Assert.AreEqual("Display is  8 ", CalculatorResult.Text);
        }
    }
}
