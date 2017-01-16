using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;

namespace OcenaKlientow.ViewModel
{
    public class Pu1ViewModel
    {


        public List<BenefitListItem> BenefitListQuery()
        {

            using (var db = new OcenaKlientowContext())
            {
                var innerJoinQuery =
               from benefit in db.Benefity
               join rodzaj in db.RodzajeBenefitow on benefit.RodzajId equals rodzaj.RodzajId
               select new BenefitListItem()
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

        public void DeleteFromBenefitList(BenefitListItem benToDel)
        {
            using (var db = new OcenaKlientowContext())
            {
                var przypStat = db.PrzypisaneStatusy.Where(ben => ben.BenefitId.Equals(benToDel.BenefitId)).ToList();
                foreach (PrzypisanyStatus przypisanyStatuse in przypStat)
                {
                    db.Entry(przypisanyStatuse).State = EntityState.Deleted;
                }
                var currBen = db.Benefity.Where(benefit => benefit.BenefitId.Equals(benToDel.BenefitId)).FirstOrDefault();
                db.Entry(currBen).State = EntityState.Deleted;
                db.SaveChanges();

            }
        }

        public void AddStatusToBenefit(Benefit benefit, string name)
        {
            using (var db = new OcenaKlientowContext())
            {
                var status = db.Statusy.Where(status1 => status1.Nazwa == name).FirstOrDefault();
                var przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => status1.BenefitId == benefit.BenefitId && status1.StatusId == status.StatusId).FirstOrDefault();
                if (przypisanyStatus == null)
                {
                    db.PrzypisaneStatusy.Add(new PrzypisanyStatus()
                    {
                        BenefitId = benefit.BenefitId,
                        StatusId = status.StatusId
                    });
                    db.SaveChanges();
                }
            }

        }

        public void DeleteStatusFromBenefit(Benefit benefit, string name)
        {
            using (var db = new OcenaKlientowContext())
            {
                var status = db.Statusy.Where(status1 => status1.Nazwa == name).FirstOrDefault();
                var przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => status1.BenefitId == benefit.BenefitId && status1.StatusId == status.StatusId).FirstOrDefault();
                if (przypisanyStatus == null)
                {
                    return;
                }
                db.PrzypisaneStatusy.Remove(przypisanyStatus);
                db.SaveChanges();
            }
        }

        public Benefit GetBenefit(BenefitListItem curr)
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.Benefity.Where(benefit1 => benefit1.BenefitId == curr.BenefitId).FirstOrDefault();
            }
        }

        public void UpdateBenefit(Benefit benefit)
        {
            using (var db = new OcenaKlientowContext())
            {
                var toChange = db.Benefity.Where(benefit1 => benefit1.BenefitId == benefit.BenefitId).FirstOrDefault();
                toChange.Nazwa = benefit.Nazwa;
                toChange.DataUaktyw = benefit.DataUaktyw;
                toChange.DataZakon = benefit.DataZakon;
                toChange.LiczbaDni = benefit.LiczbaDni;
                toChange.RodzajId = benefit.RodzajId;
                toChange.Opis = benefit.Opis;
                toChange.WartoscProc = benefit.WartoscProc;
                db.SaveChanges();
            }
        }

        public RodzajBenefitu GetRabatId()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.RodzajeBenefitow.Where(benefitu => benefitu.Nazwa.Equals("Rabat")).FirstOrDefault();
            }
        }

        public RodzajBenefitu GetTerminId()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.RodzajeBenefitow.Where(benefitu => benefitu.Nazwa.Equals("Wydłużony termin")).FirstOrDefault();
            }
        }

        public void AddNewBenefit(Benefit newBenefit)
        {
            using (var db = new OcenaKlientowContext())
            {
                db.Benefity.Add(newBenefit);
                db.SaveChanges();
            }
        }

        public List<Status> GetStatuses()
        {
            using (var db  = new OcenaKlientowContext())
            {
                return db.Statusy.ToList();
            }
        }

        public List<RodzajBenefitu> GetBenefitTypes()
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.RodzajeBenefitow.ToList();
            }
        }

        public List<int> GetCurrentBenefitPrzypisaneStatuses(BenefitListItem benefit)
        {
            using (var db = new OcenaKlientowContext())
            {
                return db.PrzypisaneStatusy.Where(status => status.BenefitId == benefit.BenefitId).Select(status => status.StatusId).ToList();
            }
        }
    }
}
