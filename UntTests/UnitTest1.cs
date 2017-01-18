using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UntTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ViewModelNotCountPaymentWhenDbIsEmpty()
        {
            OcenaViewModel ocenaViewModel = GetOcenaViewModel();

            using (var db = new OcenaKlientowContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var klient = new Klient()
                        {
                            Imie = ImieTest,
                            Nazwisko = NazwiskoTest
                        };


                        db.Klienci.Add(klient);
                        db.SaveChanges();
                        var fromDbKlient = db.Klienci.Where(klient1 => klient1.Nazwisko.Equals("TestNazwisko")).FirstOrDefault();

                        Assert.IsTrue(klient.KlientId > 0);
                        Assert.AreEqual(fromDbKlient.Nazwisko, NazwiskoTest);
                        Assert.AreEqual(fromDbKlient.Imie, ImieTest);

                        transaction.Rollback();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        public const string ImieTest = "TestImie";

        public const string NazwiskoTest = "TestNazwisko";

        public OcenaViewModel GetOcenaViewModel()
        {
            return new OcenaViewModel(false);
        }
    }
}
