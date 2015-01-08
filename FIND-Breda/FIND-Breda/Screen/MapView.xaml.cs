using System;
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
using Windows.UI.ViewManagement;
using Windows.Devices.Sensors;
using Windows.UI.Core;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;

namespace FIND_Breda.Screen
{
    public partial class MapView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private bool _routeStatus = false;
        private Geolocator _geo = null;
        private CoreDispatcher _cd;
        public MapControl _mapControl { get; set; }
        public static MapView _mapView = null;
        public static readonly object _padlock = new object();
        private SimpleOrientationSensor _simpleorientation = SimpleOrientationSensor.GetDefault();

        public Dictionary<string, MapIcon> _sightings { get; set; }
        private Ellipse _ellipse;
        public bool _started { get; set; }

        public MapView()
        {
            Debug.WriteLine("mapview instance"); //DEBUG

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Window.Current.SizeChanged += Current_SizeChanged;
            _sightings = new Dictionary<string, MapIcon>();
            _started = false;
            /* Layout goed zetten op landscape als de device al op landscape stond */
            if (_simpleorientation.GetCurrentOrientation() == SimpleOrientation.Rotated90DegreesCounterclockwise)
                this.setToLandscape();

            _cd = Window.Current.CoreWindow.Dispatcher;

            this._mapControl = map;
            _mapView = this;

            _routeStatus = true;
        
        }

        public static MapView instance
        {
            get
            {
                return _mapView;
            }
        }

        #region /* Methodes om de layout aan te passen aan de hand van de orientation */
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

            if (CurrentViewState == "Portrait")
            {

                Grid.SetRow(map, 0);
                Grid.SetColumn(map, 0);
                Grid.SetRowSpan(map, 2);
                Grid.SetColumnSpan(map, 3);

                Grid.SetRow(SettingsScrollViewer, 2);
                Grid.SetColumn(SettingsScrollViewer, 0);
                Grid.SetRowSpan(SettingsScrollViewer, 1);
                Grid.SetColumnSpan(SettingsScrollViewer, 1);
                Aerial_Checkbox.Margin = new Thickness(4, 0, 4, 0);
                AerialWithRoads_Checkbox.Margin = new Thickness(4, 0, 4, 0);
                Traffic_Checkbox.Margin = new Thickness(4, 0, 4, 0);
                Dark_Checkbox.Margin = new Thickness(4, 0, 4, 0);
                Pedestrian_Checkbox.Margin = new Thickness(4, 0, 4, 0);

                Grid.SetRow(RouteScrollViewer, 2);
                Grid.SetColumn(RouteScrollViewer, 1);
                Grid.SetColumnSpan(RouteScrollViewer, 2);
                Grid.SetRowSpan(RouteScrollViewer, 1);
            }

            if (CurrentViewState == "Landscape")
            {
                setToLandscape();
            }
        }
        private void setToLandscape()
        {
            Grid.SetRow(map, 0);
            Grid.SetColumn(map, 0);
            Grid.SetRowSpan(map, 3);
            Grid.SetColumnSpan(map, 2);

            Grid.SetRow(SettingsScrollViewer, 0);
            Grid.SetColumn(SettingsScrollViewer, 2);
            Grid.SetRowSpan(SettingsScrollViewer, 1);
            Grid.SetColumnSpan(SettingsScrollViewer, 1);
            Aerial_Checkbox.Margin = new Thickness(4, 0, 4, -25);
            AerialWithRoads_Checkbox.Margin = new Thickness(4, 0, 4, -25);
            Traffic_Checkbox.Margin = new Thickness(4, 0, 4, -25);
            Dark_Checkbox.Margin = new Thickness(4, 0, 4, -25);
            Pedestrian_Checkbox.Margin = new Thickness(4, 0, 4, -25);


            Grid.SetRow(RouteScrollViewer, 1);
            Grid.SetColumn(RouteScrollViewer, 2);
            Grid.SetColumnSpan(RouteScrollViewer, 1);
            Grid.SetRowSpan(RouteScrollViewer, 2);
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string prevpage = e.Parameter as string;
            if (prevpage.Contains("MainPage"))
            {
                ShowToastNotification(LanguageModel.instance.getText(Text.gettinglocationmessage));
            }
            else if (prevpage.Contains("RouteView"))
            {
                /* Alle bezienswaardigheden weergeven */
                if (!_started)
                {
                    displaySightings(true);
                    _started = true;
                }
            }
            setToCurrentLocation();
            popupSightings();

            Aerial_Checkbox.Content = LanguageModel.instance.getText(Text.aerialcheckbox);
            AerialWithRoads_Checkbox.Content = LanguageModel.instance.getText(Text.aerialwithroadscheckbox);
            Dark_Checkbox.Content = LanguageModel.instance.getText(Text.darkthemecheckbox);
            Traffic_Checkbox.Content = LanguageModel.instance.getText(Text.trafficcheckbox);
            Pedestrian_Checkbox.Content = LanguageModel.instance.getText(Text.pedestriancheckbox);
            RemoveRouteButton.Content = LanguageModel.instance.getText(Text.removeroutebutton);

            this.navigationHelper.OnNavigatedTo(e);
        }

        /* Mapview op je huidige positie zetten */
        private async void setToCurrentLocation()
        {
            if (_geo == null)
                _geo = new Geolocator() { DesiredAccuracy = PositionAccuracy.High, ReportInterval = 1000 };

            var location = await getLocationAsync();
            await map.TrySetViewAsync(location.Coordinate.Point, 18, 0, 0, MapAnimationKind.Linear);

            _geo.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(geo_PositionChanged);
        }

        /* Eventhandler voor je locatie updaten */
        private async void geo_PositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                var location = await getLocationAsync();
                map.Children.Remove(_ellipse);
                _ellipse = new Ellipse();

                _ellipse.Fill = new SolidColorBrush(Colors.Red);
                _ellipse.Width = 10;
                _ellipse.Height = 10;
                map.Children.Add(_ellipse);
                MapControl.SetLocation(_ellipse, location.Coordinate.Point);
                await map.TrySetViewAsync(location.Coordinate.Point, map.ZoomLevel, 0, 0, MapAnimationKind.Linear);
            });
        }

        /* Methode om je locatie te geven 
         * Gebruik: var location = await getLocationAsync();
         * Returned een Geoposition object
         */
        private async Task<Geoposition> getLocationAsync()
        {
            return await _geo.GetGeopositionAsync();
        }

        private async void popupSightings()
        {
            double latitude;
            double longitude;
            String description;
            while (_routeStatus == true)
            {
                for (int i = 0; i < DatabaseConnection.instance.getSightings().Count; i++)
                {
                    latitude = DatabaseConnection.instance.getSighting(i).Latitude;
                    longitude = DatabaseConnection.instance.getSighting(i).Longitude;
                    var location = await getLocationAsync();
                    if ((location.Coordinate.Latitude - latitude <= 0.00016799999 || latitude - location.Coordinate.Latitude <= 0.00016799999) && location.Coordinate.Longitude - longitude <= 0.000297 || longitude - location.Coordinate.Longitude <= 0.000297)
                      {
                    description = DatabaseConnection.instance.getSighting(i).Description;
                    // DatabaseConnection.instance.getSighting(i).Path;  plaatjes erbij
                    MessageDialog _msgbox = new MessageDialog(description);
                    await _msgbox.ShowAsync();
                    //_routeStatus bij t stoppen van de route op false zetten

                     }
                }
            }
        }
        /* Methode om mapicons aan en uit te zetten */
        private void displaySightings(bool on)
        {

            string name = "sighting";
            if (on == false)
            {
                for (int i = 0; i < DatabaseConnection.instance.getSightings().Count; i++)
                {
                    map.MapElements.Remove(_sightings[String.Format(name + "{0}", i.ToString())]);
                }
                _sightings.Clear();
            }
            else
            {
                for (int i = 0; i < DatabaseConnection.instance.getSightings().Count; i++)
                {
                    MapIcon tempMapIcon = new MapIcon();
                    tempMapIcon.Location = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = DatabaseConnection.instance.getSighting(i).Latitude,
                        Longitude = DatabaseConnection.instance.getSighting(i).Longitude
                    });
                    tempMapIcon.Title = DatabaseConnection.instance.getSighting(i).Name;
                    _sightings.Add(String.Format(name + "{0}", i.ToString()), tempMapIcon);
                    map.MapElements.Add(tempMapIcon);
                }
            }
        }
        private void RemoveRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_started)
            {
                RouteTextBlock.Text = String.Empty;
                map.Routes.Clear();
                displaySightings(false);
                _started = false;
            }
        }

        #region /* De route van breda */
        public async void SetRouteDirectionsBreda()
        {

            /* Sla een lijst op met waypoints waar je langs moet komen.
             * Deze waypoints worden via de database opgehaald.
             * Deze lijst van waypoints geef je mee in de MapRouteFinder.GetWalkingRouteFromWaypointsAsync() methode 
             */
            List<Geopoint> waypoints = new List<Geopoint>();

            /* Hier voeg je alle bezienswaardigheden toe aan de route */
            foreach (var sighting in DatabaseConnection.instance.getSightings())
            {
                var temploc = new Geopoint(new BasicGeoposition()
                {
                    Latitude = sighting.Latitude,
                    Longitude = sighting.Longitude
                });
                waypoints.Add(temploc);
            }

            /* Haalt de route op en slaat deze op in een variable genaamd routeResult, aan deze variable vraag je de info */
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(waypoints);

            /* Als het programma succesvol de route heeft opgehaald */
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                RouteTextBlock.Text = String.Empty;
                int seconds = routeResult.Route.EstimatedDuration.Seconds;
                RouteTextBlock.Inlines.Add(new Run() { Text = LanguageModel.instance.getText(Text.time) + " " + string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds / 60) % 60, seconds % 60) });
                RouteTextBlock.Inlines.Add(new LineBreak());
                RouteTextBlock.Inlines.Add(new Run() { Text = LanguageModel.instance.getText(Text.totaldistance) + " " + routeResult.Route.LengthInMeters.ToString("0") });
                RouteTextBlock.Inlines.Add(new LineBreak());
                RouteTextBlock.Inlines.Add(new Run() { Text = "Route:" });
                RouteTextBlock.Inlines.Add(new LineBreak());

                /* Print de routebeschrijving in een textblock */
                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                {
                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                    {
                        RouteTextBlock.Inlines.Add(new Run()
                        {
                            Text = "- " + maneuver.InstructionText + " (" + maneuver.LengthInMeters + "m)"
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
        #endregion

        #region /* Methode om notificatie te sturen */
        private void ShowToastNotification(String message)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            // Set Text
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(message));

            // Set image
            // Images must be less than 200 KB in size and smaller than 1024 x 1024 pixels.
            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///Images/logo-80px-80px.png");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");

            // toast duration
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            // toast navigation
            var toastNavigationUriString = "#/MainPage.xaml?param1=12345";
            var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
            toastElement.SetAttribute("launch", toastNavigationUriString);

            // Create the toast notification based on the XML content you've specified.
            ToastNotification toast = new ToastNotification(toastXml);

            // Send your toast notification.
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        #endregion

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
