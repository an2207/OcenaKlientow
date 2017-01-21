﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OcenaKlientow.Model;
using OcenaKlientow.Model.Models;


namespace OcenaKlientow.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
