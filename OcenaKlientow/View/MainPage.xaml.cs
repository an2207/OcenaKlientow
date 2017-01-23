using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OcenaKlientow.View
{
    public sealed partial class MainPage : Page
    {
        #region Ctors

        public MainPage()
        {
            InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        #endregion
        #region Event handlers

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Pu1_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PU1));
        }

        private void Pu2_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PU2));
        }

        #endregion
    }
}