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
using Windows.UI.Xaml.Controls.Maps;
using FIND_Breda.Screen;
using System.Diagnostics;
using Windows.UI;
using FIND_Breda.Model;
using Windows.UI.Popups;

namespace FIND_Breda
{
    public sealed partial class MainPage : Page
    {
        private MessageDialog _msgbox;
        private static MainPage _mainPage = null;
        private static readonly object _padlock = new object();

        public MainPage()
        {
            if (_mainPage == null)
            {
                Debug.WriteLine("mainpage instance"); //DEBUG
                this.InitializeComponent();
                this.NavigationCacheMode = NavigationCacheMode.Required;
                _mainPage = this;
            }
        }

        public static MainPage instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_mainPage == null)
                    {
                        _mainPage = new MainPage();
                    }
                    return _mainPage;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void PlannedRouteButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RouteView));
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapView));
        }

        private async void SightingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MapView.instance != null)
                Frame.Navigate(typeof(SightingsView));
            else
            {
                _msgbox = new MessageDialog(LanguageModel.instance.getText(Text.loadmapfirst));
                await _msgbox.ShowAsync();
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HelpView));
        }

        private void DutchLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageModel.instance.setLanguage(Model.Language.dutch);

            this.DutchLanguageButton.Background = new SolidColorBrush(Colors.DarkGray);
            this.EnglishLanguageButton.Background = new SolidColorBrush(Colors.Black);

            PlannedRouteButton.Content = LanguageModel.instance.getText(Text.plannedroutebutton);
            MapButton.Content = LanguageModel.instance.getText(Text.mapbutton);
            SightingsButton.Content = LanguageModel.instance.getText(Text.sightingbutton);
            HelpButton.Content = LanguageModel.instance.getText(Text.helpbutton);
        }

        private void EnglishLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageModel.instance.setLanguage(Model.Language.english);

            this.DutchLanguageButton.Background = new SolidColorBrush(Colors.Black);
            this.EnglishLanguageButton.Background = new SolidColorBrush(Colors.DarkGray);

            PlannedRouteButton.Content = LanguageModel.instance.getText(Text.plannedroutebutton);
            MapButton.Content = LanguageModel.instance.getText(Text.mapbutton);
            SightingsButton.Content = LanguageModel.instance.getText(Text.sightingbutton);
            HelpButton.Content = LanguageModel.instance.getText(Text.helpbutton);
        }
    }
}
