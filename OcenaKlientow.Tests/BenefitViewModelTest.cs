using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.ViewModel;

namespace OcenaKlientow.Tests
{
    [TestClass]
    public class BenefitViewModelsTests
    {
        [TestMethod]
        public void ViemModelAddsPrzypisanyStatusWhenBenefitIdAndStatusNazwaAreGiven()
        {
            BenefitViewModel viewModel = new BenefitViewModel(true);
            int benefitId;
            string statusName;

            using (var db = new OcenaKlientowContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefitId = db.Benefity.Select(benefit => benefit.BenefitId).FirstOrDefault();
                        statusName = db.Statusy.Select(status => status.Nazwa).FirstOrDefault();
                        viewModel.AddStatusToBenefit(benefitId, statusName);
                        var fromDbPrzypisanyStatus = db.PrzypisaneStatusy.Where(status => status.BenefitId == benefitId).ToList();

                        Assert.IsTrue(fromDbPrzypisanyStatus.Count>0);
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
            BenefitViewModel viewModel = new BenefitViewModel(true);
            int benefitId;
            string statusName;

            using (var db = new OcenaKlientowContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefitId = db.Benefity.Select(benefit => benefit.BenefitId).FirstOrDefault();
                        statusName = db.Statusy.Select(status => status.Nazwa).FirstOrDefault();
                        viewModel.AddStatusToBenefit(benefitId, statusName);

                        viewModel.DeleteStatusFromBenefit(benefitId, statusName);
                        var fromDbPrzypisanyStatus = db.PrzypisaneStatusy.Where(status => status.BenefitId == benefitId).ToList();
                        var oneStatus = fromDbPrzypisanyStatus.FirstOrDefault();

                        Assert.IsTrue(fromDbPrzypisanyStatus.Count==0);
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
            BenefitViewModel viewModel = new BenefitViewModel(false);
            Benefit benefitCurr;
            string statusName;

            using (var db = new OcenaKlientowContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefitCurr = db.Benefity.FirstOrDefault();
                        string newBenefitName = "TestBenefit";
                        benefitCurr.Nazwa = newBenefitName;
                        viewModel.UpdateBenefit(benefitCurr);
                        var fromDbBenefit = db.Benefity.Where(benefit => benefit.BenefitId == benefitCurr.BenefitId).FirstOrDefault();
                        
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
    }
}
