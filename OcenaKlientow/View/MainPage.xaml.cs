using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OcenaKlientow.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new OcenaKlientowContext())
            {
               // Blogs.ItemsSource = db.Platnosci.ToList();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new OcenaKlientowContext())
            {
                var blog = new Platnosc() { DataWymag = NewBlogUrl.Text };
                db.Platnosci.Add(blog);
                db.SaveChanges();

                Blogs.ItemsSource = db.Platnosci.ToList();
            }
        }

        private void Remove_All(object sender, RoutedEventArgs e)
        {
            using (var db = new OcenaKlientowContext())
            {
               var lista = db.Platnosci.Where(platnosc => platnosc.DataWymag != "123");
                db.Platnosci.RemoveRange(lista);
                db.SaveChanges();
                Blogs.ItemsSource = db.Platnosci.ToList();
            }
        }

        private void Pu2_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PU2));
        }

        private void Pu1_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PU1));
        }
    }
}
