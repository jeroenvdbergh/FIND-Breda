using FIND_Breda.Common;
using FIND_Breda.Model;
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

namespace FIND_Breda.Screen
{
    public sealed partial class SightingsView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SightingsView()
        {
            this.InitializeComponent();

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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            SightingsLabel.Text = LanguageModel.instance.getText(Text.sightingslabel);
            MonumentsCheckBox.Content = LanguageModel.instance.getText(Text.monumentscheckbox);
            BuildingsCheckBox.Content = LanguageModel.instance.getText(Text.buildingscheckbox);
            MarketCheckBox.Content = LanguageModel.instance.getText(Text.marketcheckbox);
            RemainingCheckBox.Content = LanguageModel.instance.getText(Text.remainingcheckbox);
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
