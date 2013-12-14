using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CityGuide.Data;
using CityGuide.ViewElements;
using CityGuide.Extensions;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace CityGuide
{
    /// <summary>
    /// Interaction logic for CityGuideWindow.xaml
    /// </summary>
    public partial class CityGuideWindow : SurfaceWindow
    {
        private Dictionary<long, TagCircle> _filterCircles;
        private readonly MapLayer _pushPinsMapLayer = new MapLayer { Name = "PushPins" };
        private readonly MapLayer _pushPinsInfosMapLayer = new MapLayer { Name = "PushPinsInfos" };
        private readonly MapLayer _routeMapLayer = new MapLayer { Name = "Routes" };

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CityGuideWindow()
        {
            try
            {
                InitializeComponent();

                Map.Children.Add(this._pushPinsMapLayer);
                Map.Children.Add(this._pushPinsInfosMapLayer);
                Map.Children.Add(this._routeMapLayer);

                AddTouchAndMouseEventsForDebbugConsole();

                AddPushPins();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message + "\n" + e.StackTrace,
                    "Error during Initialization",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        #region Init Methods

        #region PushPin
        private void AddPushPins()
        {
            var mockData = new InitMockData();
            mockData.Init(cnv_Draw, cnv_Interact);
            _filterCircles = mockData.TagViewItems;

            foreach (var attraction in mockData.Attractions)
            {
                attraction.TouchDown += TouchDownEventAttraction;
                attraction.MouseDown += MouseDownEventAttraction;
                attraction.ToolTip = new TextBlock { Text = attraction.Titel + " " + attraction.Address };
                this._pushPinsMapLayer.AddChild(attraction, attraction.Location);
            }
        }

        private void TouchDownEventAttraction(object sender, TouchEventArgs e)
        {
            Point positionPoint = e.GetTouchPoint(this).Position;
            AddAttractionInfobox(sender, positionPoint);
            e.Handled = false;
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
                var infoBox = new TextBox { Width = 200, Height = 200 };

                Canvas.SetLeft(infoBox, positionPoint.X - 100);
                Canvas.SetTop(infoBox, positionPoint.Y - 100);

                infoBox.Uid = attraction.Titel.Replace(' ', '_') + "InfoBox";
                infoBox.Text = attraction.Titel + ", " + attraction.Address + ", " + attraction.Teaser;

                int index = 0;
                if (_pushPinsInfosMapLayer.Children != null)
                {

                    if (!_pushPinsInfosMapLayer.Children.Contains(infoBox, out index))
                    {
                        var location = new Location();
                        _pushPinsInfosMapLayer.AddChild(infoBox, location.GetLocationByEvent(positionPoint, Map));
                    }
                    else
                    {
                        _pushPinsInfosMapLayer.Children.RemoveAt(index);
                    }
                }
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
            if (TagGone(sender, e))
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
            if (TagMove(sender, e))
            {
                e.Handled = true;
            }
        }

        private void MapTouchDownEvent(object sender, TouchEventArgs e)
        {
            var location = new Location();

            ShowEvent("TapGesture", location.GetLocationByEvent(e, Map, this),
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
            if (TagDown(sender, e))
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

        //#region Location Helper Methods
        //private Location GetLocationByEvent(MouseEventArgs e)
        //{
        //    //Get the mouse click coordinates
        //    Point mousePosition = e.GetPosition(this);
        //    //Convert the mouse coordinates to a locatoin on the map
        //    Location location = Map.ViewportPointToLocation(mousePosition);
        //    return location;
        //}

        //private Location GetLocationByEvent(Point mousePosition)
        //{
        //    //Convert the mouse coordinates to a locatoin on the map
        //    Location location = Map.ViewportPointToLocation(mousePosition);
        //    return location;
        //}

        //private Location GetLocationByEvent(TouchEventArgs e)
        //{
        //    //Get the mouse click coordinates
        //    TouchPoint touchPosition = e.GetTouchPoint(this);
        //    //Convert the mouse coordinates to a locatoin on the map
        //    Location location = Map.ViewportPointToLocation(touchPosition.Position);
        //    return location;
        //}
        //#endregion

        #region Tag Related Metheods
        private bool TagDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && _filterCircles.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                Point tp = e.GetTouchPoint(cnv_Interact).Position;
                TagCircle filterCircle = _filterCircles[e.TouchDevice.GetTagData().Value];

                filterCircle.Filter.LocationCenter = filterCircle.Filter.LocationCenter.GetLocationByEvent(tp, Map);

                filterCircle.UpdateTransform(tp.X, tp.Y, e.TouchDevice.GetOrientation(cnv_Draw));
                filterCircle.UpdateSize();
                filterCircle.Draw();
                return true;
            }
            return false;
        }

        private bool TagMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && _filterCircles.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                TagCircle filterCircle = _filterCircles[e.TouchDevice.GetTagData().Value];
                // update position
                Point tp = e.GetTouchPoint(cnv_Draw).Position;
                filterCircle.Filter.LocationCenter = filterCircle.Filter.LocationCenter.GetLocationByEvent(tp, Map);
                filterCircle.UpdateTransform(tp.X, tp.Y, e.TouchDevice.GetOrientation(cnv_Draw));
                filterCircle.UpdateSize();
                return true;
            }
            return false;
        }

        private bool TagGone(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && _filterCircles.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                TagCircle filterCircle = _filterCircles[e.TouchDevice.GetTagData().Value];
                filterCircle.Filter.LocationCenter = filterCircle.Filter.LocationCenter.GetLocationByEvent(e, Map, this);
                // remove from scene
                filterCircle.Clear();

                var coolDownTimer = new CoolDownTimer(0, 0, 10);
                coolDownTimer.OnTimerFinished += delegate
                {
                    if (!filterCircle.IsDrawn)
                    {
                        filterCircle.Filter.Radius = 200;
                    }
                };
                coolDownTimer.PlayPause();

                return true;
            }
            return false;
        }

        #endregion
    }
}