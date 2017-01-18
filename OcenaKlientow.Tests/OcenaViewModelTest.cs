using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.ViewModel;

namespace OcenaKlientow.Tests
{
    [TestClass]
    public class OcenaViewModelTest
    {
        [TestMethod]
        public void DbAddsProperKlientWhenTableExists()
        {
            OcenaViewModel ocenaViewModel = GetOcenaViewModel();
            
            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                var asd = db.Klienci.ToList();
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
