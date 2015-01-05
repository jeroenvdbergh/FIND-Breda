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
using Windows.UI.ViewManagement;

namespace FIND_Breda
{
    public sealed partial class MainPage : Page
    {
        private MessageDialog _msgbox;
        private static MainPage _mainPage = null;
        private static readonly object _padlock = new object();
        private DatabaseConnection _database;

        public MainPage()
        {
            if (_mainPage == null)
            {
                Debug.WriteLine("mainpage instance"); //DEBUG
                this.InitializeComponent();
                this.NavigationCacheMode = NavigationCacheMode.Required;
                Window.Current.SizeChanged += Current_SizeChanged;
                _mainPage = this;
                _database = DatabaseConnection.instance;
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

        /* Methode om de button layout aan te passen aan de hand van de orientation */
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

            if (CurrentViewState == "Portrait")
            {
                this.MenuGrid.Height = 566;
            }

            if (CurrentViewState == "Landscape")
            {
                this.MenuGrid.Height = 350;
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
            Frame.Navigate(typeof(MapView), this.ToString());
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
