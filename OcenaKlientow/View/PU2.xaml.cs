using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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

        private List<KlientView> _prawneList;

        private List<KlientView> _fizyczneList;

        private KlientViewModel _viewModel;

        private KlientView selectedFromBothListItem;

        private OcenaViewModel _ocenaViewModel;
        public PU2()
        {
            KlientViewModel = new KlientViewModel(true);
            OcenaVM = new OcenaViewModel(true);
            KlientViewModel.OsobyPrawneListQuery();
            
            this.InitializeComponent();
            FizyczneList = KlientViewModel.OsobyFizyczneListQuery();
            PrawneList = KlientViewModel.OsobyPrawneListQuery();
            OsobyPrawne.ItemsSource = PrawneList;
            OsobyFizyczne.ItemsSource = FizyczneList;
            

        }

        public List<KlientView> PrawneList
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

        public List<KlientView> FizyczneList
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

        public KlientViewModel KlientViewModel
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

        public OcenaViewModel OcenaVM
        {
            get
            {
                return _ocenaViewModel;
            }
            set
            {
                _ocenaViewModel = value;
            }
        }

        /// <summary>
        /// Metoda obsługująca przycisk odpowiedzialny za wyszukiwanie klientów (osób fizycznych) z listy na podstawie danych wpisanych przez użytkownika. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Metoda obsługująca przycisk odpowiedzialny za wyszukiwanie klientów (osób prawnych) z listy na podstawie danych wpisanych przez użytkownika. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Metoda wyświetlająca szczegóły oceny i statusu wybranego klienta (osoby prawnej).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OsobyPrawne_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currPrawna = (KlientView)OsobyPrawne.SelectedItem;
            if (currPrawna != null)
            {
                StatusNameBox.Text = currPrawna.NazwaStatusu;
                StatusDateBox.Text = currPrawna.DataCzas;
                StatusBoxPkt.Text = currPrawna.SumaPkt.ToString();
                selectedFromBothListItem = currPrawna;
            }
            
        }

        /// <summary>
        /// Metoda wyświetlająca szczegóły oceny i statusu wybranego klienta (osoby fizycznej).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OsobyFizyczne_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currPrawna = (KlientView)OsobyFizyczne.SelectedItem;
            if (currPrawna != null)
            {
                StatusNameBox.Text = currPrawna.NazwaStatusu;
                StatusDateBox.Text = currPrawna.DataCzas;
                StatusBoxPkt.Text = currPrawna.SumaPkt.ToString();
                selectedFromBothListItem = currPrawna;
            }
           
        }

        /// <summary>
        /// Metoda obsługująca przycisk przeliczający ocenę klienta. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <see cref="OcenaViewModel.CountStatus(Klient)"></see>
        private async void CountStatus_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedFromBothListItem != null)
            {
                var klient = KlientViewModel.ReadKlient(selectedFromBothListItem.KlientId);
                OcenaVM.CountStatus(klient);
                var name = selectedFromBothListItem.CzyFizyczna ? selectedFromBothListItem.Nazwisko : selectedFromBothListItem.Nazwa;
                var dialog = new ContentDialog()
                {
                    Title = "Przeliczono",
                    MaxWidth = this.ActualWidth,
                    Content = $"Klient {name} otrzymał nową ocenę."
                };


                dialog.PrimaryButtonText = "OK";
                dialog.PrimaryButtonClick += delegate {
                };
                if (selectedFromBothListItem.CzyFizyczna)
                {
                    OsobyFizyczne.ItemsSource = KlientViewModel.OsobyFizyczneListQuery();
                }
                else
                {
                    OsobyPrawne.ItemsSource = KlientViewModel.OsobyPrawneListQuery();
                }
                var result = await dialog.ShowAsync();
            }
           

        }

        /// <summary>
        /// Metoda obsługująca przycisk "Zobacz szczegóły oceny" i wyświetlająca pop-up ze szczegółami aktualnej oceny klienta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GradeDetails_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedFromBothListItem != null)
            {
                var lista =OcenaVM.GetGradeDetails(selectedFromBothListItem);
                var platnCzas = lista.Where(details => details.Parametr.Nazwa.Equals("PLATN_NA_CZAS")).FirstOrDefault();
                var platnCalk = lista.Where(details => details.Parametr.Nazwa.Equals("PLATN_CALK")).FirstOrDefault();
                var platnCzes = lista.Where(details => details.Parametr.Nazwa.Equals("PLATN_CZESC")).FirstOrDefault();
                var regZam = lista.Where(details => details.Parametr.Nazwa.Equals("REGUL_ZAM")).FirstOrDefault();
                var przekTerm = lista.Where(details => details.Parametr.Nazwa.Equals("PRZEK_TERM")).FirstOrDefault();
                var limitKred = lista.Where(details => details.Parametr.Nazwa.Equals("LIMIT KREDYTU")).FirstOrDefault();
                double suma = 0;
                foreach (GradeDetails gradeDetailse in lista)
                {
                    suma += gradeDetailse.SumaPkt;
                }

                var dialog = new ContentDialog()
                {
                    Title = "Szczegóły oceny",
                    MaxWidth = this.ActualWidth,
                    Content = $"Płatność na czas: {platnCzas.SumaPkt} pkt\nPłatność częściowa: {platnCzes.SumaPkt} pkt\nPłatność całkowita: {platnCalk.SumaPkt} pkt\nRegularne zamówienie: {regZam.SumaPkt} pkt\nPrzekroczenie terminu płatności: {przekTerm.SumaPkt} pkt\nLimit kredytu: {limitKred.SumaPkt} pkt\nSuma punktów: {suma} pkt"
                };


                dialog.PrimaryButtonText = "OK";
                dialog.PrimaryButtonClick += delegate {
                };

                var result = await dialog.ShowAsync();
            }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Metoda obsługująca przycisk "Przelicz wszystkie statusy"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <see cref="OcenaViewModel.CoundAllGrades"/> 
        private async void CountAllStatuses_OnClick(object sender, RoutedEventArgs e)
        {
            OcenaVM.CountAllGrades();
            OsobyFizyczne.ItemsSource = KlientViewModel.OsobyFizyczneListQuery();
            OsobyPrawne.ItemsSource = KlientViewModel.OsobyPrawneListQuery();

            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = $"Przeliczono wszystkie statusy!"
            };


            dialog.PrimaryButtonText = "OK";
            dialog.PrimaryButtonClick += delegate {
            };

            var result = await dialog.ShowAsync();
        }
    }
}
