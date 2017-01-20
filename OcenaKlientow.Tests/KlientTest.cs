using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;

namespace OcenaKlientow.Tests
{
    [TestClass]
    public class KlientTest
    {
        #region Constants

        public const string ImieTest = "TestImie";

        public const string NazwiskoTest = "TestNazwisko";

        #endregion
        #region Public methods

        [TestMethod]
        public void DbAddsProperKlientWhenTableExists()
        {
            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var klient = new Klient
                        {
                            Imie = ImieTest,
                            Nazwisko = NazwiskoTest
                        };
                        db.Klienci.Add(klient);
                        db.SaveChanges();
                        Klient fromDbKlient = db.Klienci.Where(klient1 => klient1.Nazwisko.Equals("TestNazwisko")).FirstOrDefault();

                        if (fromDbKlient == null)
                        {
                            return;
                        }
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

        #endregion
    }
}