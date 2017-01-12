using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;

namespace OcenaKlientow.ViewModel
{
    public class Pu2ViewModel
    {
        CultureInfo culture = new CultureInfo("pt-BR");

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
                var firstOrder = zam[0];
                foreach (Zamowienie zamowienie in zam)
                {
                    var firstOrderDate = DateTime.Parse(firstOrder.DataZamowienia, culture);
                    if (firstOrder.ZamowienieId == zamowienie.ZamowienieId)
                    {
                        continue;
                    }
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
                if (sumOfPayments >= klient?.KwotaKredytu)
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
                CultureInfo culture = new CultureInfo("pt-BR");
                var a = DateTime.Now.ToString("d", culture);
                db.Oceny.Add(new Ocena()
                {
                    //formatkA!!!!
                    DataCzas = DateTime.Now.ToString(),
                    KlientId = klient.KlientId,
                    StatusId = 1, //zrobic!
                    SumaPkt = sum
                });
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
    }

}
