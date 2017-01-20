using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.ViewModel;

namespace OcenaKlientow.Tests
{
    [TestClass]
    public class OcenaViewModelTest
    {
        #region Public methods

        [TestMethod]
        public void ViewModelCountAllGradesForKlientsWhenTablesExists()
        {
            var ocenaViewModel = new OcenaViewModel(true);

            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var a = db.Klienci;
                        Klient klient = db.Klienci.FirstOrDefault();
                        var culture = new CultureInfo("pt-BR");
                        //includes test for OcenaViewModelStatus.CountStatus()
                        ocenaViewModel.CountAllGrades();
                        db.SaveChanges();
                        string date = DateTime.Now.ToString("d", culture);
                        List<Klient> fromDbKlients = db.Klienci.ToList();
                        List<Ocena> fromDbOcenas = db.Oceny.Where(oc => oc.DataCzas.Equals(date)).ToList();

                        Assert.IsTrue(fromDbOcenas.Count >= fromDbKlients.Count);

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