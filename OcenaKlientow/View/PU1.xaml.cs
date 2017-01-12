using System;
using System.Collections;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;
using OcenaKlientow.View.ListItems;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OcenaKlientow.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PU1 : Page
    {
        private List<BenefitListItem> _listaBenefitow;

        private List<Status> _statuses;

        private List<PrzypisanyStatus> _przypisanyStatuses;

        private List<RodzajBenefitu> TypyBenfitowList;

        public PU1()
        {
            this.InitializeComponent();
            using (var db = new OcenaKlientowContext())
            {

                ListaBenefitow = BenefitListQuery(db);
                //ListaBenefitow = db.Benefity.ToList();
                BenefitList.ItemsSource = ListaBenefitow;
                Statuses = db.Statusy.ToList();
                TypyBenfitowList = db.RodzajeBenefitow.ToList();
                typ.ItemsSource = TypyBenfitowList;
                
                //PrzypisanyStatuses = db.PrzypisaneStatusy.ToList();
                //ListaBenefitow = new List<Benefit>();
                // Statuses = new List<Status>();
                // PrzypisanyStatuses = new List<PrzypisanyStatus>();
            }
                
            //AddData();
          // AddStatuses();
           // AddPrzypisaneStatusy();
        }

        public List<BenefitListItem> ListaBenefitow
        {
            get
            {
                return _listaBenefitow;
            }
            set
            {
                _listaBenefitow = value;
            }
        }

       public List<Status> Statuses
        {
            get
            {
                return _statuses;
            }
            set
            {
                _statuses = value;
            }
        }

        public List<PrzypisanyStatus> PrzypisanyStatuses
        {
            get
            {
                return _przypisanyStatuses;
            }
            set
            {
                _przypisanyStatuses = value;
            }
        }

        //void AddData()
        //{
           
        //    Benefit b1 = new Benefit()
        //    {
        //        BenefitId = 1,
        //        DataUaktyw = "22/10/2015",
        //        DataZakon = "22/11/2016",
        //        Nazwa = "Oferta letnia",
        //        Opis = "Okej"
        //    };
        //    Benefit b2 = new Benefit()
        //    {
        //        BenefitId = 1,
        //        DataUaktyw = "22/10/2015",
        //        DataZakon = "22/11/2016",
        //        Nazwa = "Oferta wiosna",
        //        Opis = "Dziala"
        //    };
        //    Benefit b3 = new Benefit()
        //    {
        //        BenefitId = 1,
        //        DataUaktyw = "22/10/2015",
        //        DataZakon = "22/11/2016",
        //        Nazwa = "Oferta zimowa",
        //        Opis = "Dziala"
        //    };
        //     Benefit b4 = new Benefit()
        //     {
        //         BenefitId = 1,
        //         DataUaktyw = "22/10/2015",
        //         DataZakon = "22/11/2016",
        //         Nazwa = "Oferta zimowa",
        //         Opis = "Dziala"
        //     }; Benefit b5 = new Benefit()
        //     {
        //         BenefitId = 1,
        //         DataUaktyw = "22/10/2015",
        //         DataZakon = "22/11/2016",
        //         Nazwa = "Oferta zimowa",
        //         Opis = "Dziala"
        //     }; Benefit b6 = new Benefit()
        //     {
        //         BenefitId = 1,
        //         DataUaktyw = "22/10/2015",
        //         DataZakon = "22/11/2016",
        //         Nazwa = "Oferta zimowa",
        //         Opis = "Dziala"
        //     }; Benefit b7 = new Benefit()
        //     {
        //         BenefitId = 2,
        //         DataUaktyw = "22/10/2015",
        //         DataZakon = "22/11/2016",
        //         Nazwa = "Oferta zimowaadsfasdfadfasdfasdfdas",
        //         Opis = "Dziala"
        //     }; Benefit b8 = new Benefit()
        //     {
        //         BenefitId = 2,
        //         DataUaktyw = "22/10/2015",
        //         DataZakon = "22/11/2016",
        //         Nazwa = "Oferta zimowa",
        //         Opis = "Dziala dasfasdfasdfasdfasdfadsf"
        //     };
        //    ListaBenefitow.Add(b1);
        //    ListaBenefitow.Add(b2);
        //    ListaBenefitow.Add(b3);
        //    ListaBenefitow.Add(b4);
        //    ListaBenefitow.Add(b5);
        //    ListaBenefitow.Add(b6);
        //    ListaBenefitow.Add(b7);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    ListaBenefitow.Add(b8);
        //    BenefitList.ItemsSource = ListaBenefitow;
        //}

        //void AddStatuses()
        //{
        //    Status zloty = new Status()
        //    {
        //        Nazwa = "złoty"
        //    };
        //    Status zie = new Status()
        //    {
        //        Nazwa = "zielony"
        //    };
        //    Status zolty = new Status()
        //    {
        //        Nazwa = "żółty"
        //    };
        //    Status pom = new Status()
        //    {
        //        Nazwa = "pomarańczowy"
        //    };
        //    Status czerw = new Status()
        //    {
        //        Nazwa = "czerwony"
        //    };
        //    Statuses.Add(zloty);
        //    Statuses.Add(zie);
        //    Statuses.Add(zolty);
        //    Statuses.Add(pom);
        //    Statuses.Add(czerw);
            
        //}

        //void AddPrzypisaneStatusy()
        //{
        //    var p1 = new PrzypisanyStatus()
        //    {
        //        BenefitId = 1,
        //        StatusId = 1
        //    };
        //    var p2 = new PrzypisanyStatus()
        //    {
        //        BenefitId = 1,
        //        StatusId = 1
        //    }; var p3 = new PrzypisanyStatus()
        //    {
        //        BenefitId = 2,
        //        StatusId = 1
        //    }; var p4 = new PrzypisanyStatus()
        //    {
        //        BenefitId = 2,
        //        StatusId = 2
        //    };

        //    PrzypisanyStatuses.Add(p1);
        //    PrzypisanyStatuses.Add(p2);
        //    PrzypisanyStatuses.Add(p3);
        //    PrzypisanyStatuses.Add(p4);

        //}

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeLabelsAndInputsON();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {//zrobić
            using (var db = new OcenaKlientowContext())
            {
                var benToDel =(BenefitListItem) BenefitList.SelectedItem;
                var przypStat = db.PrzypisaneStatusy.Where(ben => ben.BenefitId.Equals(benToDel.BenefitId)).ToList();
                foreach (PrzypisanyStatus przypisanyStatuse in przypStat)
                {
                    db.Entry(przypisanyStatuse).State = EntityState.Deleted;

                }
                var currBen = db.Benefity.Where(benefit => benefit.BenefitId.Equals(benToDel.BenefitId)).FirstOrDefault();
                db.Entry(currBen).State=EntityState.Deleted;
                db.SaveChanges();
               BenefitList.ItemsSource = BenefitListQuery(db);
            }
        }

        private void AddNew_OnClick(object sender, RoutedEventArgs e)
        {
            ClearLabels();
            ChangeLabelsAndInputsON();
            Save.Visibility = Visibility.Collapsed;
            Add.Visibility = Visibility.Visible;
          


        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(IdBenefitu.Text) && string.IsNullOrEmpty(NazwaBenefitu.Text))
            {
                BenefitList.ItemsSource = ListaBenefitow;
                return;
            }
            if (String.IsNullOrEmpty(IdBenefitu.Text))
            {
                var listTmp = ListaBenefitow.Where(benefit => benefit.NazwaBenefitu.ToLower().Contains(NazwaBenefitu.Text.ToLower()));
                BenefitList.ItemsSource = listTmp;
                return;
            }
            if (String.IsNullOrEmpty(NazwaBenefitu.Text))
            {
                var listTmp = ListaBenefitow.Where(benefit => benefit.BenefitId.ToString() == IdBenefitu.Text);
                BenefitList.ItemsSource = listTmp;
                return;
            }
            var listTmpF = ListaBenefitow.Where(benefit => benefit.BenefitId.ToString() == IdBenefitu.Text && benefit.NazwaBenefitu.ToLower().Contains(NazwaBenefitu.Text.ToLower()));
            BenefitList.ItemsSource = listTmpF;
        }

        
        private void BenefitList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var benefit = (BenefitListItem)BenefitList.SelectedItem;
            if (benefit == null) return;
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
                procent.Visibility=Visibility.Collapsed;
            }
            
            selName.Text = benefit.NazwaBenefitu;
            selWartProc.Text = benefit.RodzajId == 2? benefit.WartoscProc.ToString() : benefit.LiczbaDni.ToString();
            selDataUaktyw.Text = benefit.DataUaktyw;
            selDataZakon.Text = benefit.DataZakon;
            opis.Text = benefit.Opis ?? "BRAK";
            List<int> currBenefitStatusy;
            using (var db = new OcenaKlientowContext())
            {
                currBenefitStatusy = db.PrzypisaneStatusy.Where(status => status.BenefitId == benefit.BenefitId).Select(status => status.StatusId).ToList();
            }
            
            if (currBenefitStatusy.Contains(5))
            {
                zloty.IsChecked = true;
            }
            else
            {
                zloty.IsChecked = false;
            }
            if (currBenefitStatusy.Contains(4))
            {
                zielony.IsChecked = true;
            }
            else
            {
                zielony.IsChecked = false;
            }
            if (currBenefitStatusy.Contains(3))
            {
                zolty.IsChecked = true;
            }
            else
            {
                zolty.IsChecked = false;
            }
            if (currBenefitStatusy.Contains(2))
            {
                pomaran.IsChecked = true;
            }
            else
            {
                pomaran.IsChecked = false;
            }
            if (currBenefitStatusy.Contains(1))
            {
                czerw.IsChecked = true;
            }
            else
            {
                czerw.IsChecked = false;
            }
            ChangeLabelsAndInputsOFF();
            //opis.Text = benefit.Opis;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var curr =(BenefitListItem) BenefitList.SelectedItem;
            Benefit benefit;
            using (var db = new OcenaKlientowContext())
            {
                benefit = db.Benefity.Where(benefit1 => benefit1.BenefitId == curr.BenefitId).FirstOrDefault();

                if (benefit != null)
                {
                    benefit.Nazwa = selName.Text;
                    benefit.Opis = opis.Text;
                    var selectedRodzaj = (RodzajBenefitu)typ.SelectedItem;
                    if (selectedRodzaj.RodzajId == 2)
                    {
                        benefit.LiczbaDni = Int32.Parse(selWartProc.Text);
                        benefit.WartoscProc = 0;
                        benefit.RodzajId = selectedRodzaj.RodzajId;
                    }
                    else
                    {
                        benefit.WartoscProc = Double.Parse(selWartProc.Text);
                        benefit.LiczbaDni = 0;
                        benefit.RodzajId = selectedRodzaj.RodzajId;
                    }
                    benefit.DataUaktyw = selDataUaktyw.Text;
                    benefit.DataZakon = selDataZakon.Text;
                    if ((bool)zloty.IsChecked)
                    {
                        AddStatusToBenefit(benefit, "ZŁOTY");
                    }
                    else
                    {
                        DeleteStatusFromBenefit(benefit, "ZŁOTY");
                    }

                    if ((bool)zolty.IsChecked)
                    {
                        AddStatusToBenefit(benefit, "ŻÓŁTY");
                    }
                    else
                    {
                        DeleteStatusFromBenefit(benefit, "ŻÓŁTY");
                    }

                    if ((bool)zielony.IsChecked)
                    {
                        AddStatusToBenefit(benefit, "ZIELONY");
                    }
                    else
                    {
                        DeleteStatusFromBenefit(benefit, "ZIELONY");
                    }

                    if ((bool)pomaran.IsChecked)
                    {
                        AddStatusToBenefit(benefit, "POMARAŃCZOWY");
                    }
                    else
                    {
                        DeleteStatusFromBenefit(benefit, "POMARAŃCZOWY");
                    }
                    if ((bool)czerw.IsChecked)
                    {
                        AddStatusToBenefit(benefit, "CZERWONY");
                    }
                    else
                    {
                        DeleteStatusFromBenefit(benefit, "CZERWONY");
                    }

                    db.SaveChanges();
                }
                BenefitList.ItemsSource = BenefitListQuery(db);
                ChangeLabelsAndInputsOFF();
                //this.Frame.Navigate(typeof(PU1));

            }
        }

        private void DeleteStatusFromBenefit(Benefit benefit, string name)
        {
            using (var db = new OcenaKlientowContext())
            {
                var status = db.Statusy.Where(status1 => status1.Nazwa == name).FirstOrDefault();
                var przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => status1.BenefitId == benefit.BenefitId && status1.StatusId == status.StatusId).FirstOrDefault();
                //var przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => status1.Benefit.BenefitId == benefit.BenefitId && status.StatusId == status1.Status.StatusId);
                if (przypisanyStatus == null)
                {
                    return;
                }
                db.PrzypisaneStatusy.Remove(przypisanyStatus);
                db.SaveChanges();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PU1));
        }

        private void Typ_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selRodzaj = (RodzajBenefitu)typ.SelectedItem;
            typValueLabel.Text = selRodzaj.Nazwa == "Rabat" ? "Wartość rabatu*" : "Liczba dni*";
        }

        void AddStatusToBenefit(Benefit benefit, string name)
        {
            using (var db = new OcenaKlientowContext())
            {
                var status = db.Statusy.Where(status1 => status1.Nazwa == name).FirstOrDefault();
                var przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => status1.BenefitId == benefit.BenefitId && status1.StatusId == status.StatusId).FirstOrDefault();
                //var przypisanyStatus = db.PrzypisaneStatusy.Where(status1 => status1.Status == status && status1.Benefit == benefit);
                if (przypisanyStatus==null)
                {
                    db.PrzypisaneStatusy.Add(new PrzypisanyStatus()
                    {
                        BenefitId = benefit.BenefitId,
                        StatusId = status.StatusId
                    });
                    db.SaveChanges();
                }
            }

        }

        List<BenefitListItem> BenefitListQuery( OcenaKlientowContext db)
        {
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
            return innerJoinQuery.ToList();
        }

        void ChangeLabelsAndInputsON()
        {
            selWartProc.IsReadOnly = false;
            selDataUaktyw.IsReadOnly = false;
            selName.IsReadOnly = false;
            selDataZakon.IsReadOnly = false;
            opis.IsReadOnly = false;
            zloty.IsEnabled = true;
            Save.Visibility = Visibility.Visible;
            Cancel.Visibility = Visibility.Visible;;
        }

        void ChangeLabelsAndInputsOFF()
        {
            selWartProc.IsReadOnly = true;
            selDataUaktyw.IsReadOnly = true;
            selName.IsReadOnly = true;
            selDataZakon.IsReadOnly = true;
            opis.IsReadOnly = true;
            zloty.IsEnabled = false;
            Save.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
        }

        void ClearLabels()
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

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var selRodzBen = (RodzajBenefitu)typ.SelectedItem;

            using (var db = new OcenaKlientowContext())
            {
                var idRabat = db.RodzajeBenefitow.Where(benefitu => benefitu.Nazwa.Equals("Rabat")).FirstOrDefault();
                var idTermin = db.RodzajeBenefitow.Where(benefitu => benefitu.Nazwa.Equals("Wydłużony termin")).FirstOrDefault();
                var newBenefit = new Benefit()
                {
                    Nazwa = selName.Text,
                    DataZakon = selDataZakon.Text,
                    DataUaktyw = selDataUaktyw.Text,
                    RodzajId = selRodzBen.Nazwa == "Rabat" ? idRabat.RodzajId : idTermin.RodzajId,
                    WartoscProc = selRodzBen.Nazwa == "Rabat" ? Double.Parse(selWartProc.Text) : 0,
                    LiczbaDni = selRodzBen.Nazwa == "Rabat" ? 0 : Int32.Parse(selWartProc.Text),
                    Opis = opis.Text
                };
                db.Benefity.Add(newBenefit);
                db.SaveChanges();
                if ((bool)zloty.IsChecked)
                {
                    AddStatusToBenefit(newBenefit, "ZŁOTY");
                }
                else
                {
                    DeleteStatusFromBenefit(newBenefit, "ZŁOTY");
                }

                if ((bool)zolty.IsChecked)
                {
                    AddStatusToBenefit(newBenefit, "ŻÓŁTY");
                }
                else
                {
                    DeleteStatusFromBenefit(newBenefit, "ŻÓŁTY");
                }

                if ((bool)zielony.IsChecked)
                {
                    AddStatusToBenefit(newBenefit, "ZIELONY");
                }
                else
                {
                    DeleteStatusFromBenefit(newBenefit, "ZIELONY");
                }

                if ((bool)pomaran.IsChecked)
                {
                    AddStatusToBenefit(newBenefit, "POMARAŃCZOWY");
                }
                else
                {
                    DeleteStatusFromBenefit(newBenefit, "POMARAŃCZOWY");
                }
                if ((bool)czerw.IsChecked)
                {
                    AddStatusToBenefit(newBenefit, "CZERWONY");
                }
                else
                {
                    DeleteStatusFromBenefit(newBenefit, "CZERWONY");
                }

                BenefitList.ItemsSource = BenefitListQuery(db);
                ChangeLabelsAndInputsOFF();
                Add.Visibility = Visibility.Collapsed;
            }
        }
    }
}
