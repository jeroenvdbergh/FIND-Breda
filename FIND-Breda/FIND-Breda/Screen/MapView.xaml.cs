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
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Phone.UI.Input;

namespace FIND_Breda.Screen
{
    public partial class MapView : Page
    {
        Geolocator geo = null;
        public MapView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (geo == null)
            {
                geo = new Geolocator();
            }
            Geoposition pos = await geo.GetGeopositionAsync();
            var lat = pos.Coordinate.Point.Position.Latitude;
            var lon = pos.Coordinate.Point.Position.Longitude;

            // Hier kan je de text van buttons o.i.d. aanpassen
            if (MainPage.instance._isDutch)
                GetLocationAsyncButton.Content = "Geef locatie async";
            else
                GetLocationAsyncButton.Content = "Get location async";

            // TODO: Map center zetten op je huidige locatie
            map.Center = new Geopoint(new BasicGeoposition()
               {
                   //Latitude = 51.5876935784736,
                   //Longitude = 4.77448171225132

                   Latitude = 51.5940,
                   Longitude = 4.7795

                   //Latitude = lat,
                   //Longitude = lon

               });
            map.ZoomLevel = 15;

            // de bezienswaardigheden weergeven
            displaySightings();
        }

        private async void GetLocationAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            Geoposition pos = await geo.GetGeopositionAsync();
            textLatitude.Text = "Latitude: " + pos.Coordinate.Point.Position.Latitude.ToString();
            textLongitude.Text = "Longitude: " + pos.Coordinate.Point.Position.Longitude.ToString();
            textAccuracy.Text = "Accuracy: " + pos.Coordinate.Accuracy.ToString() + " Zoomlvl: " + map.ZoomLevel;

            await map.TrySetViewAsync(pos.Coordinate.Point, 16D);
        }
        private void displaySightings()
        {
            MapIcon sighting1 = new MapIcon();
            sighting1.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = 51.5940,
                Longitude = 4.7795
            });
            sighting1.Title = "VVV";
            map.MapElements.Add(sighting1);

            MapIcon sighting2 = new MapIcon();
            sighting2.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = 51.5936,
                Longitude = 4.7795
            });
            sighting2.Title = "Liefdeszuster";
            map.MapElements.Add(sighting2);
        }
    }
}
