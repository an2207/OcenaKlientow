using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;
using OcenaKlientow.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OcenaKlientow.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PU2 : Page
    {

        private List<OsobyListItem> _prawneList;

        private List<OsobyListItem> _fizyczneList;

        private Pu2ViewModel _viewModel;

        private OsobyListItem selectedFromBothListItem;
        public PU2()
        {
            Pu2ViewModel = new Pu2ViewModel();
            Pu2ViewModel.OsobyPrawneListQuery();
            
            this.InitializeComponent();
            FizyczneList = Pu2ViewModel.OsobyFizyczneListQuery();
            PrawneList = Pu2ViewModel.OsobyPrawneListQuery();
            OsobyPrawne.ItemsSource = PrawneList;
            OsobyFizyczne.ItemsSource = FizyczneList;

            //_viewModel.CountStatus(currKlient);
            // _viewModel.CoundAllGrades(ListaKlients);

        }

        public List<OsobyListItem> PrawneList
        {
            get
            {
                return _prawneList;
            }
            set
            {
                _prawneList = value;
            }
        }

        public List<OsobyListItem> FizyczneList
        {
            get
            {
                return _fizyczneList;
            }
            set
            {
                _fizyczneList = value;
            }
        }

        public Pu2ViewModel Pu2ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        //void AddData()
        //{
        //    var k1 = new Klient()
        //    {
        //        KlientId = 1,
        //        CzyFizyczna = true,
        //        Nazwa = "Leffler Group",
        //        Imie = "Laurencja",
        //        DrugieImie = "Marta",
        //        Nazwisko = "Gazdowska",
        //        DrugieNazwisko = "Litkowska",
        //        PESEL = "70090509716"
        //    };

        //    var k4 = new Klient()
        //    {
        //        KlientId = 4,
        //        CzyFizyczna = true,
        //        Nazwa = "Leffler Group",
        //        Imie = "Laurencja",
        //        DrugieImie = "Marta",
        //        Nazwisko = "Gazdowska",
        //        DrugieNazwisko = "Litkowska",
        //        PESEL = "70090509716"
        //    };
        //    var k3 = new Klient()
        //    {
        //        KlientId = 3,
        //        CzyFizyczna = true,
        //        Nazwa = "Leffler Group",
        //        Imie = "Laurencja",
        //        DrugieImie = "Marta",
        //        Nazwisko = "Gazdowska",
        //        DrugieNazwisko = "Litkowska",
        //        PESEL = "70090509716"
        //    };
        //    var k2 = new Klient()
        //    {
        //        KlientId = 2,
        //        CzyFizyczna = true,
        //        Nazwa = "Leffler Group",
        //        Imie = "Laurencja",
        //        DrugieImie = "Marta",
        //        Nazwisko = "Gazdowska",
        //        DrugieNazwisko = "Litkowska",
        //        PESEL = "70090509716"
        //    };

        //    var p1 = new Klient()
        //    {
        //        KlientId  = 5,
        //        CzyFizyczna = false,
        //        Nazwa = "Leffler Group",
        //        NIP = "721-46-77-8810",
        //        KwotaKredytu = 36645
        //    };

        //    var p2 = new Klient()
        //    {
        //        KlientId = 6,
        //        CzyFizyczna = false,
        //        Nazwa = "Leffler Group",
        //        NIP = "721-46-77-8810",
        //        KwotaKredytu = 36645
        //    };
        //    var p3 = new Klient()
        //    {
        //        KlientId = 7,
        //        CzyFizyczna = false,
        //        Nazwa = "Leffler Group",
        //        NIP = "721-46-77-8810",
        //        KwotaKredytu = 36645
        //    };
        //    var p4 = new Klient()
        //    {
        //        KlientId= 8,
        //        CzyFizyczna = false,
        //        Nazwa = "Leffler Group",
        //        NIP = "721-46-77-8810",
        //        KwotaKredytu = 36645
        //    };
        //    ListaKlients.Add(k1);
        //    ListaKlients.Add(k2);
        //    ListaKlients.Add(k3);
        //    ListaKlients.Add(k4);
        //    ListaKlients.Add(p1);
        //    ListaKlients.Add(p2);
        //    ListaKlients.Add(p3);
        //    ListaKlients.Add(p4);

        //    OsobyFizyczne.ItemsSource = ListaKlients.Where(klient => klient.CzyFizyczna);
        //    OsobyPrawne.ItemsSource = ListaKlients.Where(klient => !klient.CzyFizyczna);
        //}

        private void SearchFizyczna_OnClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(IdFizyczna.Text) && string.IsNullOrEmpty(NazwaFizyczna.Text))
            {
                OsobyFizyczne.ItemsSource = FizyczneList.Where(klient => klient.CzyFizyczna); 
                return;
            }
            if (String.IsNullOrEmpty(IdFizyczna.Text))
            {
                var listTmp = FizyczneList.Where(klient => klient.CzyFizyczna).Where(klient => klient.Nazwisko.ToLower().Contains(NazwaFizyczna.Text.ToLower()));
                OsobyFizyczne.ItemsSource = listTmp;
                return;
            }
            if (String.IsNullOrEmpty(NazwaFizyczna.Text))
            {
                var listTmp = FizyczneList.Where(klient => klient.CzyFizyczna).Where(klient => klient.KlientId.ToString() == IdFizyczna.Text);
                OsobyFizyczne.ItemsSource = listTmp;
                return;
            }
            var listTmpF = FizyczneList.Where(klient => klient.CzyFizyczna).Where(klient => klient.KlientId.ToString() == IdFizyczna.Text && klient.Nazwisko.ToLower().Contains(NazwaFizyczna.Text.ToLower()));
            OsobyFizyczne.ItemsSource = listTmpF;
        }

        private void SearchPrawna_OnClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(IdPrawna.Text) && string.IsNullOrEmpty(NazwaPrawna.Text))
            {
                OsobyPrawne.ItemsSource = PrawneList.Where(klient => !klient.CzyFizyczna);
                return;
            }
            if (String.IsNullOrEmpty(IdPrawna.Text))
            {
                var listTmp = PrawneList.Where(klient => !klient.CzyFizyczna).Where(klient => klient.Nazwa.ToLower().Contains(NazwaPrawna.Text.ToLower()));
                OsobyPrawne.ItemsSource = listTmp;
                return;
            }
            if (String.IsNullOrEmpty(NazwaPrawna.Text))
            {
                var listTmp = PrawneList.Where(klient => !klient.CzyFizyczna).Where(klient => klient.KlientId.ToString() == IdPrawna.Text);
                OsobyPrawne.ItemsSource = listTmp;
                return;
            }
            var listTmpF = PrawneList.Where(klient => !klient.CzyFizyczna).Where(klient => klient.KlientId.ToString() == IdPrawna.Text && klient.Nazwa.ToLower().Contains(NazwaPrawna.Text.ToLower()));
            OsobyPrawne.ItemsSource = listTmpF;
        }

        private void OsobyPrawne_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currPrawna = (OsobyListItem)OsobyPrawne.SelectedItem;
            StatusNameBox.Text = currPrawna.NazwaStatusu;
            StatusDateBox.Text = currPrawna.DataCzas;
            StatusBoxPkt.Text = currPrawna.SumaPkt.ToString();
            selectedFromBothListItem = currPrawna;
        }

        private void OsobyFizyczne_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currPrawna = (OsobyListItem)OsobyFizyczne.SelectedItem;
            StatusNameBox.Text = currPrawna.NazwaStatusu;
            StatusDateBox.Text = currPrawna.DataCzas;
            StatusBoxPkt.Text = currPrawna.SumaPkt.ToString();
            selectedFromBothListItem = currPrawna;
        }

        private void CountStatus_OnClick(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
