using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CityGuide.Data;
using CityGuide.Data.Route;
using CityGuide.ViewElements;
using CityGuide.Extensions;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Surface.Presentation.Input;

namespace CityGuide
{
    /// <summary>
    /// Interaction logic for CityGuideWindow.xaml
    /// </summary>
    public partial class CityGuideWindow
    {
        #region Fields
        private Dictionary<long, TagCircle> _filterCircles;
        private readonly MapLayer _pushPinsMapLayer = new MapLayer { Name = "PushPins", CacheMode = new BitmapCache() };
        private readonly MapLayer _pushPinsInfosMapLayer = new MapLayer { Name = "PushPinsInfos", CacheMode = new BitmapCache() };
        private readonly MapLayer _routeMapLayer = new MapLayer { Name = "Routes"};
        private readonly CoolDownTimer _coolDownTimer = new CoolDownTimer(0, 0, 1) { Name = "DoubleTouchCoolDownTimer" };
        private readonly Point _diffPoint = new Point { X = 200.00, Y = 200.00 };
        private Point _lastTapPosition;
        private int FILTER_INTEREST = 5;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CityGuideWindow()
        {
            try
            {
                InitializeComponent();

                Map.Children.Add(_routeMapLayer);
                Map.Children.Add(_pushPinsMapLayer);
                Map.Children.Add(_pushPinsInfosMapLayer);
                AddTouchAndMouseEventsForDebbugConsole();

                AddPushPins();

                CurrentLocationButton.TouchDown += CurrentLocationButtonTouchDown;
                CurrentLocationButton.Click += CurrentLocationButtonMouseDown;

                Reset.Click += ResetMouseClick;
                Reset.TouchUp += ResetTouchUp;

                TimeTable.RoutMapLayer = _routeMapLayer;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message + "\n" + e.StackTrace,
                    "Error during Initialization",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #region Methods
        #region Init Methods

        #region PushPin
        private void AddPushPins()
        {
            var mockData = MockData.Init(CnvDraw, CnvInteract);
            _filterCircles = mockData.TagViewItems;

            foreach (var attraction in mockData.Attractions)
            {
                attraction.TouchDown += TouchDownEventAttraction;
                attraction.MouseDown += MouseDownEventAttraction;
                if (attraction.Interest <= FILTER_INTEREST)
                {
                    attraction.Opacity = 0.5;
                }
                _pushPinsMapLayer.AddChild(attraction, attraction.Location);
            }
        }

        private void TouchDownEventAttraction(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsTagRecognized())
            {
                Point positionPoint = e.GetTouchPoint(this).Position;
                AddAttractionInfobox(sender, positionPoint);
                e.Handled = false;
            }
        }

        private void MouseDownEventAttraction(object sender, MouseButtonEventArgs e)
        {
            Point positionPoint = e.GetPosition(this);
            AddAttractionInfobox(sender, positionPoint);
            e.Handled = false;
        }

        private void AddAttractionInfobox(object sender, Point positionPoint)
        {
            var attraction = sender as Attraction;
            if (attraction != null)
            {
                var infoBox = new InfoBox(attraction);

                Canvas.SetLeft(infoBox, positionPoint.X - 100);
                Canvas.SetTop(infoBox, positionPoint.Y - 100);

                InfoBoxContainer.Items.Add(infoBox);
            }
        }
        #endregion

        private void AddTouchAndMouseEventsForDebbugConsole()
        {
            //***************** Test Mous Events ***********************//
            // Fires every animated frame from one location to another.
            Map.ViewChangeOnFrame +=
                MapWithEvents_ViewChangeOnFrame;
            // Fires when the map view location has changed.
            Map.TargetViewChanged +=
                MapWithEvents_TargetViewChanged;
            // Fires when the map view starts to move to its new target view.
            Map.ViewChangeStart +=
                MapWithEvents_ViewChangeStart;
            // Fires when the map view has reached its new target view.
            Map.ViewChangeEnd +=
                MapWithEvents_ViewChangeEnd;
            // Fires when a different mode button on the navigation bar is selected.
            Map.ModeChanged +=
                MapWithEvents_ModeChanged;
            // Fires when the mouse is double clicked
            Map.MouseDoubleClick +=
                MapWithEvents_MouseDoubleClick;
            // Fires when the mouse wheel is used to scroll the map
            Map.MouseWheel +=
                MapWithEvents_MouseWheel;
            // Fires when the left mouse button is depressed
            Map.MouseLeftButtonDown +=
                MapWithEvents_MouseLeftButtonDown;
            // Fires when the left mouse button is released
            Map.MouseLeftButtonUp +=
                MapWithEvents_MouseLeftButtonUp;

            Map.TouchDown += MapTouchDownEvent;
            Map.TouchMove += MapTouchMoveEvent;
            Map.TouchUp += MapTouchUpEvent;
        }
        #endregion

        #region Mouse and Touch Event Debug Console Methods
        // A collection of key/value pairs containing the event name 
        // and the text block to display the event to.
        private readonly Dictionary<string, TextBlock> _eventBlocks = new Dictionary<string, TextBlock>();
        // A collection of key/value pairs containing the event name  
        // and the number of times the event fired.
        private readonly Dictionary<string, int> _eventCount = new Dictionary<string, int>();

        private void MapWithEvents_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            var location = new Location();
            // Updates the count of single mouse clicks.
            ShowEvent("MapWithEvents_MouseLeftButtonUp", location.GetLocationByEvent(e, Map, this), 0x1869f);
        }

        private void MapWithEvents_MouseWheel(object sender, MouseEventArgs e)
        {
            var location = new Location();
            // Updates the count of mouse drag boxes created.
            ShowEvent("MapWithEvents_MouseWheel", location.GetLocationByEvent(e, Map, this), 0x1869f);
        }

        private void MapWithEvents_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            var location = new Location();
            // Updates the count of mouse pans.
            ShowEvent("MapWithEvents_MouseLeftButtonDown", location.GetLocationByEvent(e, Map, this), 0x1869f);
        }

        private void MapWithEvents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var location = new Location();
            // Updates the count of mouse double clicks.
            ShowEvent("MapWithEvents_MouseDoubleClick", location.GetLocationByEvent(e, Map, this), 0x1869f);
            e.Handled = true;
        }

        private void MapWithEvents_ViewChangeEnd(object sender, MapEventArgs e)
        {
            var location = new Location();
            //Updates the number of times the map view has changed.
            ShowEvent("ViewChangeEnd", location, 0x1869f);
        }

        private void MapWithEvents_ViewChangeStart(object sender, MapEventArgs e)
        {
            var location = new Location();
            //Updates the number of times the map view started changing.
            ShowEvent("ViewChangeStart", location, 0x1869f);
        }

        private void MapWithEvents_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            var location = new Location();
            // Updates the number of times a map view has changed 
            // during an animation from one location to another.
            ShowEvent("ViewChangeOnFrame", location, 0x1869f);
        }

        private void MapWithEvents_TargetViewChanged(object sender, MapEventArgs e)
        {
            var location = new Location();
            // Updates the number of map view changes that occured during
            // a zoom or pan.
            ShowEvent("TargetViewChange", location, 0x1869f);
        }

        private void MapWithEvents_ModeChanged(object sender, MapEventArgs e)
        {
            var location = new Location();
            // Updates the number of times the map mode changed.
            ShowEvent("ModeChanged", location, 0x1869f);
        }

        private void MapTouchUpEvent(object sender, TouchEventArgs e)
        {
            var location = new Location();

            ShowEvent("TapGesture", location.GetLocationByEvent(e, Map, this),
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
            if (TagGone(e))
            {
                e.Handled = true;
            }
        }

        private void MapTouchMoveEvent(object sender, TouchEventArgs e)
        {
            var location = new Location();
            ShowEvent("HoldGesture", location.GetLocationByEvent(e, Map, this),
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
            if (TagMove(e))
            {
                e.Handled = true;
            }

            foreach (TagCircle t in _filterCircles.Values)
            {
                if (t.cLocation != null)
                {
                    // get the touch location of the tag
                    Location touchLocation = Map.ViewportPointToLocation(t.cPosition);

                    // update resolution
                    double resolution = 156543.04 * Math.Cos(Deg2Rad(touchLocation.Latitude)) / (Math.Pow(2, Map.ZoomLevel));
                    t.UpdateResolution(resolution);
                    t.cLocation = touchLocation;

                    // update position
                    t.Filter.LocationCenter = t.Filter.LocationCenter.GetLocationByEvent(t.cPosition, Map);
                    FilterPins(t.Filter, t.cLocation, t.GetRadius());
                }
            }
        }

        private void MapTouchDownEvent(object sender, TouchEventArgs e)
        {
            //Doubel Touch disabler
            var currentPosition = e.TouchDevice.GetTouchPoint(this).Position;
            var diffResult = currentPosition.IsInTolleranz(_lastTapPosition, _diffPoint);
            if (diffResult && _coolDownTimer.Enabled)
            {
                e.Handled = true;
                _coolDownTimer.Stop();
            }
            else
            {
                _coolDownTimer.StartWithReset();
                _lastTapPosition = currentPosition;
            }

            var location = new Location();
            ShowEvent("TapGesture", location.GetLocationByEvent(e, Map, this),
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
            if (TagDown(e))
            {
                e.Handled = true;
            }
        }


        private void ShowEvent(string eventName, Location location, long tagValue)
        {
            // Updates the display box showing the number of times 
            // the wired events occured.
            if (!_eventBlocks.ContainsKey(eventName))
            {
                var tb = new TextBlock
                {
                    Foreground = new SolidColorBrush(
                        Color.FromArgb(255, 128, 255, 128)),
                    Margin = new Thickness(5)
                };
                _eventBlocks.Add(eventName, tb);
                _eventCount.Add(eventName, 0);
                EventsPanel.Children.Add(tb);
            }

            _eventCount[eventName]++;
            _eventBlocks[eventName].Text = String.Format(
                "{0}: [{1} times] {2} (HH:mm:ss:ffff) {3}, Tag Value: {4}",
                eventName, _eventCount[eventName], DateTime.Now, location.Latitude + "," + location.Longitude, tagValue);
        }
        #endregion

        #region Tag Related Metheods
        private bool TagDown(TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && _filterCircles.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                Point tp = e.GetTouchPoint(CnvInteract).Position;
                TagCircle filterCircle = _filterCircles[e.TouchDevice.GetTagData().Value];

                // get the touch location of the tag
                Location touchLocation = Map.ViewportPointToLocation(e.GetTouchPoint(Map).Position);

                // update resolution
                double resolution = 156543.04 * Math.Cos(Deg2Rad(touchLocation.Latitude)) / (Math.Pow(2, Map.ZoomLevel));
                filterCircle.UpdateResolution(resolution);
                filterCircle.cLocation = touchLocation;
                filterCircle.cPosition = e.GetTouchPoint(Map).Position;

                // update filter opacity
                FilterPins(filterCircle.Filter, touchLocation, filterCircle.GetRadius());

                if (!filterCircle.IsDrawn)
                {
                    filterCircle.Filter.LocationCenter = filterCircle.Filter.LocationCenter.GetLocationByEvent(tp, Map);

                    filterCircle.UpdateTransform(tp.X, tp.Y, e.TouchDevice.GetOrientation(CnvDraw));

                    filterCircle.UpdateSize();
                    filterCircle.Draw(this);
                    return true;
                }
            }
            return false;
        }

        private bool TagMove(TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && _filterCircles.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                Point tp = e.GetTouchPoint(CnvDraw).Position;
                TagCircle filterCircle = _filterCircles[e.TouchDevice.GetTagData().Value];

                // get the touch location of the tag
                Location touchLocation = Map.ViewportPointToLocation(e.GetTouchPoint(Map).Position);

                // update resolution
                double resolution = 156543.04 * Math.Cos(Deg2Rad(touchLocation.Latitude)) / (Math.Pow(2, Map.ZoomLevel));
                filterCircle.UpdateResolution(resolution);
                filterCircle.cLocation = touchLocation;
                filterCircle.cPosition = e.GetTouchPoint(Map).Position;

                // update filter opacity
                FilterPins(filterCircle.Filter, touchLocation, filterCircle.GetRadius());

                // update position
                filterCircle.Filter.LocationCenter = filterCircle.Filter.LocationCenter.GetLocationByEvent(tp, Map);
                filterCircle.UpdateTransform(tp.X, tp.Y, e.TouchDevice.GetOrientation(CnvDraw));
                filterCircle.UpdateSize();

                return true;
            }
            return false;
        }

        private bool TagGone(TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && _filterCircles.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                TagCircle filterCircle = _filterCircles[e.TouchDevice.GetTagData().Value];
                filterCircle.Filter.LocationCenter = filterCircle.Filter.LocationCenter.GetLocationByEvent(e, Map, this);

                // remove from scene
                filterCircle.Clear();

                // remove filter opacity
                Location touchLocation = Map.ViewportPointToLocation(e.GetTouchPoint(Map).Position);
                FilterPins(filterCircle.Filter, touchLocation, -1);

                return true;
            }
            return false;
        }
        #endregion

        #region Map Related Methods
        private void CurrentLocationButtonMouseDown(object sender, RoutedEventArgs e)
        {
            SetMapNorthAndMannheimAsCenter();
        }

        private void CurrentLocationButtonTouchDown(object sender, TouchEventArgs e)
        {
            SetMapNorthAndMannheimAsCenter();
        }

        private void DrawRoute(IEnumerable<MapPolyline> routePolylines)
        {
            foreach (var routePolyline in routePolylines)
            {
                _routeMapLayer.Children.Add(routePolyline);
            }
        }
        #endregion

        #region Helper Methods
        public void FilterPins(Filter filter, Location location, double radius)
        {
            foreach (Attraction a in _pushPinsMapLayer.Children)
            {
                if (a.Filter == filter)
                {
                    // if reset or pin in filter range: full opacity
                    if ((radius == -1 && a.Interest > FILTER_INTEREST)  || GetDistance(location, a.Location) < (radius / 1000.0))
                    {
                        a.Opacity = 1;
                    }
                    else // else fade
                    {
                        a.Opacity = 0.5;
                    }
                }
            }
        }

        private double GetDistance(Location l1, Location l2)
        {
            int R = 6371; // Radius of the earth in km
            double dLat = Deg2Rad(l1.Latitude - l2.Latitude);
            double dLon = Deg2Rad(l1.Longitude - l2.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(Deg2Rad(l1.Latitude)) * Math.Cos(Deg2Rad(l2.Latitude)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        #endregion

        private void ResetMouseClick(object sender, RoutedEventArgs e)
        {
            ResetTimeTableAndMapPosition();
        }

        private void ResetTouchUp(object sender, TouchEventArgs e)
        {
            ResetTimeTableAndMapPosition();
        }

        private void ResetTimeTableAndMapPosition()
        {
            TimeTable.Reset();
            SetMapNorthAndMannheimAsCenter();
            MockData.MockDataInstance.ResetMockingData(CnvDraw, CnvInteract);
        }

        private void SetMapNorthAndMannheimAsCenter()
        {
            Map.Center = new Location(49.479886, 8.469992, 0.0);
            Map.ZoomLevel = 15.00;
            Map.Heading = 0;
        }
        #endregion
    }
}