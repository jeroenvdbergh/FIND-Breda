﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
using FIND_Breda.Model;
using Windows.Storage.Streams;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using System.Threading.Tasks;

namespace FIND_Breda.Screen
{
    public partial class MapView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private Geolocator _geo = null;
        public MapControl _mapControl { get; set; }
        public static MapView _mapView = null;
        public static readonly object _padlock = new object();

        /* Twee test bezienswaardigheden, later uit een databse halen */
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
            //map.Language = "fr-FR";

            this._mapControl = map;
            _mapView = this;

            /* De kaart goedzetten op basis van je locatie */
            setToCurrentLocation();

            /* Alle bezienswaardigheden weergeven */
            displaySightings();
        }

        public static MapView instance
        {
            get
            {
                return _mapView;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetLocationAsyncButton.Content = LanguageModel.instance.getText(Text.getlocationbutton);
            Aerial_Checkbox.Content = LanguageModel.instance.getText(Text.aerialcheckbox);
            AerialWithRoads_Checkbox.Content = LanguageModel.instance.getText(Text.aerialwithroadscheckbox);
            Dark_Checkbox.Content = LanguageModel.instance.getText(Text.darkthemecheckbox);
            Traffic_Checkbox.Content = LanguageModel.instance.getText(Text.trafficcheckbox);
            Pedestrian_Checkbox.Content = LanguageModel.instance.getText(Text.pedestriancheckbox);

            this.navigationHelper.OnNavigatedTo(e);
        }

        private async void GetLocationAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            var location = await getLocationAsync();
            var pin = new MapIcon()
            {
                Location = location.Coordinate.Point,
                Title = "you are here!",
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            };

            textLatitude.Text = "latitude: " + location.Coordinate.Point.Position.Latitude.ToString();
            textLongitude.Text = "longitude: " + location.Coordinate.Point.Position.Longitude.ToString();
            textAccuracy.Text = "accuracy: " + location.Coordinate.Accuracy.ToString() + " zoomlvl: " + map.ZoomLevel;

            map.MapElements.Add(pin);
            await map.TrySetViewAsync(location.Coordinate.Point, 15, 0, 0, MapAnimationKind.Bow);
        }

        private async void setToCurrentLocation()
        {
            if (_geo == null)
                _geo = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
            var location = await getLocationAsync();
            await map.TrySetViewAsync(location.Coordinate.Point, 15, 0, 0, MapAnimationKind.Bow);
            // Geopoint breda = new Geopoint(new BasicGeoposition() { Latitude = 51.5940, Longitude = 4.7795 });
            // await map.TrySetViewAsync(breda, 15, 0, 0, MapAnimationKind.Bow);
        }

        /* Methode om je locatie te geven 
         * Gebruik: var location = await getLocationAsync();
         * Returned een Geoposition object
         */
        private async Task<Geoposition> getLocationAsync()
        {
            return await _geo.GetGeopositionAsync();
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
        }

        /* De route van breda */
        public async void SetRouteDirectionsBreda()
        {
            string beginLocation = "Willemstraat 17 Breda"; // Begin stadswandeling (VVV-Breda), volgens bestand
            string endLocation = "Reigerstraat 2 Breda";    // Einde stadswandeling volgens bestand

            // Locatie van begin en eindpunt ophalen
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(beginLocation, map.Center);
            MapLocation begin = result.Locations.First();

            result = await MapLocationFinder.FindLocationsAsync(endLocation, map.Center);
            MapLocation end = result.Locations.First();

            /* Sla een lijst op met waypoints waar je langs moet komen.
             * Deze waypoints moeten via de database opgehaald worden.
             * Deze lijst van waypoints geef je mee in de MapRouteFinder.GetWalkingRouteFromWaypointsAsync() methode 
             */
            List<Geopoint> waypoints = new List<Geopoint>();
            waypoints.Add(begin.Point);
            /* Hier tussen voeg je de bezienswaardigheden toe
             * vb. van een bezienswaardigheid: 
             * var sighting1 = new Geopoint(new BasicGeoposition() { Latitude = 51.5940, Longitude = 4.7795 });
             * waypoints.Add(sighting1);
             */
            waypoints.Add(end.Point);
            /* Haalt de route op en slaat deze op in een variable genaamd routeResult, aan deze variable vraag je de info */
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(waypoints);

            Debug.WriteLine(routeResult.Status); // DEBUG

            /* Als het programma succesvol de route heeft opgehaald */
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                RouteTextBlock.Inlines.Add(new Run() { Text = routeResult.Route.EstimatedDuration.TotalMinutes.ToString() });
                RouteTextBlock.Inlines.Add(new LineBreak());
                RouteTextBlock.Inlines.Add(new Run() { Text = routeResult.Route.LengthInMeters.ToString() });
                RouteTextBlock.Inlines.Add(new LineBreak());
                /* Print de routebeschrijving in een textblock */
                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                {
                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                    {
                        RouteTextBlock.Inlines.Add(new Run()
                        {
                            Text = maneuver.InstructionText + " (" + maneuver.LengthInMeters + "m)"
                        });
                        RouteTextBlock.Inlines.Add(new LineBreak());
                    }
                }

                /* Tekenen van de route op de kaart */
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Blue;
                viewOfRoute.OutlineColor = Colors.Black;

                map.Routes.Add(viewOfRoute);

                /* De kaart goed schalen op de map */
                await map.TrySetViewBoundsAsync(routeResult.Route.BoundingBox, null, MapAnimationKind.Bow);
            }
            else
            {
                throw new Exception(routeResult.Status.ToString());
            }
        }

        #region Map settings om de map aan te passen
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

        #region Autogenerated navigationhelpers
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
