using FIND_Breda.Common;
using FIND_Breda.GPSHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FIND_Breda.Screen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SightingsView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SightingsView()
        {
            this.InitializeComponent();

            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            MarketCheckBox.IsChecked = true;
            MonumentsCheckBox.IsChecked = true;
            RemainingCheckBox.IsChecked = true;
            BuildingsCheckBox.IsChecked = true;
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            // Hier kan je de text van buttons o.i.d. aanpassen
            if (MainPage.instance._isDutch)
            {
                SightingsLabel.Text = "Bezienswaardigheden";
                MonumentsCheckBox.Content = "Monumenten";
                BuildingsCheckBox.Content = "Gebouwen";
                MarketCheckBox.Content = "Markten";
                RemainingCheckBox.Content = "Overig";
            }
            else
            {
                SightingsLabel.Text = "Sightings";
                MonumentsCheckBox.Content = "Monuments";
                BuildingsCheckBox.Content = "Buildings";
                MarketCheckBox.Content = "Marketplaces";
                RemainingCheckBox.Content = "Other sightings";
            }
        }

        private void MonumentsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (MonumentsCheckBox.IsChecked == true)
            {
              //  MapView.instance._mapControl.Opacity = 1;
                MapView.instance._sighting1.Visible = true;
                MapView.instance._sighting2.Visible = true;
            }
            else
            {
               // MapView.instance._mapControl.Opacity = 0;
                MapView.instance._sighting1.Visible = false;
                MapView.instance._sighting2.Visible = false;
            }
        }

        private void BuildingsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (BuildingsCheckBox.IsChecked == true)
            {

            }
            else
            {

            }
        }

        private void MarketCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (MarketCheckBox.IsChecked == true)
            {

            }
            else
            {

            }
        }

        private void RemainingCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (RemainingCheckBox.IsChecked == true)
            {

            }
            else
            {

            }
        }
    }
}
