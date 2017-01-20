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
    public class OcenaViewModel
    {
        private readonly CultureInfo culture = new CultureInfo("pt-BR");

        private bool _saving;

        public OcenaViewModel(bool saving)
        {
            Saving = saving;
        }

        public bool Saving
        {
            get
            {
                return _saving;
            }
            set
            {
                _saving = value;
            }
        }

        public List<GradeDetails> GetGradeDetails(KlientView item)
        {
            using (var db = new OcenaKlientowContext())
            {
                //var details = db.Wyliczono.Where(ocena => ocena.OcenaId == item.OcenaId).ToList();
                var detailsQuery =
               from wyliczenie in db.Wyliczono
               join ocena in db.Oceny on wyliczenie.OcenaId equals ocena.OcenaId
               join klient in db.Klienci on ocena.KlientId equals item.KlientId
               join par in db.Parametry on wyliczenie.ParametrId equals par.ParametrId
               select new GradeDetails()
               {
                   Klient = new Klient()
                   {
                       KlientId = klient.KlientId,
                       
                   },
                   Parametr = new Parametr()
                   {
                       Nazwa = par.Nazwa,
                       ParametrId = par.ParametrId,
                       Wartosc = par.Wartosc
                   },
                   Ocena = new Ocena()
                   {
                       KlientId = klient.KlientId,
                       OcenaId = ocena.OcenaId
                   },
                   SumaPkt = wyliczenie.WartoscWyliczona
               };
                var lista = detailsQuery.ToList();
                lista = lista.Where(grDet => grDet.Klient.KlientId == item.KlientId && grDet.Ocena.OcenaId == item.OcenaId).ToList();
                return lista;
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


        private void AssignGrade(Klient klient)
        {
            Dictionary<int, int> parameters = new Dictionary<int, int>();
            int sum = 0;
            Ocena lastOcena;
            using (var db = new OcenaKlientowContext())
            {
                var parId = db.Parametry.Where(parametr => parametr.Nazwa == "LIMIT KREDYTU").Select(parametr => parametr.ParametrId).FirstOrDefault();
                var points = LoanLimit(klient);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "REGUL_ZAM").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = RegularOrders(klient);
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
                int statusId = -1;
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
                if (Saving)
                {
                    db.SaveChanges();
                }
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
                    if (Saving)
                    {
                        db.SaveChanges();
                    }
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

        public void CountStatus(Klient klient)
        {
            AssignGrade(klient);
        }

        public void CountAllGrades()
        {
            List<Klient> listaKlients;

            using (var db = new OcenaKlientowContext())
            {
                listaKlients = db.Klienci.ToList();
            }
            foreach (Klient listaKlient in listaKlients)
            {
                CountStatus(listaKlient);
            }
        }

    }

    public class GradeDetails
    {
        public Klient Klient { get; set; }

        public Ocena Ocena { get; set; }

        public Parametr Parametr { get; set; }

        public Wyliczenie Wyliczenie { get; set; }

        public double SumaPkt { get; set; }
    }
}