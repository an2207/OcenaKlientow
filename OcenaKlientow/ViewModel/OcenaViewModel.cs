using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;

namespace OcenaKlientow.ViewModel
{
    public class OcenaViewModel
    {
        #region Private fields

        private readonly CultureInfo culture = new CultureInfo("pt-BR");

        #endregion
        #region Properties

        public bool Saving { get; set; }

        #endregion
        #region Ctors

        public OcenaViewModel(bool saving)
        {
            Saving = saving;
        }

        #endregion
        #region Public methods

        public void CountAllGrades()
        {
            List<Klient> listaKlients;

            using (var db = new OcenaKlientowContext())
            {
                listaKlients = db.Klienci.ToList();
            }
            foreach (Klient listaKlient in listaKlients)
                CountStatus(listaKlient);
        }

        public void CountStatus(Klient klient)
        {
            AssignGrade(klient);
        }

        public List<GradeDetails> GetGradeDetails(KlientView item)
        {
            using (var db = new OcenaKlientowContext())
            {
                //var details = db.Wyliczono.Where(ocena => ocena.OcenaId == item.OcenaId).ToList();
                IQueryable<GradeDetails> detailsQuery =
                    from wyliczenie in db.Wyliczono
                    join ocena in db.Oceny on wyliczenie.OcenaId equals ocena.OcenaId
                    join klient in db.Klienci on ocena.KlientId equals item.KlientId
                    join par in db.Parametry on wyliczenie.ParametrId equals par.ParametrId
                    select new GradeDetails
                    {
                        Klient = new Klient
                        {
                            KlientId = klient.KlientId
                        },
                        Parametr = new Parametr
                        {
                            Nazwa = par.Nazwa,
                            ParametrId = par.ParametrId,
                            Wartosc = par.Wartosc
                        },
                        Ocena = new Ocena
                        {
                            KlientId = klient.KlientId,
                            OcenaId = ocena.OcenaId
                        },
                        SumaPkt = wyliczenie.WartoscWyliczona
                    };
                List<GradeDetails> lista = detailsQuery.ToList();
                lista = lista.Where(grDet => (grDet.Klient.KlientId == item.KlientId) && (grDet.Ocena.OcenaId == item.OcenaId)).ToList();
                return lista;
            }
        }

        #endregion
        #region Private methods

        int AfterDeadline(Klient klient, List<Zamowienie> listaZamowien, List<Platnosc> listaPlatnosci)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PRZEK_TERM").Select(parametr => parametr.Wartosc).FirstOrDefault();
                foreach (var zamowienie in listaZamowien)
                {
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (platnosc.ZamowienieId == zamowienie.ZamowienieId)
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
                }
                return sum;
            }
        }

        void AssignGrade(Klient klient)
        {
            Dictionary<int, int> parameters = new Dictionary<int, int>();
            int sum = 0;
            Ocena lastOcena;
            using (var db = new OcenaKlientowContext())
            {
                var listaZam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                List<Platnosc> listaPlat = new List<Platnosc>();
                foreach (Zamowienie zam in listaZam)
                {
                    var listaTemp = db.Platnosci.Where(platnosc => platnosc.ZamowienieId == zam.ZamowienieId);
                    listaPlat.AddRange(listaTemp);
                }
                var parId = db.Parametry.Where(parametr => parametr.Nazwa == "REGUL_ZAM").Select(parametr => parametr.ParametrId).FirstOrDefault();
                var points = RegularOrders(klient, listaZam);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "LIMIT KREDYTU").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = LoanLimit(klient, listaZam, listaPlat);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CZESC").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = PartialPayment(klient, listaZam, listaPlat);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CALK").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = FullPayment(klient, listaZam);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PRZEK_TERM").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = AfterDeadline(klient, listaZam, listaPlat);
                parameters.Add(parId, points);
                sum += points;
                parId = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_NA_CZAS").Select(parametr => parametr.ParametrId).FirstOrDefault();
                points = PaymentOnTime(klient, listaZam, listaPlat);
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

        private int Comparison(Zamowienie zamowienie, Zamowienie zamowienie1)
        {
            if (DateTime.Parse(zamowienie.DataZamowienia, culture) > DateTime.Parse(zamowienie1.DataZamowienia, culture))
                return 1;
            if (DateTime.Parse(zamowienie.DataZamowienia, culture) < DateTime.Parse(zamowienie1.DataZamowienia, culture))
                return -1;
            return 0;
        }

        int FullPayment(Klient klient, List<Zamowienie> listaZamowien)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CALK").Select(parametr => parametr.Wartosc).FirstOrDefault();
                var zam = db.Zamowienia.Where(zamowienie => zamowienie.KlientId == klient.KlientId).ToList();
                foreach (var zamowienie in listaZamowien)
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

        int LoanLimit(Klient klient, List<Zamowienie> listaZamowien, List<Platnosc> listaPlatnosci)
        {
            double sumOfPayments = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "LIMIT KREDYTU").Select(parametr => parametr.Wartosc).FirstOrDefault();
                foreach (var zamowienie in listaZamowien)
                {

                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (platnosc.ZamowienieId == zamowienie.ZamowienieId && platnosc.DataZaplaty == null)
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

        int PartialPayment(Klient klient, List<Zamowienie> listaZamowien, List<Platnosc> listaPlatnosci)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_CZESC").Select(parametr => parametr.Wartosc).FirstOrDefault();
                foreach (var zamowienie in listaZamowien)
                {
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if (platnosc.ZamowienieId == zamowienie.ZamowienieId)
                            if (DateTime.Parse(platnosc?.DataZaplaty, culture) < DateTime.Parse(platnosc?.DataWymag, culture))
                            {
                                sum += wart;
                                break;
                            }
                    }
                }
            }
            return sum;

        }

        int PaymentOnTime(Klient klient, List<Zamowienie> listaZamowien, List<Platnosc> listaPlatnosci)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "PLATN_NA_CZAS").Select(parametr => parametr.Wartosc).FirstOrDefault();
                foreach (var zamowienie in listaZamowien)
                {
                    sum += wart;
                    foreach (Platnosc platnosc in listaPlatnosci)
                    {
                        if ((platnosc.ZamowienieId == zamowienie.ZamowienieId) && !(DateTime.Parse(platnosc?.DataZaplaty, culture) < DateTime.Parse(platnosc?.DataWymag, culture)))
                        {
                            sum -= wart;
                            break;
                        }

                    }
                }
                return sum;
            }
        }

        int RegularOrders(Klient klient, List<Zamowienie> listaZamowien)
        {

            var sum = 0;
            using (var db = new OcenaKlientowContext())
            {
                var wart = db.Parametry.Where(parametr => parametr.Nazwa == "REGUL_ZAM").Select(parametr => parametr.Wartosc).FirstOrDefault();
                listaZamowien.Sort(Comparison);
                if (listaZamowien.Count == 0)
                    return 0;
                var firstOrder = listaZamowien[0];
                foreach (Zamowienie zamowienie in listaZamowien)
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

        #endregion
    }

    public class GradeDetails
    {
        #region Properties

        public Klient Klient { get; set; }

        public Ocena Ocena { get; set; }

        public Parametr Parametr { get; set; }

        public double SumaPkt { get; set; }

        public Wyliczenie Wyliczenie { get; set; }

        #endregion
    }
}