using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Remote;

namespace OcenaKlientowUITests
{
        
    public class PU1Tests
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static RemoteWebDriver OcenaKlientowSession;
        

        [Test]
        public void ApplicationIsLaunched()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "5ec31022-368f-4ba6-a474-fc3a7d7f2a69_w76ajrt0wdmmw!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(OcenaKlientowSession); 
            OcenaKlientowSession.Close();
        }


        [Test]
        public void PrzegladanieBenefitowTestFilter()
        {
            SetupApp();

            OcenaKlientowSession.FindElementByName("Przeglądanie benefitów").Click();
            var idTb =OcenaKlientowSession.FindElementByName("ID");
            idTb.SendKeys("1");
            OcenaKlientowSession.FindElementByName("Szukaj").Click();
            var listItems = OcenaKlientowSession.FindElementsByClassName("ListViewItem");

            Assert.AreEqual(listItems.Count, 1);

            idTb.Clear();
            idTb.SendKeys("");
            OcenaKlientowSession.FindElementByName("Szukaj").Click();
            OcenaKlientowSession.Close();
        }


        [Test]
        public void BenefitNameIsUpdated()
        {
            SetupApp();
            OcenaKlientowSession.FindElementByName("Przeglądanie benefitów").Click();
            Thread.Sleep(1000);
            int Id = 4;
            string BenefitName = "Benefit";
            string Modified = "Modified";
            string newName = $"{BenefitName} {Id} {Modified}";
            var listItems1 = OcenaKlientowSession.FindElementsByClassName("ListViewItem");
            listItems1[Id-1].Click();
            var buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var editButton = buttons.Where(element => element.Text == "Edytuj").FirstOrDefault();
            new Actions(OcenaKlientowSession).MoveToElement(editButton).MoveByOffset(0, 0).Click().Build().Perform();
            //editButton.Click();
            var textBoxes = OcenaKlientowSession.FindElementsByClassName("TextBox");
            var currTB = textBoxes.Where(element => element.Text == $"{BenefitName} {Id}").FirstOrDefault();
            currTB.Clear();
            currTB.SendKeys($"{BenefitName} {Id} {Modified}");
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var saveButton = buttons.Where(element => element.Text == "Zapisz").FirstOrDefault();
            saveButton.Click();
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var okButton = buttons.Where(element => element.Text == "OK").FirstOrDefault();
            okButton.Click();
            Thread.Sleep(2000);
            Assert.AreEqual(currTB.Text, newName);
            BenefitRollback();
            OcenaKlientowSession.Close();
        }

        void BenefitRollback()
        {
            Thread.Sleep(1000);
            int Id = 4;
            string BenefitName = "Benefit";
            string Modified = "Modified";
            string newName = $"{BenefitName} {Id} {Modified}";
            var listItems1 = OcenaKlientowSession.FindElementsByClassName("ListViewItem");
            listItems1[Id - 1].Click();
            var buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var editButton = buttons.Where(element => element.Text == "Edytuj").FirstOrDefault();
            new Actions(OcenaKlientowSession).MoveToElement(editButton).MoveByOffset(0, 0).Click().Build().Perform();
            //editButton.Click();
            var textBoxes = OcenaKlientowSession.FindElementsByClassName("TextBox");
            var currTB = textBoxes.Where(element => element.Text == $"{BenefitName} {Id} {Modified}").FirstOrDefault();
            currTB.Clear();
            currTB.SendKeys($"{BenefitName} {Id}");
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var saveButton = buttons.Where(element => element.Text == "Zapisz").FirstOrDefault();
            saveButton.Click();
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var okButton = buttons.Where(element => element.Text == "OK").FirstOrDefault();
            okButton.Click();
        }


        [Test]
        public void BenefitIsDeleted()
        {
            SetupApp();
            OcenaKlientowSession.FindElementByName("Przeglądanie benefitów").Click();
            Thread.Sleep(1000);
            
            var listItems = OcenaKlientowSession.FindElementsByClassName("ListViewItem");
            int CountBefore = listItems.Count;
            int Id = listItems.Count-1;
            listItems[Id].Click();
            var buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var deleteButton = buttons.Where(element => element.Text == "Usuń").FirstOrDefault();
            new Actions(OcenaKlientowSession).MoveToElement(deleteButton).MoveByOffset(0, 0).Click().Build().Perform();
            //editButton.Click();
            buttons = OcenaKlientowSession.FindElementsByClassName("Button");
            var okButton = buttons.Where(element => element.Text == "Tak").FirstOrDefault();
            okButton.Click();
            Thread.Sleep(1000);
            var listItemsAfterCount = OcenaKlientowSession.FindElementsByClassName("ListViewItem").Count;
            Assert.AreEqual(listItems.Count, listItemsAfterCount+1);
            OcenaKlientowSession.Close();
        }

        public void WpiszWTextBox(string nazwa, string wartosc)
        {
            IWebElement e = OcenaKlientowSession.FindElementByName(nazwa);
            e.SendKeys(wartosc);
        }

        public void SetupApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "5ec31022-368f-4ba6-a474-fc3a7d7f2a69_w76ajrt0wdmmw!App");
            OcenaKlientowSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            OcenaKlientowSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
        }
    }
}
