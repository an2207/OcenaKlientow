using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;
using Remotion.Linq.Clauses;

namespace OcenaKlientow.ViewModel
{
    public class KlientViewModel
    {
        CultureInfo culture = new CultureInfo("pt-BR");

        private List<Klient> Klients;


        public KlientViewModel()
        {
            using (var db = new OcenaKlientowContext())
            {
                Klients = db.Klienci.ToList();
            }
        }

     

        public List<KlientView> OsobyPrawneListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                var statusyQuery =
                from klient in db.Klienci where !klient.CzyFizyczna
                join ocena in db.Oceny on klient.KlientId equals ocena.KlientId 
                join status in db.Statusy on  ocena.StatusId equals status.StatusId
                select new KlientView()
                {
                    
                        Nazwa = klient.Nazwa,
                        CzyFizyczna = klient.CzyFizyczna,
                        KlientId = klient.KlientId,
                        KwotaKredytu = klient.KwotaKredytu,
                        NIP = klient.NIP,
                    
                        OcenaId = ocena.OcenaId,
                        DataCzas = ocena.DataCzas,
                        SumaPkt = ocena.SumaPkt,
                    
                        NazwaStatusu = status.Nazwa,
                        StatusId = status.StatusId,
                        ProgDolny = status.ProgDolny,
                        ProgGorny = status.ProgGorny
                    
                    
                };
                var lista = statusyQuery.ToList();
                var secList = lista;
                var filtered = statusyQuery.ToList();
                foreach (KlientView osobyListItem in filtered)
                {
                    foreach (KlientView listItem in filtered)
                    {
                        if (listItem.KlientId == osobyListItem.KlientId && listItem.OcenaId < osobyListItem.OcenaId)
                        {
                            secList = secList.Where(item => item.OcenaId!=listItem.OcenaId).ToList();
                        }
                    }
                }
                return secList.OrderBy(item => item.KlientId).ToList();
            }
            
        }

        public List<KlientView> OsobyFizyczneListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                var statusyQuery =
                from klient in db.Klienci
                where klient.CzyFizyczna 
                join ocena in db.Oceny on klient.KlientId equals ocena.KlientId
                join status in db.Statusy on ocena.StatusId equals status.StatusId
                select new KlientView()
                {
                    
                        Nazwisko = klient.Nazwisko,
                        Imie = klient.Imie,
                        DrugieNazwisko = klient.DrugieNazwisko,
                        DrugieImie = klient.DrugieImie,
                        CzyFizyczna = klient.CzyFizyczna,
                        KlientId = klient.KlientId,
                        KwotaKredytu = klient.KwotaKredytu,
                        PESEL= klient.PESEL,
                    
                        OcenaId = ocena.OcenaId,
                        DataCzas = ocena.DataCzas,
                        SumaPkt = ocena.SumaPkt,
                    
                        ProgDolny = status.ProgDolny,
                        ProgGorny = status.ProgGorny,
                        NazwaStatusu = status.Nazwa
                    

                };
                var lista = statusyQuery.ToList();
                var secList = lista;
                var filtered = statusyQuery.ToList();
                foreach (KlientView osobyListItem in filtered)
                {
                    foreach (KlientView listItem in filtered)
                    {
                        if (listItem.KlientId == osobyListItem.KlientId && listItem.OcenaId < osobyListItem.OcenaId)
                        {
                            secList = secList.Where(item => item.OcenaId != listItem.OcenaId).ToList();
                        }
                    }
                }
                return secList.OrderBy(item => item.KlientId).ToList();
            }

        }

       

        public Klient ReadKlient(int klientId)
        {
            using (var db =new OcenaKlientowContext())
            {
                return db.Klienci.Where(klient => klient.KlientId == klientId).FirstOrDefault();
            }
        }

       
    }

}
