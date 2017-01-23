using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace OcenaKlientowUITests
{
    public class PU2Tests
    {

        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        protected static RemoteWebDriver OcenaKlientowSession;


        public void SetupApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "5ec31022-368f-4ba6-a474-fc3a7d7f2a69_w76ajrt0wdmmw!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            OcenaKlientowSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
        }

        [Test]
        public void CountStatusUpdateCurrentKlientOcenaAndDateEqualsToday()
        {
            SetupApp();

            OcenaKlientowSession.FindElementByName("Przeglądanie klientów").Click();
            Thread.Sleep(1000);
            var listItems1 = OcenaKlientowSession.FindElementsByClassName("ListViewItem");

            var listItem = listItems1[5];
            listItem.Click();
            var buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var countButton = buttons.Where(elem => elem.Text.Equals("Przelicz status")).FirstOrDefault();
            countButton.Click();
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var okButton = buttons.Where(elem => elem.Text.Equals("OK")).FirstOrDefault();
            okButton.Click();
            Thread.Sleep(2000);
            var listItemsAfter = OcenaKlientowSession.FindElementsByClassName("ListViewItem");
            listItem = listItemsAfter[5];
            listItem.Click();
            Thread.Sleep(1000);
            var textBoxes = OcenaKlientowSession.FindElementsByClassName("TextBox");
            bool found = false;
            var cultureInfo = new CultureInfo("pt-BR");
            var todayDate = DateTime.Today.ToString("d", cultureInfo);
            foreach (IWebElement webElement in textBoxes)
            {
                if (todayDate.Equals(webElement.Text))
                {
                    found = true;
                }
            }

            Assert.AreEqual(found, true);
            
            OcenaKlientowSession.Close();
        }


        [Test]
        public void CountAllStatusesIsCalledAndDateEqualsToToday()
        {
            SetupApp();

            OcenaKlientowSession.FindElementByName("Przeglądanie klientów").Click();
            Thread.Sleep(1000);            
            var buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var countButton = buttons.Where(elem => elem.Text.Equals("Przelicz wszystkie statusy")).FirstOrDefault();
            countButton.Click();
            Thread.Sleep(40000);
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var okButton = buttons.Where(elem => elem.Text.Equals("OK")).FirstOrDefault();
            okButton.Click();
            Thread.Sleep(2000);
            var listItemsAfter = OcenaKlientowSession.FindElementsByClassName("ListViewItem");
            var listItem = listItemsAfter[5];
            listItem.Click();
            Thread.Sleep(1000);
            var textBoxes = OcenaKlientowSession.FindElementsByClassName("TextBox");
            bool found = false;
            var cultureInfo = new CultureInfo("pt-BR");
            var todayDate = DateTime.Today.ToString("d", cultureInfo);
            foreach (IWebElement webElement in textBoxes)
            {
                if (todayDate.Equals(webElement.Text))
                {
                    found = true;
                }
            }

            Assert.AreEqual(found, true);
            
            OcenaKlientowSession.Close();
        }
    }
}
