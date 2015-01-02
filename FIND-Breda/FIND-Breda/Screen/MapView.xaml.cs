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
using System.Diagnostics;
using FIND_Breda.Common;
using FIND_Breda.GPSHandler;
using Windows.Storage.Streams;

namespace FIND_Breda.Screen
{
    public partial class MapView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Geolocator geo = null;

        public MapControl _mapControl { get; set; }
        public static MapView _mapView = null;
        public static readonly object _padlock = new object();

        public MapIcon _sighting1 { get; set; }
        public MapIcon _sighting2 { get; set; }

        public MapView()
        {
            Debug.WriteLine("mapview instance"); //DEBUG

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this._mapControl = map;
            _mapView = this;
            displaySightings();
        }
        public static MapView instance
        {
            get
            {
                return _mapView;
            }
        }

        private async void map_Loaded(object sender, RoutedEventArgs e)
        {
            // Geopoint breda = new Geopoint(new BasicGeoposition() { Latitude = 51.5940, Longitude = 4.7795 });
            // await map.TrySetViewAsync(breda, 15, 0, 0, MapAnimationKind.Bow);
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (geo == null)
                geo = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };

            // Hier kan je de text van buttons o.i.d. aanpassen
            if (MainPage.instance._isDutch)
            {
                GetLocationAsyncButton.Content = "Geef locatie async";
                Aerial_Checkbox.Content = "Lucht weergave";
                AerialWithRoads_Checkbox.Content = "Lucht+wegen weergave";
                Dark_Checkbox.Content = "Donker thema";
                Traffic_Checkbox.Content = "Verkeer";
                Pedestrian_Checkbox.Content = "Voetganger";

            }
            else
            {
                GetLocationAsyncButton.Content = "Get location async";
                Aerial_Checkbox.Content = "Aerial view";
                AerialWithRoads_Checkbox.Content = "Aerial+roads view";
                Dark_Checkbox.Content = "Dark theme";
                Traffic_Checkbox.Content = "Traffic";
                Pedestrian_Checkbox.Content = "Walker";
            }
            Geoposition location = await geo.GetGeopositionAsync();
            var lat = location.Coordinate.Point.Position.Latitude;
            var lon = location.Coordinate.Point.Position.Longitude;

            // TODO: Map center zetten op je huidige locatie
            // Latitude = lat
            // Longitude = lon
            Geopoint breda = new Geopoint(new BasicGeoposition() { Latitude = 51.5940, Longitude = 4.7795 });
            await map.TrySetViewAsync(breda, 15, 0, 0, MapAnimationKind.Bow);

            this.navigationHelper.OnNavigatedTo(e);
        }

        private async void GetLocationAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            Geoposition location = await geo.GetGeopositionAsync();

            var pin = new MapIcon()
            {
                Location = location.Coordinate.Point,
                Title = "You are here!",
                //  Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pin.png")),
                // NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            };

            textLatitude.Text = "Latitude: " + location.Coordinate.Point.Position.Latitude.ToString();
            textLongitude.Text = "Longitude: " + location.Coordinate.Point.Position.Longitude.ToString();
            textAccuracy.Text = "Accuracy: " + location.Coordinate.Accuracy.ToString() + " Zoomlvl: " + map.ZoomLevel;

            map.MapElements.Add(pin);
            await map.TrySetViewAsync(location.Coordinate.Point, 16D, 0, 0, MapAnimationKind.Bow);
        }
        private void displaySightings()
        {
            _sighting1 = new MapIcon();
            _sighting1.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = 51.5940,
                Longitude = 4.7795
            });
            _sighting1.Title = "VVV";
            map.MapElements.Add(_sighting1);

            _sighting2 = new MapIcon();
            _sighting2.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = 51.5936,
                Longitude = 4.7795
            });
            _sighting2.Title = "Liefdeszuster";
            map.MapElements.Add(_sighting2);
            Windows.UI.Xaml.Shapes.Ellipse fence = new Windows.UI.Xaml.Shapes.Ellipse();
            map.Children.Add(fence);
            //map.SetNormalizedAnchorPoint(fence, new Point(0.5, 0.5));
            map.Children.Add(new Button());
        }
        #region mapsettings
        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            map.ColorScheme = MapColorScheme.Dark;
        }

        private void Dark_Unchecked(object sender, RoutedEventArgs e)
        {
            map.ColorScheme = MapColorScheme.Light;
        }
        private void Traffic_Checked(object sender, RoutedEventArgs e)
        {
            map.TrafficFlowVisible = true;
        }

        private void Traffic_Unchecked(object sender, RoutedEventArgs e)
        {
            map.TrafficFlowVisible = false;
        }

        private void Pedestrian_Checked(object sender, RoutedEventArgs e)
        {
            map.PedestrianFeaturesVisible = true;
        }

        private void Pedestrian_Unchecked(object sender, RoutedEventArgs e)
        {
            map.PedestrianFeaturesVisible = false;
        }

        private void Aerial_Checked(object sender, RoutedEventArgs e)
        {
            map.Style = MapStyle.Aerial;
        }

        private void Map_NoStyle(object sender, RoutedEventArgs e)
        {
            map.Style = MapStyle.Road;
        }

        private void AerialWithRoads_Checked(object sender, RoutedEventArgs e)
        {
            map.Style = MapStyle.AerialWithRoads;
        }
        #endregion

        #region navigationhelpers
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
        #endregion
    }
}
