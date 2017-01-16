﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;

namespace OcenaKlientow.ViewModel
{
    public class Pu2ViewModel
    {
        CultureInfo culture = new CultureInfo("pt-BR");

        private List<Klient> Klients;
        

        public Pu2ViewModel()
        {
            using (var db = new OcenaKlientowContext())
            {
                Klients = db.Klienci.ToList();
            }
        }

        int PartialPayment(Klient klient)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CZESC").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                foreach (var zamowienie in zam)
                {
                    var listaPlatnosci = db.Platnosci.Where(platnosc => platnosc.ZamowienieId == zamowienie.ZamowienieId);
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (DateTime.Parse(platnosc?.DataZaplaty, culture) < DateTime.Parse(platnosc?.DataWymag, culture))
                        {
                            sum += wart;
                            break;
                        }
                    }
                }
                return sum;
            }
        }

        int FullPayment(Klient klient)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CALK").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                foreach (var zamowienie in zam)
                {
                    sum += wart;

                    var listaPlatnosci = db.Platnosci.Where(platnosc => platnosc.ZamowienieId == zamowienie.ZamowienieId).ToList();

                    var firstPaymentDate = listaPlatnosci[0]?.DataZaplaty;
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (firstPaymentDate == null || firstPaymentDate != platnosc?.DataZaplaty)
                        {
                            sum -= wart;
                            break;
                        }
                    }
                }
                return sum;
            }
        }

        int RegularOrders(Klient klient)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "REGUL_ZAM").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                zam.Sort(Comparison);
                if (zam.Count == 0)
                    return 0;
                var firstOrder = zam[0];
                foreach (Zamowienie zamowienie in zam)
                {
                    var firstOrderDate = DateTime.Parse(firstOrder.DataZamowienia, culture);
                    if (firstOrder.ZamowienieId == zamowienie.ZamowienieId)
                    {
                        continue;
                    }
                    sum += wart;
                    DateTime currZamDateTime = DateTime.Parse(zamowienie.DataZamowienia, culture);
                    if (!(currZamDateTime.Month == firstOrderDate.Month && currZamDateTime.Year == firstOrderDate.Year))
                    {
                        if (!(firstOrderDate.AddMonths(1).Month == currZamDateTime.Month))
                        {
                            sum -= wart;
                            break;
                        }

                    }

                }

                return sum;
            }
        }

        int AfterDeadline(Klient klient)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PRZEK_TERM").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                foreach (var zamowienie in zam)
                {
                    var listaPlatnosci = db.Platnosci.Where(platnosc => platnosc.ZamowienieId == zamowienie.ZamowienieId);
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        var dataWymag = DateTime.Parse(platnosc?.DataWymag, culture);
                        if (platnosc.DataZaplaty == null)
                        {
                            if (dataWymag < DateTime.Now)
                            {
                                var liczbaDni = (int)(DateTime.Now - dataWymag).TotalDays;
                                sum += wart * liczbaDni;
                            }
                        }
                        else
                        {
                            var dataZap = DateTime.Parse(platnosc.DataZaplaty, culture);
                            if (DateTime.Parse(platnosc?.DataZaplaty, culture) > DateTime.Parse(platnosc?.DataWymag, culture))
                            {
                                var liczbaDni = (int)(dataZap - dataWymag).TotalDays;
                                sum += wart * liczbaDni;
                            }
                        }


                    }
                }
                return sum;
            }
        }

        int LoanLimit(Klient klient)
        {
            double sumOfPayments = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "LIMIT KREDYTU").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                foreach (var zamowienie in zam)
                {
                    var listaPlatnosci = db.Platnosci.Where(platnosc => platnosc.ZamowienieId == zamowienie.ZamowienieId);
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (platnosc.DataZaplaty == null)
                        {
                            sumOfPayments += platnosc.Kwota;

                        }

                    }
                }
                if (klient.KwotaKredytu == 0)
                {
                    return 0;
                }
                if (sumOfPayments >= klient.KwotaKredytu)
                {
                    return wart;
                }
            }
            return 0;
        }


        void AssignGrade(Klient klient)
        {
            Dictionary<int, int> parameters = new Dictionary<int, int>();
            int sum = 0;
            Ocena lastOcena;
            using (var db = new OcenaKlientowContext())
            {

                var parId = db.Parametry.Where(parametr => parametr.Nazwa == "REGUL_ZAM").Select(parametr => parametr.ParametrId).FirstOrDefault();
                var points = RegularOrders(klient);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "LIMIT KREDYTU").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = LoanLimit(klient);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CZESC").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = PartialPayment(klient);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CALK").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = FullPayment(klient);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PRZEK_TERM").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = AfterDeadline(klient);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_NA_CZAS").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = PaymentOnTime(klient);
                parameters.Add(parId, points);
                sum += points;
                int statusId=-1;
                var statusList = db.Statusy.ToList();
                foreach (Status statuse in statusList)
                {
                    if (sum < statuse.ProgGorny && sum > statuse.ProgDolny)
                    {
                        statusId = statuse.StatusId;
                    }
                }
                db.Oceny.Add(new Ocena()
                {
                    DataCzas = DateTime.Now.ToString("d", culture),
                    KlientId = klient.KlientId,
                    StatusId = statusId, 
                    SumaPkt = sum
                });
                db.SaveChanges();
               // db.Wyliczono.
                var currKlientOcenyList = db.Oceny.Where(ocena => ocena.KlientId.Equals(klient.KlientId)).OrderByDescending(ocena => ocena.OcenaId).ToList();
                lastOcena = currKlientOcenyList[0];
               

            }
            foreach (KeyValuePair<int, int> keyValuePair in parameters)
            {
                using (var db = new OcenaKlientowContext())
                {
                    var wyliczenie = new Wyliczenie()
                    {
                        OcenaId = lastOcena.OcenaId,
                        ParametrId = keyValuePair.Key,
                        WartoscWyliczona = keyValuePair.Value
                    };
                    db.Wyliczono.Add(wyliczenie);
                    db.SaveChanges();
                }
                

            }


        }
        int PaymentOnTime(Klient klient)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_NA_CZAS").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                foreach (var zamowienie in zam)
                {
                    sum += wart;
                    var listaPlatnosci = db.Platnosci.Where(platnosc => platnosc.ZamowienieId == zamowienie.ZamowienieId);
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (!(DateTime.Parse(platnosc?.DataZaplaty, culture) < DateTime.Parse(platnosc?.DataWymag, culture)))
                        {
                            sum -= wart;
                            break;
                        }

                    }
                }
                return sum;
            }
        }

        private int Comparison(Zamowienie zamowienie, Zamowienie zamowienie1)
        {
            if (DateTime.Parse(zamowienie.DataZamowienia, culture) > DateTime.Parse(zamowienie1.DataZamowienia, culture))
            {
                return 1;
            }
            if (DateTime.Parse(zamowienie.DataZamowienia, culture) < DateTime.Parse(zamowienie1.DataZamowienia, culture))
            {
                return -1;
            }
            return 0;
        }

        public List<OsobyListItem> OsobyPrawneListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                var statusyQuery =
                from klient in db.Klienci where !klient.CzyFizyczna
                join ocena in db.Oceny on klient.KlientId equals ocena.KlientId 
                join status in db.Statusy on  ocena.StatusId equals status.StatusId
                select new OsobyListItem()
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
                foreach (OsobyListItem osobyListItem in filtered)
                {
                    foreach (OsobyListItem listItem in filtered)
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

        public List<OsobyListItem> OsobyFizyczneListQuery()
        {
            using (var db = new OcenaKlientowContext())
            {
                var statusyQuery =
                from klient in db.Klienci
                where klient.CzyFizyczna 
                join ocena in db.Oceny on klient.KlientId equals ocena.KlientId
                join status in db.Statusy on ocena.StatusId equals status.StatusId
                select new OsobyListItem()
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
                foreach (OsobyListItem osobyListItem in filtered)
                {
                    foreach (OsobyListItem listItem in filtered)
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

        public void CountStatus(Klient klient)
        {
            AssignGrade(klient);
            
        }

        public void CoundAllGrades(List<Klient> listaKlients)
        {
            foreach (Klient listaKlient in listaKlients)
            {
                CountStatus(listaKlient);
            }
        }

        public Klient GetKlient(int klientId)
        {
            using (var db =new OcenaKlientowContext())
            {
                return db.Klienci.Where(klient => klient.KlientId == klientId).FirstOrDefault();
            }
        }

        public void GetGradeDetails(OsobyListItem item)
        {
            using (var db = new OcenaKlientowContext())
            {
                var details = db.Wyliczono.Where(ocena => ocena.OcenaId == item.OcenaId).ToList();
                var detailsQuery =
               from wyliczenie in db.Wyliczono
               join ocena in db.Oceny on wyliczenie.OcenaId equals ocena.OcenaId
               join klient in db.Klienci on ocena.KlientId equals item.KlientId
               join par in db.Parametry on wyliczenie.ParametrId equals par.ParametrId
               select new
               {
                   SumaPkt = wyliczenie.WartoscWyliczona, Kredyt = klient.KwotaKredytu,
                   NazwaPar = par.Nazwa, WartoscPar= par.Wartosc, klient.KlientId, ocena.OcenaId
               };
                var lista = detailsQuery.ToList();
                lista =lista.Where(arg => arg.KlientId == item.KlientId && arg.OcenaId == item.OcenaId).ToList();
            }
        }
    }

}
