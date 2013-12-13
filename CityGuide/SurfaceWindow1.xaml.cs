using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Maps.MapControl.WPF;
using SurfaceApplication1.Data;

namespace SurfaceApplication1
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private List<TagCircle> filterCircles;
        private Dictionary<long, Color> filterColors;

        private readonly MapLayer _pushPinsMapLayer = new MapLayer { Name = "PushPins" };
        private readonly MapLayer _pushPinsInfosMapLayer = new MapLayer { Name = "PushPinsInfos" };
        private readonly MapLayer _routeMapLayer = new MapLayer { Name = "Routes" };

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            try
            {
                InitializeComponent();
                filterCircles = new List<TagCircle>();

                filterColors = new Dictionary<long, Color>();
                filterColors.Add(0x00, Color.FromArgb(90, 16, 143, 151));
                filterColors.Add(0x01, Color.FromArgb(90, 255, 139, 107));
                filterColors.Add(0x02, Color.FromArgb(90, 255, 227, 159));
                filterColors.Add(0x03, Color.FromArgb(90, 22, 134, 109));
                filterColors.Add(0x64, Color.FromArgb(90, 16, 54, 54));

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
            mockData.Init();

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
                if (!ContainsUIElement(infoBox, _pushPinsInfosMapLayer.Children, out index))
                {
                    _pushPinsInfosMapLayer.AddChild(infoBox, GetLocationByEvent(positionPoint));
                }
                else
                {
                    _pushPinsInfosMapLayer.Children.RemoveAt(index);
                }

            }
        }

        private bool ContainsUIElement(UIElement element, UIElementCollection collection)
        {
            bool found = false;

            for (int counter = 0; counter < collection.Count; counter++)
            {
                if (collection[counter].Uid.Equals(element.Uid))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private bool ContainsUIElement(UIElement element, UIElementCollection collection, out int index)
        {
            bool found = false;
            index = -1;

            for (int counter = 0; counter < collection.Count; counter++)
            {
                if (collection[counter].Uid.Equals(element.Uid))
                {
                    found = true;
                    index = counter;
                    break;
                }
            }
            return found;
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
        readonly Dictionary<string, TextBlock> _eventBlocks = new Dictionary<string, TextBlock>();
        // A collection of key/value pairs containing the event name  
        // and the number of times the event fired.
        readonly Dictionary<string, int> _eventCount = new Dictionary<string, int>();

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
            if (tagGone(sender, e))
            {
                e.Handled = true;
            }
        }

        private void MapTouchMoveEvent(object sender, TouchEventArgs e)
        {
            Location location = GetLocationByEvent(e);
            ShowEvent("HoldGesture", location,
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
            if (tagMove(sender, e))
            {
                e.Handled = true;
            }
        }

        private void MapTouchDownEvent(object sender, TouchEventArgs e)
        {
            Location location = GetLocationByEvent(e);

            ShowEvent("TapGesture", location,
                e.TouchDevice.GetIsTagRecognized() ?
                    e.TouchDevice.GetTagData().Value : 0x1869f);
            if (tagDown(sender, e))
            {
                e.Handled = true;
            }
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

        #region Location Helper Methods
        private Location GetLocationByEvent(MouseEventArgs e)
        {
            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location location = Map.ViewportPointToLocation(mousePosition);
            return location;
        }

        private Location GetLocationByEvent(Point mousePosition)
        {
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
        #endregion

        private bool tagDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && filterColors.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                Point tp = e.GetTouchPoint(cnv_Interact).Position;
                TagCircle circle = new TagCircle(tp.X, tp.Y, cnv_Draw, cnv_Interact, e.TouchDevice.Id, e.TouchDevice.GetOrientation(cnv_Draw), filterColors[e.TouchDevice.GetTagData().Value]);
                filterCircles.Add(circle);
                return true;
            }
            return false;
        }

        private bool tagMove(object sender, TouchEventArgs e)
        {
            TagCircle filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (e.TouchDevice.GetIsTagRecognized() && filterColors.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                // update position
                Point tp = e.GetTouchPoint(cnv_Draw).Position;
                filterCircle.updateTransform(tp.X, tp.Y, e.TouchDevice.GetOrientation(cnv_Draw));
                return true;
            }
            return false;
        }

        private bool tagGone(object sender, TouchEventArgs e)
        {
            TagCircle filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (e.TouchDevice.GetIsTagRecognized() && filterColors.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                // remove from scene
                filterCircle.clear();

                // remove from list
                filterCircles.Remove(filterCircle);
                return true;
            }
            return false;
        }

        private TagCircle getCircleByTag(int tag)
        {
            foreach (TagCircle circle in filterCircles)
                if (circle.getTag() == tag)
                    return circle;
            return null;
        }
    }
}