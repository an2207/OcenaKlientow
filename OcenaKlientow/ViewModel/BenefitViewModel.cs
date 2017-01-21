using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;

namespace OcenaKlientow.ViewModel
{
    public class BenefitViewModel
    {
        #region Private fields

        #endregion
        #region Properties

        public bool Saving { get; set; }

        #endregion
        #region Ctors

        public BenefitViewModel(bool saving)
        {
            Saving = saving;
        }

        #endregion
        #region Public methods

        public void AddNewBenefit(Benefit newBenefit)
        {
            using (var db = new OcenaKlientowContext())
            {
                db.Benefity.Add(newBenefit);
                if (Saving)
                    db.SaveChanges();
            }
        }

        public void AddStatusToBenefit(int benefitId, string name)
        {
            using (var db = new OcenaKlientowContext())
            {
                Status status = db.Statusy.Where(status1 => status1.Nazwa == name).FirstOrDefault();
                PrzypisanyStatus przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => (status1.BenefitId == benefitId) && (status1.StatusId == status.StatusId)).FirstOrDefault();
                if (przypisanyStatus == null)
                {
                    db.PrzypisaneStatusy.Add(new PrzypisanyStatus
                                             {
                                                 BenefitId = benefitId,
                                                 StatusId = status.StatusId
                                             });
                    if (Saving)
                        db.SaveChanges();
                }
            }
        }

        public List<BenefitView> BenefitListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                IQueryable<BenefitView> innerJoinQuery =
                    from benefit in db.Benefity
                    join rodzaj in db.RodzajeBenefitow on benefit.RodzajId equals rodzaj.RodzajId
                    select new BenefitView
                    {
                        RodzajId = benefit.RodzajId,
                        BenefitId = benefit.BenefitId,
                        DataUaktyw = benefit.DataUaktyw,
                        DataZakon = benefit.DataZakon,
                        WartoscProc = benefit.WartoscProc,
                        LiczbaDni = benefit.LiczbaDni,
                        NazwaBenefitu = benefit.Nazwa,
                        NazwaRodzaju = rodzaj.Nazwa,
                        Opis = benefit.Opis
                    };
                return innerJoinQuery.ToList();
            }
        }

        public void DeleteFromBenefitList(BenefitView benToDel)
        {
            using (var db = new OcenaKlientowContext())
            {
                List<PrzypisanyStatus> przypStat = db.PrzypisaneStatusy.Where(ben => ben.BenefitId.Equals(benToDel.BenefitId)).ToList();
                foreach (PrzypisanyStatus przypisanyStatuse in przypStat)
                    db.Entry(przypisanyStatuse).State = EntityState.Deleted;
                Benefit currBen = db.Benefity.Where(benefit => benefit.BenefitId.Equals(benToDel.BenefitId)).FirstOrDefault();
                db.Entry(currBen).State = EntityState.Deleted;
                if (Saving)
                    db.SaveChanges();
            }
        }

        public void DeleteStatusFromBenefit(int benefitId, string name)
        {
            using (var db = new OcenaKlientowContext())
            {
                Status status = db.Statusy.Where(status1 => status1.Nazwa == name).FirstOrDefault();
                PrzypisanyStatus przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => (status1.BenefitId == benefitId) && (status1.StatusId == status.StatusId)).FirstOrDefault();
                if (przypisanyStatus == null)
                    return;
                db.PrzypisaneStatusy.Remove(przypisanyStatus);
                if (Saving)
                    db.SaveChanges();
            }
        }

        public Benefit GetBenefit(int benefitId)
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.Benefity.Where(benefit1 => benefit1.BenefitId == benefitId).FirstOrDefault();
            }
        }

        public List<RodzajBenefitu> GetBenefitTypes()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.RodzajeBenefitow.ToList();
            }
        }

        public List<int> GetCurrentBenefitPrzypisaneStatuses(BenefitView benefit)
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.PrzypisaneStatusy.Where(status => status.BenefitId == benefit.BenefitId).Select(status => status.StatusId).ToList();
            }
        }

        public RodzajBenefitu GetRabatId()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.RodzajeBenefitow.Where(benefitu => benefitu.Nazwa.Equals("Rabat")).FirstOrDefault();
            }
        }

        public List<Status> GetStatuses()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.Statusy.ToList();
            }
        }

        public RodzajBenefitu GetTerminId()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.RodzajeBenefitow.Where(benefitu => benefitu.Nazwa.Equals("Wydłużony termin")).FirstOrDefault();
            }
        }

        public Benefit ReadBenefit(int benefitId)
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.Benefity.Where(benefit => benefit.BenefitId == benefitId).FirstOrDefault();
            }
        }

        public void UpdateBenefit(Benefit benefit)
        {
            using (var db = new OcenaKlientowContext())
            {
                Benefit toChange = db.Benefity.Where(benefit1 => benefit1.BenefitId == benefit.BenefitId).FirstOrDefault();
                toChange.Nazwa = benefit.Nazwa;
                toChange.DataUaktyw = benefit.DataUaktyw;
                toChange.DataZakon = benefit.DataZakon;
                toChange.LiczbaDni = benefit.LiczbaDni;
                toChange.RodzajId = benefit.RodzajId;
                toChange.Opis = benefit.Opis;
                toChange.WartoscProc = benefit.WartoscProc;
                if (Saving)
                    db.SaveChanges();
            }
        }

        #endregion
    }
}