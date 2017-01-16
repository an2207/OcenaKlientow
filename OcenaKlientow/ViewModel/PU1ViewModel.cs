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

    }
}
