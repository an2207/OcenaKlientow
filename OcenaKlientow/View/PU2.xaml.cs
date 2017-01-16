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
            if (currPrawna != null)
            {
                StatusNameBox.Text = currPrawna.NazwaStatusu;
                StatusDateBox.Text = currPrawna.DataCzas;
                StatusBoxPkt.Text = currPrawna.SumaPkt.ToString();
                selectedFromBothListItem = currPrawna;
            }
            
        }

        private void OsobyFizyczne_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currPrawna = (OsobyListItem)OsobyFizyczne.SelectedItem;
            if (currPrawna != null)
            {
                StatusNameBox.Text = currPrawna.NazwaStatusu;
                StatusDateBox.Text = currPrawna.DataCzas;
                StatusBoxPkt.Text = currPrawna.SumaPkt.ToString();
                selectedFromBothListItem = currPrawna;
            }
           
        }

        private async void CountStatus_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedFromBothListItem != null)
            {
                var klient = Pu2ViewModel.GetKlient(selectedFromBothListItem.KlientId);
                Pu2ViewModel.CountStatus(klient);
                var name = selectedFromBothListItem.CzyFizyczna ? selectedFromBothListItem.Nazwisko : selectedFromBothListItem.Nazwa;
                var dialog = new ContentDialog()
                {
                    Title = "Przeliczono",
                    MaxWidth = this.ActualWidth,
                    Content = $"Klient {name} otrzymał nową ocene."
                };


                dialog.PrimaryButtonText = "OK";
                dialog.PrimaryButtonClick += delegate {
                };
                if (selectedFromBothListItem.CzyFizyczna)
                {
                    OsobyFizyczne.ItemsSource = Pu2ViewModel.OsobyFizyczneListQuery();
                }
                else
                {
                    OsobyPrawne.ItemsSource = Pu2ViewModel.OsobyPrawneListQuery();
                }
                var result = await dialog.ShowAsync();
            }
           

        }

        private async void GradeDetails_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedFromBothListItem != null)
            {
                Pu2ViewModel.GetGradeDetails(selectedFromBothListItem);


                var dialog = new ContentDialog()
                {
                    Title = "Szczegóły",
                    MaxWidth = this.ActualWidth,
                    Content = $"Punkty dodatnie:\n Płatność na czas\n10 punktów\nPłatność częściowa\n5 punktóws"
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
    }
}
