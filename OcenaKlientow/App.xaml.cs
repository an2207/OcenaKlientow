using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View;
using OcenaKlientow.View.ListItems;

namespace OcenaKlientow
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
            using (var db = new OcenaKlientowContext())
            {
                db.Database.Migrate();
                //var a = RegularOrders();
                //SeedData(db);
                // InitData(db);
                //var A = DateTime.Parse(db.Oceny.ToList().FirstOrDefault().DataCzas);
                //var test = db.Platnosci.ToList();


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
                CultureInfo culture = new CultureInfo("pt-BR");
                var a = DateTime.Now.ToString("d", culture);
                string b = "12/01/2015";
                var c = DateTime.Parse(b, culture);
                //var temp = db.Zamowienia.Select(zamowienie => zamowienie.Klient.Imie).ToList();


                //var asd = db.Zamowienia.Select(zamowienie => new
                //                               {
                //                                   ProductName = zamowienie.Klient.Imie,
                //                                   Category = zamowienie.Kwota
                //                               }).ToList();

                //var statusyQuery =
                //from benefit in db.Benefity
                //join pstatus in db.PrzypisaneStatusy on benefit.BenefitId equals pstatus.BenefitId
                //join status in db.Statusy on pstatus.StatusId equals status.StatusId
                //select new { benefitNazwa = benefit.Nazwa, NazwaStatusu = status.Nazwa };
                ;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void InitData(OcenaKlientowContext db)
        {

            db.Klienci.Add(new Klient()
            {
                CzyFizyczna = false,
                Nazwa = "Leffler Group",
                NIP = "721-46-77-8810",
                KwotaKredytu = 36645
            }); db.SaveChanges();
            db.Klienci.Add(new Klient()
            {
                CzyFizyczna = true,
                Nazwa = "Leffler Group",
                Imie = "Laurencja",
                DrugieImie = "Marta",
                Nazwisko = "Gazdowska",
                DrugieNazwisko = "Litkowska",
                PESEL = "70090509716"
            }); db.SaveChanges();

            //db.Statusy.Add(new Status()
            //{
            //    Nazwa = "Zielony",
            //    ProgDolny = 40,
            //    ProgGorny = 59
            //});

            db.Oceny.Add(new Ocena()
                             {
                                 KlientId = 2,
                                 DataCzas = "2016/11/21",
                                 StatusId = 1
                             }); db.SaveChanges();


                db.SaveChanges();

                var client = db.Klienci.Where(klient => klient.DrugieImie == "Marta");
                var status = db.Statusy.Where(status1 => status1.Nazwa == "Zielony");
                db.Oceny.Where(ocena => ocena.KlientId == client.FirstOrDefault().KlientId && ocena.StatusId == status.FirstOrDefault().StatusId).FirstOrDefault().SumaPkt=200;
                

            
        }

        //void SeedData(OcenaKlientowContext db)
        //{
        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Antoni",
        //        Nazwisko = "Hoy",
        //        PESEL = "81070404428"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Schroeder, Stehr and Schuppe",
        //        NIP = "465-85-14-484",
        //        KwotaKredytu = 49060
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "McDermott-Senger",
        //        NIP = "582-32-28-449",
        //        KwotaKredytu = 60610
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Lubowitz and Sons",
        //        NIP = "211-92-96-694",
        //        KwotaKredytu = 39129
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Bergnaum-McClure",
        //        NIP = "451-37-11-398",
        //        KwotaKredytu = 12625
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Witalis",
        //        Nazwisko = "Diedlaczek",
        //        PESEL = "54121402991"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Bayer-Wiza",
        //        NIP = "475-31-73-813",
        //        KwotaKredytu = 71913
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Amanda",
        //        DrugieImie = "Krystyna",
        //        Nazwisko = "Zak�lski",
        //        DrugieNazwisko = "Pryszczepko",
        //        PESEL = "70090509716"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Buckridge Inc",
        //        NIP = "665-64-93-313",
        //        KwotaKredytu = 10584
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Sporer Inc",
        //        NIP = "772-92-54-751",
        //        KwotaKredytu = 25954
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Radomi�a",
        //        DrugieImie = "Erazma",
        //        Nazwisko = "Karsa",
        //        DrugieNazwisko = "Sartowicz",
        //        PESEL = "69071121939"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Walsh-Wolff",
        //        NIP = "412-39-87-744",
        //        KwotaKredytu = 82560
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Kamila",
        //        DrugieImie = "Madlena",
        //        Nazwisko = "Huncia",
        //        DrugieNazwisko = "Czu�daniuk",
        //        PESEL = "68082410021"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Ligia",
        //        DrugieImie = "Bazylissa",
        //        Nazwisko = "�odygo",
        //        DrugieNazwisko = "Seraczyk",
        //        PESEL = "74042822755"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Jozue",
        //        Nazwisko = "Pusklak",
        //        PESEL = "75030320985"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Walsh-Wolff",
        //        NIP = "907-15-25-772",
        //        KwotaKredytu = 50273
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Tadeusz",
        //        Nazwisko = "Wojcielo�",
        //        PESEL = "53020202097"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Kshlerin Group",
        //        NIP = "275-99-19-810",
        //        KwotaKredytu = 46702
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = false,
        //        Nazwa = "Bartoletti-Schmitt",
        //        NIP = "583-55-23-792",
        //        KwotaKredytu = 76974
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Brunon",
        //        Nazwisko = "Swierszulska",
        //        PESEL = "64101409773"
        //    }); db.SaveChanges();

        //    db.Klienci.Add(new Klient()
        //    {
        //        CzyFizyczna = true,
        //        Imie = "Gaudenty",
        //        Nazwisko = "Pastro�ny",
        //        PESEL = "55061216336"
        //    }); db.SaveChanges();
        //    db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/03/12",
        //        KlientId = 14,
        //        Kwota = 424.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/05",
        //        DataZaplaty = "2016/02/06",
        //        Kwota = 424.0,
        //        ZamowienieId = 1
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/11/12",
        //        KlientId = 21,
        //        Kwota = 1579.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/06/14",
        //        DataZaplaty = "2015/07/28",
        //        Kwota = 394.75,
        //        ZamowienieId = 2
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/13",
        //        DataZaplaty = "2015/12/31",
        //        Kwota = 394.75,
        //        ZamowienieId = 2
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/01",
        //        DataZaplaty = "2016/03/08",
        //        Kwota = 394.75,
        //        ZamowienieId = 2
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/30",
        //        DataZaplaty = "2016/01/24",
        //        Kwota = 394.75,
        //        ZamowienieId = 2
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/09/11",
        //        KlientId = 4,
        //        Kwota = 9854.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/17",
        //        DataZaplaty = "2016/01/30",
        //        Kwota = 4927.0,
        //        ZamowienieId = 3
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/08/14",
        //        DataZaplaty = "2015/11/25",
        //        Kwota = 4927.0,
        //        ZamowienieId = 3
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/30",
        //        KlientId = 9,
        //        Kwota = 24.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/30",
        //        DataZaplaty = "2016/12/22",
        //        Kwota = 6.0,
        //        ZamowienieId = 4
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/24",
        //        DataZaplaty = "2015/11/12",
        //        Kwota = 6.0,
        //        ZamowienieId = 4
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/28",
        //        DataZaplaty = "2015/05/28",
        //        Kwota = 6.0,
        //        ZamowienieId = 4
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/30",
        //        DataZaplaty = "2015/11/16",
        //        Kwota = 6.0,
        //        ZamowienieId = 4
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/10/02",
        //        KlientId = 10,
        //        Kwota = 8501.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/24",
        //        DataZaplaty = "2016/10/22",
        //        Kwota = 2125.25,
        //        ZamowienieId = 5
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/24",
        //        DataZaplaty = "2015/02/24",
        //        Kwota = 2125.25,
        //        ZamowienieId = 5
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/28",
        //        DataZaplaty = "2016/01/30",
        //        Kwota = 2125.25,
        //        ZamowienieId = 5
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/10/02",
        //        DataZaplaty = "2016/08/31",
        //        Kwota = 2125.25,
        //        ZamowienieId = 5
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/11/03",
        //        KlientId = 3,
        //        Kwota = 7093.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/12",
        //        DataZaplaty = "2015/12/05",
        //        Kwota = 1418.6,
        //        ZamowienieId = 6
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/05",
        //        DataZaplaty = "2015/03/31",
        //        Kwota = 1418.6,
        //        ZamowienieId = 6
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/04/30",
        //        DataZaplaty = "2016/08/31",
        //        Kwota = 1418.6,
        //        ZamowienieId = 6
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/12",
        //        DataZaplaty = "2016/09/13",
        //        Kwota = 1418.6,
        //        ZamowienieId = 6
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/12",
        //        DataZaplaty = "2015/08/22",
        //        Kwota = 1418.6,
        //        ZamowienieId = 6
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/12/08",
        //        KlientId = 20,
        //        Kwota = 9278.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/14",
        //        DataZaplaty = "2016/01/08",
        //        Kwota = 3092.67,
        //        ZamowienieId = 7
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/19",
        //        DataZaplaty = "2015/08/31",
        //        Kwota = 3092.67,
        //        ZamowienieId = 7
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/08/14",
        //        DataZaplaty = "2016/01/01",
        //        Kwota = 3092.67,
        //        ZamowienieId = 7
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/09/13",
        //        KlientId = 8,
        //        Kwota = 751.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/30",
        //        DataZaplaty = "2015/09/06",
        //        Kwota = 751.0,
        //        ZamowienieId = 8
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/03/15",
        //        KlientId = 13,
        //        Kwota = 1757.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/01",
        //        DataZaplaty = "2015/09/10",
        //        Kwota = 878.5,
        //        ZamowienieId = 9
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/10/13",
        //        DataZaplaty = "2016/04/30",
        //        Kwota = 878.5,
        //        ZamowienieId = 9
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/01/30",
        //        KlientId = 15,
        //        Kwota = 8349.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/08/11",
        //        DataZaplaty = "2016/06/18",
        //        Kwota = 2087.25,
        //        ZamowienieId = 10
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/24",
        //        DataZaplaty = "2015/12/05",
        //        Kwota = 2087.25,
        //        ZamowienieId = 10
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/31",
        //        DataZaplaty = "2016/09/11",
        //        Kwota = 2087.25,
        //        ZamowienieId = 10
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/28",
        //        DataZaplaty = "2016/12/22",
        //        Kwota = 2087.25,
        //        ZamowienieId = 10
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/02/17",
        //        KlientId = 16,
        //        Kwota = 4233.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/02",
        //        DataZaplaty = "2015/03/10",
        //        Kwota = 4233.0,
        //        ZamowienieId = 11
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/19",
        //        KlientId = 15,
        //        Kwota = 367.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/17",
        //        DataZaplaty = "2016/09/13",
        //        Kwota = 122.33,
        //        ZamowienieId = 12
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/11/12",
        //        DataZaplaty = "2016/05/21",
        //        Kwota = 122.33,
        //        ZamowienieId = 12
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/11/12",
        //        DataZaplaty = "2016/05/25",
        //        Kwota = 122.33,
        //        ZamowienieId = 12
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/01/27",
        //        KlientId = 6,
        //        Kwota = 7835.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/04/12",
        //        DataZaplaty = "2015/11/25",
        //        Kwota = 1567.0,
        //        ZamowienieId = 13
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/12",
        //        DataZaplaty = "2015/03/04",
        //        Kwota = 1567.0,
        //        ZamowienieId = 13
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/10/21",
        //        DataZaplaty = "2015/11/02",
        //        Kwota = 1567.0,
        //        ZamowienieId = 13
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/24",
        //        DataZaplaty = "2016/04/17",
        //        Kwota = 1567.0,
        //        ZamowienieId = 13
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/07/09",
        //        DataZaplaty = "2015/03/25",
        //        Kwota = 1567.0,
        //        ZamowienieId = 13
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/31",
        //        KlientId = 5,
        //        Kwota = 2283.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/12/22",
        //        DataZaplaty = "2016/08/14",
        //        Kwota = 761.0,
        //        ZamowienieId = 14
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/06/23",
        //        DataZaplaty = "2015/08/30",
        //        Kwota = 761.0,
        //        ZamowienieId = 14
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/08/07",
        //        DataZaplaty = "2016/10/22",
        //        Kwota = 761.0,
        //        ZamowienieId = 14
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/24",
        //        KlientId = 6,
        //        Kwota = 4650.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/10/13",
        //        DataZaplaty = "2016/01/30",
        //        Kwota = 930.0,
        //        ZamowienieId = 15
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/07/09",
        //        DataZaplaty = "2016/10/22",
        //        Kwota = 930.0,
        //        ZamowienieId = 15
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/14",
        //        DataZaplaty = "2015/03/10",
        //        Kwota = 930.0,
        //        ZamowienieId = 15
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/04/24",
        //        DataZaplaty = "2016/06/18",
        //        Kwota = 930.0,
        //        ZamowienieId = 15
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/30",
        //        DataZaplaty = "2016/08/20",
        //        Kwota = 930.0,
        //        ZamowienieId = 15
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/12/03",
        //        KlientId = 13,
        //        Kwota = 9379.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/31",
        //        DataZaplaty = "2016/01/01",
        //        Kwota = 9379.0,
        //        ZamowienieId = 16
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/02/01",
        //        KlientId = 2,
        //        Kwota = 3258.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/28",
        //        DataZaplaty = "2016/08/14",
        //        Kwota = 1629.0,
        //        ZamowienieId = 17
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/25",
        //        DataZaplaty = "2016/09/16",
        //        Kwota = 1629.0,
        //        ZamowienieId = 17
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/09/16",
        //        KlientId = 16,
        //        Kwota = 8313.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/09/11",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 1662.6,
        //        ZamowienieId = 18
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/01",
        //        DataZaplaty = "2015/08/30",
        //        Kwota = 1662.6,
        //        ZamowienieId = 18
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/09/13",
        //        DataZaplaty = "2016/12/07",
        //        Kwota = 1662.6,
        //        ZamowienieId = 18
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/11/02",
        //        DataZaplaty = "2016/09/13",
        //        Kwota = 1662.6,
        //        ZamowienieId = 18
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/01/16",
        //        DataZaplaty = "2015/06/14",
        //        Kwota = 1662.6,
        //        ZamowienieId = 18
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/06/23",
        //        KlientId = 10,
        //        Kwota = 479.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/07",
        //        DataZaplaty = "2015/11/16",
        //        Kwota = 159.67,
        //        ZamowienieId = 19
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/28",
        //        DataZaplaty = "2015/08/22",
        //        Kwota = 159.67,
        //        ZamowienieId = 19
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/17",
        //        DataZaplaty = "2016/01/24",
        //        Kwota = 159.67,
        //        ZamowienieId = 19
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/30",
        //        KlientId = 15,
        //        Kwota = 7868.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/28",
        //        DataZaplaty = "2016/01/24",
        //        Kwota = 7868.0,
        //        ZamowienieId = 20
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/31",
        //        KlientId = 6,
        //        Kwota = 5774.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/09",
        //        DataZaplaty = "2015/12/17",
        //        Kwota = 2887.0,
        //        ZamowienieId = 21
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/24",
        //        DataZaplaty = "2015/11/03",
        //        Kwota = 2887.0,
        //        ZamowienieId = 21
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/09/06",
        //        KlientId = 4,
        //        Kwota = 4159.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/14",
        //        DataZaplaty = "2016/01/24",
        //        Kwota = 2079.5,
        //        ZamowienieId = 22
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/04/12",
        //        DataZaplaty = "2016/08/11",
        //        Kwota = 2079.5,
        //        ZamowienieId = 22
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/04/12",
        //        KlientId = 4,
        //        Kwota = 3016.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/25",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 3016.0,
        //        ZamowienieId = 23
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/05/13",
        //        KlientId = 19,
        //        Kwota = 766.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/31",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 766.0,
        //        ZamowienieId = 24
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/03/12",
        //        KlientId = 10,
        //        Kwota = 7315.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/25",
        //        DataZaplaty = "2016/08/31",
        //        Kwota = 2438.33,
        //        ZamowienieId = 25
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/11/02",
        //        DataZaplaty = "2016/06/23",
        //        Kwota = 2438.33,
        //        ZamowienieId = 25
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/06/20",
        //        DataZaplaty = "2015/11/16",
        //        Kwota = 2438.33,
        //        ZamowienieId = 25
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/12/07",
        //        KlientId = 18,
        //        Kwota = 6891.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/09/14",
        //        DataZaplaty = "2015/02/11",
        //        Kwota = 3445.5,
        //        ZamowienieId = 26
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/07",
        //        DataZaplaty = "2016/01/30",
        //        Kwota = 3445.5,
        //        ZamowienieId = 26
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/11/02",
        //        KlientId = 21,
        //        Kwota = 8141.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/30",
        //        DataZaplaty = "2016/07/09",
        //        Kwota = 4070.5,
        //        ZamowienieId = 27
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/01",
        //        DataZaplaty = "2015/09/12",
        //        Kwota = 4070.5,
        //        ZamowienieId = 27
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/11/03",
        //        KlientId = 1,
        //        Kwota = 9603.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/11/03",
        //        DataZaplaty = "2015/08/20",
        //        Kwota = 1920.6,
        //        ZamowienieId = 28
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/06",
        //        DataZaplaty = "2016/08/20",
        //        Kwota = 1920.6,
        //        ZamowienieId = 28
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/30",
        //        DataZaplaty = "2016/06/18",
        //        Kwota = 1920.6,
        //        ZamowienieId = 28
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/02/24",
        //        DataZaplaty = "2015/11/16",
        //        Kwota = 1920.6,
        //        ZamowienieId = 28
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/28",
        //        DataZaplaty = "2015/03/31",
        //        Kwota = 1920.6,
        //        ZamowienieId = 28
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/24",
        //        KlientId = 13,
        //        Kwota = 2030.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/06/14",
        //        DataZaplaty = "2016/09/11",
        //        Kwota = 406.0,
        //        ZamowienieId = 29
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/21",
        //        DataZaplaty = "2015/09/10",
        //        Kwota = 406.0,
        //        ZamowienieId = 29
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/11/03",
        //        DataZaplaty = "2015/12/17",
        //        Kwota = 406.0,
        //        ZamowienieId = 29
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/10/02",
        //        DataZaplaty = "2016/04/17",
        //        Kwota = 406.0,
        //        ZamowienieId = 29
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/21",
        //        DataZaplaty = "2016/06/18",
        //        Kwota = 406.0,
        //        ZamowienieId = 29
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/12/08",
        //        KlientId = 8,
        //        Kwota = 5986.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/11/03",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 1496.5,
        //        ZamowienieId = 30
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/30",
        //        DataZaplaty = "2015/04/04",
        //        Kwota = 1496.5,
        //        ZamowienieId = 30
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/06/23",
        //        DataZaplaty = "2015/11/16",
        //        Kwota = 1496.5,
        //        ZamowienieId = 30
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/05",
        //        DataZaplaty = "2015/08/31",
        //        Kwota = 1496.5,
        //        ZamowienieId = 30
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/08/31",
        //        KlientId = 10,
        //        Kwota = 2649.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/10",
        //        DataZaplaty = "2015/05/28",
        //        Kwota = 662.25,
        //        ZamowienieId = 31
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/01/27",
        //        DataZaplaty = "2015/09/10",
        //        Kwota = 662.25,
        //        ZamowienieId = 31
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/19",
        //        DataZaplaty = "2016/04/24",
        //        Kwota = 662.25,
        //        ZamowienieId = 31
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/20",
        //        DataZaplaty = "2015/08/30",
        //        Kwota = 662.25,
        //        ZamowienieId = 31
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/04/12",
        //        KlientId = 15,
        //        Kwota = 6140.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/27",
        //        DataZaplaty = "2015/08/20",
        //        Kwota = 1535.0,
        //        ZamowienieId = 32
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/12/18",
        //        DataZaplaty = "2016/04/30",
        //        Kwota = 1535.0,
        //        ZamowienieId = 32
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/10",
        //        DataZaplaty = "2016/11/03",
        //        Kwota = 1535.0,
        //        ZamowienieId = 32
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/03",
        //        DataZaplaty = "2015/08/20",
        //        Kwota = 1535.0,
        //        ZamowienieId = 32
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/09/24",
        //        KlientId = 6,
        //        Kwota = 7343.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/17",
        //        DataZaplaty = "2015/01/16",
        //        Kwota = 3671.5,
        //        ZamowienieId = 33
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/10",
        //        DataZaplaty = "2016/05/13",
        //        Kwota = 3671.5,
        //        ZamowienieId = 33
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/08/14",
        //        KlientId = 19,
        //        Kwota = 9014.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/10",
        //        DataZaplaty = "2015/08/09",
        //        Kwota = 4507.0,
        //        ZamowienieId = 34
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/10",
        //        DataZaplaty = "2015/07/28",
        //        Kwota = 4507.0,
        //        ZamowienieId = 34
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/01/27",
        //        KlientId = 5,
        //        Kwota = 3359.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/07",
        //        DataZaplaty = "2016/04/30",
        //        Kwota = 839.75,
        //        ZamowienieId = 35
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/17",
        //        DataZaplaty = "2015/11/02",
        //        Kwota = 839.75,
        //        ZamowienieId = 35
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/14",
        //        DataZaplaty = "2015/07/28",
        //        Kwota = 839.75,
        //        ZamowienieId = 35
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/04/04",
        //        DataZaplaty = "2016/10/21",
        //        Kwota = 839.75,
        //        ZamowienieId = 35
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/06/14",
        //        KlientId = 20,
        //        Kwota = 3125.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/13",
        //        DataZaplaty = "2016/12/03",
        //        Kwota = 1041.67,
        //        ZamowienieId = 36
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/11/03",
        //        DataZaplaty = "2015/08/02",
        //        Kwota = 1041.67,
        //        ZamowienieId = 36
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/17",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 1041.67,
        //        ZamowienieId = 36
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/06/23",
        //        KlientId = 3,
        //        Kwota = 7.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/08",
        //        DataZaplaty = "2016/09/14",
        //        Kwota = 3.5,
        //        ZamowienieId = 37
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/03/05",
        //        DataZaplaty = "2016/12/03",
        //        Kwota = 3.5,
        //        ZamowienieId = 37
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/08/20",
        //        KlientId = 18,
        //        Kwota = 8649.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/08",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 2883.0,
        //        ZamowienieId = 38
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/08",
        //        DataZaplaty = "2015/08/18",
        //        Kwota = 2883.0,
        //        ZamowienieId = 38
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/01/08",
        //        DataZaplaty = "2016/12/18",
        //        Kwota = 2883.0,
        //        ZamowienieId = 38
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/01/30",
        //        KlientId = 20,
        //        Kwota = 8994.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/01/27",
        //        DataZaplaty = "2016/03/07",
        //        Kwota = 8994.0,
        //        ZamowienieId = 39
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/12/05",
        //        KlientId = 20,
        //        Kwota = 3076.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/30",
        //        DataZaplaty = "2016/04/24",
        //        Kwota = 3076.0,
        //        ZamowienieId = 40
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/10/21",
        //        KlientId = 12,
        //        Kwota = 6568.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/09/14",
        //        DataZaplaty = "2016/12/03",
        //        Kwota = 2189.33,
        //        ZamowienieId = 41
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/08/11",
        //        DataZaplaty = "2015/08/30",
        //        Kwota = 2189.33,
        //        ZamowienieId = 41
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/08",
        //        DataZaplaty = "2015/08/24",
        //        Kwota = 2189.33,
        //        ZamowienieId = 41
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/09/13",
        //        KlientId = 16,
        //        Kwota = 1423.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/04/17",
        //        DataZaplaty = "2015/09/30",
        //        Kwota = 711.5,
        //        ZamowienieId = 42
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/03",
        //        DataZaplaty = "2016/06/22",
        //        Kwota = 711.5,
        //        ZamowienieId = 42
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/01/01",
        //        KlientId = 15,
        //        Kwota = 355.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/31",
        //        DataZaplaty = "2015/08/31",
        //        Kwota = 88.75,
        //        ZamowienieId = 43
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/12",
        //        DataZaplaty = "2015/05/28",
        //        Kwota = 88.75,
        //        ZamowienieId = 43
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/02/01",
        //        DataZaplaty = "2015/09/30",
        //        Kwota = 88.75,
        //        ZamowienieId = 43
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/04/12",
        //        DataZaplaty = "2016/11/03",
        //        Kwota = 88.75,
        //        ZamowienieId = 43
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/10/16",
        //        KlientId = 5,
        //        Kwota = 2125.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/20",
        //        DataZaplaty = "2015/02/24",
        //        Kwota = 2125.0,
        //        ZamowienieId = 44
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/05/01",
        //        KlientId = 14,
        //        Kwota = 4918.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/20",
        //        DataZaplaty = "2015/11/12",
        //        Kwota = 1229.5,
        //        ZamowienieId = 45
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/03/07",
        //        DataZaplaty = "2015/09/24",
        //        Kwota = 1229.5,
        //        ZamowienieId = 45
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/01",
        //        DataZaplaty = "2015/03/25",
        //        Kwota = 1229.5,
        //        ZamowienieId = 45
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/07/09",
        //        DataZaplaty = "2015/08/28",
        //        Kwota = 1229.5,
        //        ZamowienieId = 45
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/09/30",
        //        KlientId = 12,
        //        Kwota = 2517.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/04/04",
        //        DataZaplaty = "2015/02/05",
        //        Kwota = 2517.0,
        //        ZamowienieId = 46
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/09/13",
        //        KlientId = 3,
        //        Kwota = 6988.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/24",
        //        DataZaplaty = "2016/12/18",
        //        Kwota = 1397.6,
        //        ZamowienieId = 47
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/01",
        //        DataZaplaty = "2015/11/25",
        //        Kwota = 1397.6,
        //        ZamowienieId = 47
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/08/11",
        //        DataZaplaty = "2015/08/30",
        //        Kwota = 1397.6,
        //        ZamowienieId = 47
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/07/28",
        //        DataZaplaty = "2015/08/19",
        //        Kwota = 1397.6,
        //        ZamowienieId = 47
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/01/16",
        //        DataZaplaty = "2015/08/18",
        //        Kwota = 1397.6,
        //        ZamowienieId = 47
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/01/16",
        //        KlientId = 5,
        //        Kwota = 9470.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/11/03",
        //        DataZaplaty = "2016/12/07",
        //        Kwota = 9470.0,
        //        ZamowienieId = 48
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2015/02/01",
        //        KlientId = 10,
        //        Kwota = 4166.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/08/19",
        //        DataZaplaty = "2016/06/22",
        //        Kwota = 2083.0,
        //        ZamowienieId = 49
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/12/03",
        //        DataZaplaty = "2016/08/20",
        //        Kwota = 2083.0,
        //        ZamowienieId = 49
        //    }); db.SaveChanges();

        //    db.Zamowienia.Add(new Zamowienie()
        //    {
        //        CzyZweryfikowano = true,
        //        DataZamowienia = "2016/01/30",
        //        KlientId = 3,
        //        Kwota = 241.0
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/05/28",
        //        DataZaplaty = "2016/09/11",
        //        Kwota = 60.25,
        //        ZamowienieId = 50
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/09/24",
        //        DataZaplaty = "2016/09/16",
        //        Kwota = 60.25,
        //        ZamowienieId = 50
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2016/05/21",
        //        DataZaplaty = "2016/06/23",
        //        Kwota = 60.25,
        //        ZamowienieId = 50
        //    }); db.SaveChanges();

        //    db.Platnosci.Add(new Platnosc()
        //    {
        //        DataWymag = "2015/10/02",
        //        DataZaplaty = "2016/03/12",
        //        Kwota = 60.25,
        //        ZamowienieId = 50
        //    }); db.SaveChanges();

        //    db.RodzajeBenefitow.Add(new RodzajBenefitu()
        //    {
        //        RodzajId = 1,
        //        Nazwa = "Wydłużony termin"
        //    }); db.SaveChanges();

        //    db.RodzajeBenefitow.Add(new RodzajBenefitu()
        //    {
        //        RodzajId = 2,
        //        Nazwa = "Rabat"
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/09/10",
        //        DataZakon = "2016/10/22",
        //        Nazwa = "Benefit 1",
        //        RodzajId = 2,
        //        WartoscProc = 51.0
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/08/28",
        //        DataZakon = "2016/10/04",
        //        Nazwa = "Benefit 2",
        //        RodzajId = 2,
        //        WartoscProc = 64.0
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/05/28",
        //        DataZakon = "2015/06/23",
        //        LiczbaDni = 19,
        //        Nazwa = "Benefit 3",
        //        RodzajId = 1
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/10/13",
        //        DataZakon = "2016/06/18",
        //        LiczbaDni = 87,
        //        Nazwa = "Benefit 4",
        //        RodzajId = 1
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/06/20",
        //        DataZakon = "2016/10/26",
        //        LiczbaDni = 61,
        //        Nazwa = "Benefit 5",
        //        RodzajId = 1
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2016/05/21",
        //        DataZakon = "2016/10/22",
        //        Nazwa = "Benefit 6",
        //        RodzajId = 2,
        //        WartoscProc = 14.0
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2016/01/08",
        //        DataZakon = "2016/02/24",
        //        LiczbaDni = 74,
        //        Nazwa = "Benefit 7",
        //        RodzajId = 1
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/04/12",
        //        DataZakon = "2015/09/12",
        //        Nazwa = "Benefit 8",
        //        RodzajId = 2,
        //        WartoscProc = 34.0
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2015/11/25",
        //        DataZakon = "2016/01/24",
        //        Nazwa = "Benefit 9",
        //        RodzajId = 2,
        //        WartoscProc = 59.0
        //    }); db.SaveChanges();

        //    db.Benefity.Add(new Benefit()
        //    {
        //        DataUaktyw = "2016/01/08",
        //        DataZakon = "2016/03/12",
        //        LiczbaDni = 63,
        //        Nazwa = "Benefit 10",
        //        RodzajId = 1
        //    }); db.SaveChanges();

        //    db.Parametry.Add(new Parametr()
        //    {
        //        Nazwa = "PLATN_NA_CZAS",
        //        Wartosc = 15
        //    }); db.SaveChanges();

        //    db.Parametry.Add(new Parametr()
        //    {
        //        Nazwa = "PLATN_CZESC",
        //        Wartosc = 5
        //    }); db.SaveChanges();

        //    db.Parametry.Add(new Parametr()
        //    {
        //        Nazwa = "PLATN_CALK",
        //        Wartosc = 10
        //    }); db.SaveChanges();

        //    db.Parametry.Add(new Parametr()
        //    {
        //        Nazwa = "REGUL_ZAM",
        //        Wartosc = 15
        //    }); db.SaveChanges();

        //    db.Parametry.Add(new Parametr()
        //    {
        //        Nazwa = "PRZEK_TERM",
        //        Wartosc = -2
        //    }); db.SaveChanges();

        //    db.Parametry.Add(new Parametr()
        //    {
        //        Nazwa = "LIMIT KREDYTU",
        //        Wartosc = -10
        //    }); db.SaveChanges();

        //    db.Statusy.Add(new Status()
        //    {
        //        Nazwa = "CZERWONY",
        //        ProgDolny = -2147483648,
        //        ProgGorny = -19
        //    }); db.SaveChanges();

        //    db.Statusy.Add(new Status()
        //    {
        //        Nazwa = "POMARAŃCZOWY",
        //        ProgDolny = -20,
        //        ProgGorny = -1
        //    }); db.SaveChanges();

        //    db.Statusy.Add(new Status()
        //    {
        //        Nazwa = "ŻÓŁTY",
        //        ProgDolny = 0,
        //        ProgGorny = 39
        //    }); db.SaveChanges();

        //    db.Statusy.Add(new Status()
        //    {
        //        Nazwa = "ZIELONY",
        //        ProgDolny = 40,
        //        ProgGorny = 59
        //    }); db.SaveChanges();

        //    db.Statusy.Add(new Status()
        //    {
        //        Nazwa = "ZŁOTY",
        //        ProgDolny = 60,
        //        ProgGorny = 2147483647
        //    }); db.SaveChanges();

        //    db.SaveChanges();
        //}
    }
}
