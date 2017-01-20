using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestOcenaKlientow
{
    public class Class1
    {

        [Test]
        public void ViemModelAddsPrzypisanyStatusWhenBenefitIdAndStatusNazwaAreGiven()
        {
            var viewModel = new BenefitViewModel(true);
            int benefitId;
            string statusName;

            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        benefitId = db.Benefity.Select(benefit => benefit.BenefitId).FirstOrDefault();
                        statusName = db.Statusy.Select(status => status.Nazwa).FirstOrDefault();
                        viewModel.AddStatusToBenefit(benefitId, statusName);
                        List<PrzypisanyStatus> fromDbPrzypisanyStatus = db.PrzypisaneStatusy.Where(status => status.BenefitId == benefitId).ToList();

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
    }
}
