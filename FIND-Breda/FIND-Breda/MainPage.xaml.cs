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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace FIND_Breda
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool _isDutch { get; set; }
        private static MainPage _mainPage = null;
        private static readonly object _padlock = new object();

        public MainPage()
        {
            if (_mainPage == null)
            {
                Debug.WriteLine("instance");
                this._isDutch = true;
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

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HelpView));
        }

        private void PlannedRouteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapView));
        }

        private void BezienswaardighedenButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SightingsView));
        }

        private void DutchLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            this._isDutch = true;

            SightingsButton.Content = "Bezienswaardigheden";
            PlannedRouteButton.Content = "Voorgeplande route kiezen";
            MapButton.Content = "Kaart";
            HelpButton.Content = "Help / Info";
        }

        private void EnglishLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            this._isDutch = false;

            SightingsButton.Content = "Sightings";
            PlannedRouteButton.Content = "Choose planned route";
            MapButton.Content = "Map";
            HelpButton.Content = "Help / Info";
        }
    }
}
