using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Surface.Presentation.Input;
using SurfaceApplication1.Data;

namespace SurfaceApplication1
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1
    {
        private readonly List<Ellipse> _filterCircles;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            _filterCircles = new List<Ellipse>();

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

            var mockData = new InitMockData();
            mockData.Init();

            foreach (var attraction in mockData.Attractions)
            {
                Map.Children.Add(attraction);
                MapLayer.SetPosition(attraction, attraction.Location);
            }
        }


        #region Event Capture
        // A collection of key/value pairs containing the event name 
        // and the text block to display the event to.
        readonly Dictionary<string, TextBlock> _eventBlocks = new Dictionary<string, TextBlock>();
        // A collection of key/value pairs containing the event name  
        // and the number of times the event fired.
        readonly Dictionary<string, int> _eventCount = new Dictionary<string, int>();

        private Location GetLocationByEvent(MouseEventArgs e)
        {
            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location location = Map.ViewportPointToLocation(mousePosition);
            return location;
        }

        private Location GetLocationByEvent(TouchEventArgs e)
        {
            //Get the mouse click coordinates
            TouchPoint touchPosition = e.GetTouchPoint(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location location = Map.ViewportPointToLocation(touchPosition.Position);
            return location;
        }

        void MapWithEvents_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Location location = GetLocationByEvent(e);
            // Updates the count of single mouse clicks.
            ShowEvent("MapWithEvents_MouseLeftButtonUp", location, 0x1869f);
        }

        void MapWithEvents_MouseWheel(object sender, MouseEventArgs e)
        {
            Location location = GetLocationByEvent(e);
            // Updates the count of mouse drag boxes created.
            ShowEvent("MapWithEvents_MouseWheel", location, 0x1869f);
        }

        void MapWithEvents_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Location location = GetLocationByEvent(e);
            // Updates the count of mouse pans.
            ShowEvent("MapWithEvents_MouseLeftButtonDown", location, 0x1869f);
        }

        void MapWithEvents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Location location = GetLocationByEvent(e);
            // Updates the count of mouse double clicks.
            ShowEvent("MapWithEvents_MouseDoubleClick", location, 0x1869f);
        }

        void MapWithEvents_ViewChangeEnd(object sender, MapEventArgs e)
        {
            var location = new Location();
            //Updates the number of times the map view has changed.
            ShowEvent("ViewChangeEnd", location, 0x1869f);
        }

        void MapWithEvents_ViewChangeStart(object sender, MapEventArgs e)
        {
            var location = new Location();
            //Updates the number of times the map view started changing.
            ShowEvent("ViewChangeStart", location, 0x1869f);
        }

        void MapWithEvents_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            var location = new Location();
            // Updates the number of times a map view has changed 
            // during an animation from one location to another.
            ShowEvent("ViewChangeOnFrame", location, 0x1869f);
        }
        void MapWithEvents_TargetViewChanged(object sender, MapEventArgs e)
        {
            var location = new Location();
            // Updates the number of map view changes that occured during
            // a zoom or pan.
            ShowEvent("TargetViewChange", location, 0x1869f);
        }

        void MapWithEvents_ModeChanged(object sender, MapEventArgs e)
        {
            var location = new Location();
            // Updates the number of times the map mode changed.
            ShowEvent("ModeChanged", location, 0x1869f);
        }

        private void MapTouchUpEvent(object sender, TouchEventArgs e)
        {
            Location location = GetLocationByEvent(e);

            ShowEvent("TapGesture", location,
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
        }

        private void MapTouchMoveEvent(object sender, TouchEventArgs e)
        {
            Location location = GetLocationByEvent(e);
            ShowEvent("HoldGesture", location,
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
        }

        private void MapTouchDownEvent(object sender, TouchEventArgs e)
        {
            Location location = GetLocationByEvent(e);

            ShowEvent("TapGesture", location,
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
        }


        void ShowEvent(string eventName, Location location, long tagValue)
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

        private void TagDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // Create a red Ellipse.
                var filterCircle = new Ellipse();

                // Create a SolidColorBrush with a red color to fill the  
                // Ellipse with.
                var mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values.  
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Color.FromArgb(40, 255, 255, 255);
                filterCircle.Fill = mySolidColorBrush;
                filterCircle.StrokeThickness = 2;
                filterCircle.Stroke = Brushes.White;

                // Set the width and height of the Ellipse.
                filterCircle.Width = 200;
                filterCircle.Height = 200;

                // set position
                var canvas = (Canvas)sender;
                Point tp = e.GetTouchPoint(canvas).Position;
                Canvas.SetLeft(filterCircle, tp.X - 100);
                Canvas.SetTop(filterCircle, tp.Y - 100);

                // add to scene
                Wrapper.Children.Add(filterCircle);

                // append circle object with tag id
                filterCircle.Tag = e.TouchDevice.Id;
                _filterCircles.Add(filterCircle);
            }
        }

        private void TagMove(object sender, TouchEventArgs e)
        {
            Ellipse filterCircle = GetCircleByTag(e.TouchDevice.Id);
            if (filterCircle != null && e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // update position
                var canvas = (Canvas)sender;
                Point tp = e.GetTouchPoint(canvas).Position;
                Canvas.SetLeft(filterCircle, tp.X - 100);
                Canvas.SetTop(filterCircle, tp.Y - 100);
            }
        }

        private void TagGone(object sender, TouchEventArgs e)
        {
            Ellipse filterCircle = GetCircleByTag(e.TouchDevice.Id);
            if (filterCircle != null && e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // remove from scene
                Wrapper.Children.Remove(filterCircle);

                // remove from list
                _filterCircles.Remove(filterCircle);
            }
        }

        private Ellipse GetCircleByTag(int tag)
        {
            return _filterCircles.FirstOrDefault(circle => (int)circle.Tag == tag);
        }
    }
}