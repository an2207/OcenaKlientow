using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;
using OcenaKlientow.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OcenaKlientow.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PU1 : Page
    {
        #region Private fields

        private readonly List<RodzajBenefitu> TypyBenfitowList;
        private BenefitView _currentBenefitView;
        CultureInfo culture = new CultureInfo("pt-BR");

        #endregion
        #region Properties

        public List<BenefitView> ListaBenefitow { get; set; }

        public List<PrzypisanyStatus> PrzypisanyStatuses { get; set; }

        public BenefitViewModel Pu1ViewModel { get; set; }

        public List<Status> Statuses { get; set; }

        #endregion
        #region Ctors

        public PU1()
        {
            InitializeComponent();

            Pu1ViewModel = new BenefitViewModel(true);
            ListaBenefitow = Pu1ViewModel.BenefitListQuery();
            //ListaBenefitow = db.Benefity.ToList();
            BenefitList.ItemsSource = ListaBenefitow;
            Statuses = Pu1ViewModel.GetStatuses();
            TypyBenfitowList = Pu1ViewModel.GetBenefitTypes();
            typ.ItemsSource = TypyBenfitowList;
            //PrzypisanyStatuses = db.PrzypisaneStatusy.ToList();
            //ListaBenefitow = new List<Benefit>();
            // Statuses = new List<Status>();
            // PrzypisanyStatuses = new List<PrzypisanyStatus>();

            //AddData();
            // AddStatuses();
            // AddPrzypisaneStatusy();
        }

        #endregion
        #region Event handlers

        private async void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var selRodzBen = (RodzajBenefitu)typ.SelectedItem;
            if (CheckValues())
                return;

            RodzajBenefitu idRabat = Pu1ViewModel.GetRabatId();
            RodzajBenefitu idTermin = Pu1ViewModel.GetTerminId();
            var newBenefit = new Benefit
            {
                Nazwa = selName.Text,
                DataZakon = selDataZakon.Text,
                DataUaktyw = selDataUaktyw.Text,
                RodzajId = selRodzBen.Nazwa == "Rabat" ? idRabat.RodzajId : idTermin.RodzajId,
                WartoscProc = selRodzBen.Nazwa == "Rabat" ? double.Parse(selWartProc.Text) : 0,
                LiczbaDni = selRodzBen.Nazwa == "Rabat" ? 0 : int.Parse(selWartProc.Text),
                Opis = opis.Text
            };
            Pu1ViewModel.AddNewBenefit(newBenefit);

            if ((bool)zloty.IsChecked)
                Pu1ViewModel.AddStatusToBenefit(newBenefit.BenefitId, "ZŁOTY");
            else
                Pu1ViewModel.DeleteStatusFromBenefit(newBenefit.BenefitId, "ZŁOTY");

            if ((bool)zolty.IsChecked)
                Pu1ViewModel.AddStatusToBenefit(newBenefit.BenefitId, "ŻÓŁTY");
            else
                Pu1ViewModel.DeleteStatusFromBenefit(newBenefit.BenefitId, "ŻÓŁTY");

            if ((bool)zielony.IsChecked)
                Pu1ViewModel.AddStatusToBenefit(newBenefit.BenefitId, "ZIELONY");
            else
                Pu1ViewModel.DeleteStatusFromBenefit(newBenefit.BenefitId, "ZIELONY");

            if ((bool)pomaran.IsChecked)
                Pu1ViewModel.AddStatusToBenefit(newBenefit.BenefitId, "POMARAŃCZOWY");
            else
                Pu1ViewModel.DeleteStatusFromBenefit(newBenefit.BenefitId, "POMARAŃCZOWY");
            if ((bool)czerw.IsChecked)
                Pu1ViewModel.AddStatusToBenefit(newBenefit.BenefitId, "CZERWONY");
            else
                Pu1ViewModel.DeleteStatusFromBenefit(newBenefit.BenefitId, "CZERWONY");

            BenefitList.ItemsSource = Pu1ViewModel.BenefitListQuery();
            ChangeLabelsAndInputsOFF();
            Add.Visibility = Visibility.Collapsed;
            ResetLabels();
            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = "Benefit został pomyślnie zapisany."
            };


            dialog.PrimaryButtonText = "OK";
            dialog.PrimaryButtonClick += delegate {
            };

            var result = await dialog.ShowAsync();
        }

        private void AddNew_OnClick(object sender, RoutedEventArgs e)
        {
            ClearLabels();
            ChangeLabelsAndInputsON();
            Save.Visibility = Visibility.Collapsed;
            Add.Visibility = Visibility.Visible;
        }

        private async void BenefitList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cancel.Visibility == Visibility.Visible)
            {
                if (BenefitList.SelectedItem == _currentBenefitView)
                {
                    return;
                }

                if (await RejectChanges())
                {
                    
                    BenefitList.SelectedItem = _currentBenefitView;
                    return;
                }
            }
            ResetLabels();
            Add.Visibility = Visibility.Collapsed;
            var benefit = (BenefitView)BenefitList.SelectedItem;
            if (benefit == null) return;
            _currentBenefitView = benefit;
            if (benefit.RodzajId == 2)
            {
                typValueLabel.Text = "Wartość rabatu*";
                typ.SelectedIndex = 1;
                procent.Visibility = Visibility.Visible;
            }
            else
            {
                typValueLabel.Text = "Liczba dni*";
                typ.SelectedIndex = 0;
                procent.Visibility = Visibility.Collapsed;
            }

            selName.Text = benefit.NazwaBenefitu;
            selWartProc.Text = benefit.RodzajId == 2 ? benefit.WartoscProc.ToString() : benefit.LiczbaDni.ToString();
            selDataUaktyw.Text = benefit.DataUaktyw;
            selDataZakon.Text = benefit.DataZakon;
            opis.Text = benefit.Opis ?? "BRAK";
            List<int> currBenefitStatusy = Pu1ViewModel.GetCurrentBenefitPrzypisaneStatuses(benefit);

            if (currBenefitStatusy.Contains(5))
                zloty.IsChecked = true;
            else
                zloty.IsChecked = false;
            if (currBenefitStatusy.Contains(4))
                zielony.IsChecked = true;
            else
                zielony.IsChecked = false;
            if (currBenefitStatusy.Contains(3))
                zolty.IsChecked = true;
            else
                zolty.IsChecked = false;
            if (currBenefitStatusy.Contains(2))
                pomaran.IsChecked = true;
            else
                pomaran.IsChecked = false;
            if (currBenefitStatusy.Contains(1))
                czerw.IsChecked = true;
            else
                czerw.IsChecked = false;
            ChangeLabelsAndInputsOFF();

        }

        private async Task<bool> RejectChanges()
        {
            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = $"Czy na pewno chcesz odrzucić wprowadzone zmiany?"
            };
            bool stay = false;

            dialog.PrimaryButtonText = "Tak";
            dialog.SecondaryButtonText = "Nie";
            dialog.PrimaryButtonClick += delegate {
                                                 stay = false;

            };
            dialog.SecondaryButtonClick += delegate
                                               {
                                                   stay = true;
            };

            var result = await dialog.ShowAsync();
            return stay;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = $"Czy na pewno chcesz odrzucić wprowadzone zmiany?"
            };


            dialog.PrimaryButtonText = "Tak";
            dialog.SecondaryButtonText = "Nie";
            dialog.PrimaryButtonClick += delegate {
                                                 if (Save.Visibility == Visibility.Collapsed)
                                                 {
                                                        Frame.Navigate(typeof(PU1));
                                                 }
                                                 ChangeLabelsAndInputsOFF();
                                                 
            };
            dialog.SecondaryButtonClick += delegate
                                               {
                                                   return;
                                               };

            var result = await dialog.ShowAsync();
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog1 = new ContentDialog()
            {
                MaxWidth = this.ActualWidth,
                Content = $"Czy na pewno chcesz usunąć benefit?"
            };


            dialog1.PrimaryButtonText = "Tak";
            dialog1.SecondaryButtonText = "Nie";
            dialog1.PrimaryButtonClick += delegate
            {
                var benToDel = (BenefitView)BenefitList.SelectedItem;
                Pu1ViewModel.DeleteFromBenefitList(benToDel);
                BenefitList.ItemsSource = Pu1ViewModel.BenefitListQuery();
                ClearLabels();
            };
            dialog1.SecondaryButtonClick += delegate
            {
                return;
            };
            var result1 = await dialog1.ShowAsync();
        }


        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeLabelsAndInputsON();
        }

        private async void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckValues())
                return;
            var curr = (BenefitView)BenefitList.SelectedItem;
            Benefit benefit;
            benefit = Pu1ViewModel.GetBenefit(curr.BenefitId);

            if (benefit != null)
            {
                benefit.Nazwa = selName.Text;
                benefit.Opis = opis.Text;
                var selectedRodzaj = (RodzajBenefitu)typ.SelectedItem;
                if (selectedRodzaj.Nazwa == "Rabat")
                {
                    benefit.WartoscProc = double.Parse(selWartProc.Text);
                    benefit.LiczbaDni = 0;
                    benefit.RodzajId = selectedRodzaj.RodzajId;
                }
                else
                {
                    benefit.LiczbaDni = int.Parse(selWartProc.Text);
                    benefit.WartoscProc = 0;
                    benefit.RodzajId = selectedRodzaj.RodzajId;
                }
                benefit.DataUaktyw = selDataUaktyw.Text;
                benefit.DataZakon = selDataZakon.Text;
                if ((bool)zloty.IsChecked)
                    Pu1ViewModel.AddStatusToBenefit(benefit.BenefitId, "ZŁOTY");
                else
                    Pu1ViewModel.DeleteStatusFromBenefit(benefit.BenefitId, "ZŁOTY");

                if ((bool)zolty.IsChecked)
                    Pu1ViewModel.AddStatusToBenefit(benefit.BenefitId, "ŻÓŁTY");
                else
                    Pu1ViewModel.DeleteStatusFromBenefit(benefit.BenefitId, "ŻÓŁTY");

                if ((bool)zielony.IsChecked)
                    Pu1ViewModel.AddStatusToBenefit(benefit.BenefitId, "ZIELONY");
                else
                    Pu1ViewModel.DeleteStatusFromBenefit(benefit.BenefitId, "ZIELONY");

                if ((bool)pomaran.IsChecked)
                    Pu1ViewModel.AddStatusToBenefit(benefit.BenefitId, "POMARAŃCZOWY");
                else
                    Pu1ViewModel.DeleteStatusFromBenefit(benefit.BenefitId, "POMARAŃCZOWY");
                if ((bool)czerw.IsChecked)
                    Pu1ViewModel.AddStatusToBenefit(benefit.BenefitId, "CZERWONY");
                else
                    Pu1ViewModel.DeleteStatusFromBenefit(benefit.BenefitId, "CZERWONY");

                Pu1ViewModel.UpdateBenefit(benefit);
                
                ChangeLabelsAndInputsOFF();
                ResetLabels();
                var dialog = new ContentDialog()
                {
                    Title = "Zmodyfikowano",
                    MaxWidth = this.ActualWidth,
                    Content = $"Benefit {benefit.Nazwa} został edytowany."
                };


                dialog.PrimaryButtonText = "OK";
                dialog.PrimaryButtonClick += delegate {
                };

                var result = await dialog.ShowAsync();
                BenefitList.ItemsSource = Pu1ViewModel.BenefitListQuery();
            }
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(IdBenefitu.Text) && string.IsNullOrEmpty(NazwaBenefitu.Text))
            {
                BenefitList.ItemsSource = ListaBenefitow;
                return;
            }
            if (string.IsNullOrEmpty(IdBenefitu.Text))
            {
                IEnumerable<BenefitView> listTmp = ListaBenefitow.Where(benefit => benefit.NazwaBenefitu.ToLower().Contains(NazwaBenefitu.Text.ToLower()));
                BenefitList.ItemsSource = listTmp;
                return;
            }
            if (string.IsNullOrEmpty(NazwaBenefitu.Text))
            {
                IEnumerable<BenefitView> listTmp = ListaBenefitow.Where(benefit => benefit.BenefitId.ToString() == IdBenefitu.Text);
                BenefitList.ItemsSource = listTmp;
                return;
            }
            IEnumerable<BenefitView> listTmpF = ListaBenefitow.Where(benefit => (benefit.BenefitId.ToString() == IdBenefitu.Text) && benefit.NazwaBenefitu.ToLower().Contains(NazwaBenefitu.Text.ToLower()));
            BenefitList.ItemsSource = listTmpF;
        }

        private void Typ_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selRodzaj = (RodzajBenefitu)typ.SelectedItem;
            if (selRodzaj.Nazwa == "Rabat")
            {
                typValueLabel.Text = "Wartość rabatu*";
                procent.Visibility = Visibility.Visible;
            }
            else
            {
                typValueLabel.Text = "Liczba dni*";
                procent.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
        #region Public methods

        public bool CheckValues()
        {
            var error = false;
            if (selName.Text == "")
            {
                selName.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if (typ.SelectedItem == null)
            {
                typ.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }

            if ((selWartProc.Text == "") || (int.Parse(selWartProc?.Text) < 0) || (int.Parse(selWartProc?.Text) >100))
            {
                selWartProc.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if (selDataUaktyw.Text == "")
            {
                selDataUaktyw.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if (!(bool)zloty.IsChecked && !(bool)zielony.IsChecked && !(bool)zolty.IsChecked && !(bool)pomaran.IsChecked && !(bool)czerw.IsChecked)
            {
                zloty.BorderThickness = new Thickness(2);
                zielony.BorderThickness = new Thickness(2);
                zolty.BorderThickness = new Thickness(2);
                pomaran.BorderThickness = new Thickness(2);
                czerw.BorderThickness = new Thickness(2);
                zloty.BorderBrush = new SolidColorBrush(Colors.Red);
                zolty.BorderBrush = new SolidColorBrush(Colors.Red);
                zielony.BorderBrush = new SolidColorBrush(Colors.Red);
                czerw.BorderBrush = new SolidColorBrush(Colors.Red);
                pomaran.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            var dataRegex = new Regex("^(19|20)\\d{2}\\/(0[1-9]|1[0-2])\\/(0[1-9]|1\\d|2\\d|3[01])$");
            if (!dataRegex.IsMatch(selDataUaktyw.Text) && !dataRegex.IsMatch(selDataZakon.Text))
            {
                selDataZakon.BorderBrush = new SolidColorBrush(Colors.Red);
                selDataUaktyw.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            else
            {
                var dataU = DateTime.Parse(selDataUaktyw.Text, culture);
                var dataZ = DateTime.Parse(selDataZakon.Text, culture);
                if (dataU > dataZ)
                {
                    selDataZakon.BorderBrush = new SolidColorBrush(Colors.Red);
                    selDataUaktyw.BorderBrush = new SolidColorBrush(Colors.Red);
                    error = true;
                }
            }
            
            var regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(selWartProc.Text))
            {
                selWartProc.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            return error;
        }

        public void ResetLabels()
        {
            var color = opis.BorderBrush;

            selName.BorderBrush = color;

            typ.BorderBrush = color;

            selWartProc.BorderBrush = color;

            selDataUaktyw.BorderBrush = color;


            zloty.BorderThickness = new Thickness(0);
            zielony.BorderThickness = new Thickness(0);
            zolty.BorderThickness = new Thickness(0);
            pomaran.BorderThickness = new Thickness(0);
            czerw.BorderThickness = new Thickness(0);
            zloty.BorderBrush = color;
            zolty.BorderBrush = color;
            zielony.BorderBrush = color;
            czerw.BorderBrush = color;
            pomaran.BorderBrush = color;
        }

        #endregion
        #region Private methods

        private void ChangeLabelsAndInputsOFF()
        {
            selWartProc.IsReadOnly = true;
            selDataUaktyw.IsReadOnly = true;
            selName.IsReadOnly = true;
            selDataZakon.IsReadOnly = true;
            opis.IsReadOnly = true;
            zloty.IsEnabled = false;
            zielony.IsEnabled = false;
            zolty.IsEnabled = false;
            pomaran.IsEnabled = false;
            czerw.IsEnabled = false;
            Save.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
        }

        private void ChangeLabelsAndInputsON()
        {
            selWartProc.IsReadOnly = false;
            selDataUaktyw.IsReadOnly = false;
            selName.IsReadOnly = false;
            selDataZakon.IsReadOnly = false;
            opis.IsReadOnly = false;
            zloty.IsEnabled = true;
            zielony.IsEnabled = true;
            zolty.IsEnabled = true;
            pomaran.IsEnabled = true;
            czerw.IsEnabled = true;
            Save.Visibility = Visibility.Visible;
            Cancel.Visibility = Visibility.Visible;
            ;
        }

        private void ClearLabels()
        {
            selName.Text = "";
            selWartProc.Text = "";
            selDataZakon.Text = "";
            selDataUaktyw.Text = "";
            opis.Text = "";
            zloty.IsChecked = false;
            zielony.IsChecked = false;
            zolty.IsChecked = false;
            pomaran.IsChecked = false;
            czerw.IsChecked = false;
        }

        #endregion
        private void SelWartProc_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (selWartProc.Text == "")
            {
                selWartProc.BorderBrush = opis.BorderBrush;
                return;
            }
            var regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(selWartProc.Text) )
            {
                selWartProc.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                if (selWartProc.Text=="" ||(int.Parse(selWartProc?.Text) < 0) || (int.Parse(selWartProc?.Text) > 100))
                {
                    selWartProc.BorderBrush = new SolidColorBrush(Colors.Red);
                    return;
                }
                selWartProc.BorderBrush = opis.BorderBrush;
            }
        }

        private void Button_OnGotFocus(object sender, RoutedEventArgs e)
        {
            button.Background = new SolidColorBrush(Colors.GhostWhite);
        }
    }
}