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
        }
        #region Navigationhelpers
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
            SightingsLabel.Text = LanguageModel.instance.getText(Text.sightingslabel);
            MonumentsCheckBox.Content = LanguageModel.instance.getText(Text.monumentscheckbox);
            BuildingsCheckBox.Content = LanguageModel.instance.getText(Text.buildingscheckbox);
            MarketCheckBox.Content = LanguageModel.instance.getText(Text.marketcheckbox);
            RemainingCheckBox.Content = LanguageModel.instance.getText(Text.remainingcheckbox);
            this.navigationHelper.OnNavigatedTo(e);
        }
        #endregion

        /* Methode om mapicons aan en uit te zetten
         * toggleMapIconVisibility(new int[] { 0, 1,2,3,4 },true)
         *                                          ^          ^
         *                                    nummers van      true = aan, false = uit
         *                                de bezienswaardig-
         *                                       heden
         */
        private void toggleMapIconVisibility(int[] icons, bool boolean)
        {
            string name = "sighting";
            for (int i = 0; i < MapView.instance._sightings.Count; i++)
            {
                for (int j = 0; j < icons.Length; j++)
                {
                    string dynamicvar = String.Format(name + "{0}", i.ToString());
                    if (dynamicvar.Contains(icons[j].ToString()))
                    {
                        MapView.instance._sightings[dynamicvar].Visible = boolean;
                        // Debug.WriteLine(MapView.instance._sightings["sighting1"] + "@" +  dynamicvar);
                        Debug.WriteLine("Toggled " + dynamicvar + " " + boolean);
                    }
                }
            }
        }

        /* TODO: sightings aan en uit kunnen zetten
         * Bijvoorbeeld sighting0 is het VVV gebouw
         * in BuildingsCheckBox_Click als de checkbox aangeklikt is doe je:
         * toggleMapIconVisibility(new int[] { 0, 1,2,3,4 },true)
         * 0 is vvv building, 1 is voor een ander gebouw, 2 nog een gebouw etc...
         * Dit doen voor alle 45 sightings, staat op volgorde volgens het document
         * Bij 0 begint het, dus sighting0 is eerste bezienswaardigheid namelijk vvv
         */
        private void MonumentsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (MonumentsCheckBox.IsChecked == true)
            {
                toggleMapIconVisibility(new int[] { 0 }, true);
            }
            else
            {
                toggleMapIconVisibility(new int[] { 0 }, false);
            }
        }

        private void BuildingsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (BuildingsCheckBox.IsChecked == true)
            {
                toggleMapIconVisibility(new int[] { 0 }, true);
            }
            else
            {
                toggleMapIconVisibility(new int[] { 0 }, false);
            }
        }

        private void MarketCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (MarketCheckBox.IsChecked == true)
            {
                toggleMapIconVisibility(new int[] { 0 }, true);
            }
            else
            {
                toggleMapIconVisibility(new int[] { 0 }, false);
            }
        }

        private void RemainingCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (RemainingCheckBox.IsChecked == true)
            {
                toggleMapIconVisibility(new int[] { 0 }, true);
            }
            else
            {
                toggleMapIconVisibility(new int[] { 0 }, false);
            }
        }
    }
}
