using System;
using System.Collections.Generic;
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
    public class BenefitViewModelsTests
    {
        #region Public methods

        [TestMethod]
        public void ViemModelAddsPrzypisanyStatusWhenBenefitIdAndStatusNazwaAreGiven()
        {
            var viewModel = new BenefitViewModel(true);
            Benefit benefit;
            string statusName;

            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefit = db.Benefity.FirstOrDefault();
                        statusName = db.Statusy.Select(status => status.Nazwa).FirstOrDefault();
                        viewModel.AddStatusToBenefit(benefit, statusName);
                        List<PrzypisanyStatus> fromDbPrzypisanyStatus = db.PrzypisaneStatusy.Where(status => status.BenefitId == benefit.BenefitId).ToList();

                        Assert.IsTrue(fromDbPrzypisanyStatus.Count > 0);
                        Assert.IsNotNull(fromDbPrzypisanyStatus.FirstOrDefault());
                        transaction.Rollback();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        [TestMethod]
        public void ViemModelDeletesPrzypisanyStatusWhenBenefitIdAndStatusNazwaAreGiven()
        {
            var viewModel = new BenefitViewModel(true);
            Benefit benefit;
            string statusName;

            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefit = db.Benefity.FirstOrDefault();
                        statusName = db.Statusy.Select(status => status.Nazwa).FirstOrDefault();
                        viewModel.AddStatusToBenefit(benefit, statusName);

                        viewModel.DeleteStatusFromBenefit(benefit, statusName);
                        List<PrzypisanyStatus> fromDbPrzypisanyStatus = db.PrzypisaneStatusy.Where(status => status.BenefitId == benefit.BenefitId).ToList();
                        PrzypisanyStatus oneStatus = fromDbPrzypisanyStatus.FirstOrDefault();

                        Assert.IsTrue(fromDbPrzypisanyStatus.Count == 0);
                        Assert.IsNull(oneStatus);
                        transaction.Rollback();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        [TestMethod]
        public void ViewModelUpdatesBenefitWhenTableIsCreatedAndBenefitExists()
        {
            var viewModel = new BenefitViewModel(false);
            Benefit benefitCurr;

            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefitCurr = db.Benefity.FirstOrDefault();
                        var newBenefitName = "TestBenefit";
                        benefitCurr.Nazwa = newBenefitName;
                        viewModel.UpdateBenefit(benefitCurr);
                        Benefit fromDbBenefit = db.Benefity.Where(benefit => benefit.BenefitId == benefitCurr.BenefitId).FirstOrDefault();

                        Assert.AreEqual(fromDbBenefit.Nazwa, benefitCurr.Nazwa);
                        transaction.Rollback();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        #endregion
    }
}