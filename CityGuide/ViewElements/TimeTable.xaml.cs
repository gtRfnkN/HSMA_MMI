using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CityGuide.Data;
using CityGuide.Data.Route;
using CityGuide.Extensions;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Input;
using Microsoft.Surface.Presentation.Controls;

namespace CityGuide.ViewElements
{
    /// <summary>
    /// Interaktionslogik für TimeTable.xaml
    /// </summary>
    public partial class TimeTable : UserControl
    {
        #region Fields
        public MapLayer RouteMapLayer { get; set; }
        public ScatterView InfoBoxContainer { get; set; }

        /// <summary>
        /// SortedDictionary<Tuple<Uid, row, rowSpan, colum, columSpan>, Grid UIElement>
        /// </summary>
        private Dictionary<Tuple<String, int, int, int, int>, UIElement> _gridInformationDictionary;

        /// <summary>
        /// SortedDictionary<GridRow, EventAttraction> 
        /// </summary>
        public SortedDictionary<int, EventAttraction> EventAttractions { get; private set; }
        /// <summary>
        /// SortedDictionary<GridRow, EventTransport> 
        /// </summary>
        public SortedDictionary<int, EventTransport> EventTransports { get; private set; }
        /// <summary>
        /// SortedDictionary<Tuple<FromGridRow,ToGridRow,RouteModes which is used to request the Route>, Route which containts the information of the route from Attraction to Attraction>
        /// </summary>
        public SortedDictionary<Tuple<int, int, RouteModes>, Route> Routes { get; private set; }

        private readonly SolidColorBrush _normalColorBrush = new SolidColorBrush(Colors.Transparent);
        private readonly SolidColorBrush _hoverSolidColorBrush = new SolidColorBrush(Color.FromArgb(100, 254, 204, 92));
        private readonly Color _routeColorBrush = Colors.Orange;
        #endregion

        public TimeTable()
        {
            InitializeComponent();

            AddAllRectangelDropTargets();

            EventAttractions = new SortedDictionary<int, EventAttraction>();
            EventTransports = new SortedDictionary<int, EventTransport>();
            Routes = new SortedDictionary<Tuple<int, int, RouteModes>, Route>();
        }

        #region Rectangle Drop Target Methods
        private void AddAllRectangelDropTargets()
        {
            _gridInformationDictionary = new Dictionary<Tuple<string, int, int, int, int>, UIElement>();
            int rowcounter = 0;
            for (int counter = 8; counter < 24; counter++)
            {
                CreateDropTargetRectangle(counter, rowcounter, "15");
                CreateDropTargetRectangle(counter, rowcounter + 1, "30");
                CreateDropTargetRectangle(counter, rowcounter + 2, "45");
                rowcounter += 3;
            }
        }

        private void CreateDropTargetRectangle(int counter, int rowcounter, String minutes)
        {
            String nameUID = (counter < 10) ? "DropTarget0" + counter + minutes : "DropTarget" + counter + minutes;

            var rec = new Rectangle
            {
                Name = nameUID,
                Uid = nameUID,
                AllowDrop = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = _normalColorBrush
            };

            rec.Drop += TimeTableDrop;
            rec.DragOver += TimeTableDragOver;
            rec.DragEnter += TimeTableDragEnter;
            rec.DragLeave += TimeTableDragLeave;

            Grid.SetRow(rec, rowcounter);
            Grid.SetColumn(rec, 3);
            Grid.SetRowSpan(rec, 1);
            TimeTableGrid.Children.Add(rec);
            _gridInformationDictionary.Add(new Tuple<string, int, int, int, int>(rec.Uid, rowcounter, 1, 3, 1), rec);
        }
        #endregion

        #region Drag Effect Methods
        private void TimeTableDragLeave(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = _normalColorBrush;
            }
        }

        private void TimeTableDragEnter(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = _hoverSolidColorBrush;
            }
        }

        private void TimeTableDragOver(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = _hoverSolidColorBrush;
            }
        }
        #endregion

        private void TimeTableDrop(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (e.Data.GetDataPresent("Attraction") && rectangle != null)
            {
                var attraction = e.Data.GetData("Attraction") as Attraction;
                if (attraction != null)
                {
                    var data = MockData.Init();
                    if (data != null)
                    {
                        int index;
                        if (TimeTableGrid.Children.Contains(rectangle, out index))
                        {
                            var rec = TimeTableGrid.Children[index] as Rectangle;
                            if (rec != null)
                            {
                                int row = Grid.GetRow(rec);
                                int rowSpan = 0, rowSpanRouteBefor = 0, rowSpanRouteAfter = 0;

                                TimeTableEventAttraction timeTableAttraction = CreateAttractionToTimeTableByDropRegion(rec, attraction, row, out rowSpan);
                                int endRowOfAttraction = row + rowSpan;
                                bool isFreeAttraction = IsAreaInTimeTableFree(row, endRowOfAttraction);

                                if (isFreeAttraction)
                                {
                                    #region Routes Created and test Variables init
                                    ResetRoutesAndCheckIfNewRoutesAreNeeded();


                                    var beforRouteTupel = Routes.Keys.FirstOrDefault(k => k.Item2 == row);
                                    TimeTableEventTransportation timeTableRouteBefor = null;
                                    bool isFreeRouteBefor = false;
                                    bool isSpaceRouteBefor = false;
                                    if (beforRouteTupel != null)
                                    {
                                        timeTableRouteBefor = CreateRouteToTimeTableAttraction(beforRouteTupel, out rowSpanRouteBefor);
                                        isFreeRouteBefor = IsAreaInTimeTableFree(beforRouteTupel, rowSpanRouteBefor);
                                        isSpaceRouteBefor = beforRouteTupel.Item2 - beforRouteTupel.Item1 >= rowSpanRouteBefor;
                                    }

                                    var afterRouteTupel = Routes.Keys.FirstOrDefault(k => k.Item1 == row);
                                    TimeTableEventTransportation timeTableRouteAfter = null;
                                    bool isFreeRouteAfter = false;
                                    bool isSpaceRouteAfter = false;
                                    if (afterRouteTupel != null)
                                    {
                                        timeTableRouteAfter = CreateRouteToTimeTableAttraction(afterRouteTupel, out rowSpanRouteAfter);
                                        isFreeRouteAfter = IsAreaInTimeTableFree(afterRouteTupel, rowSpanRouteAfter);
                                        isSpaceRouteAfter = afterRouteTupel.Item2 - afterRouteTupel.Item1 >= rowSpanRouteAfter;
                                    }
                                    #endregion

                                    #region Add all Items to TimeTable

                                    bool beforRouteCheck = (isFreeRouteBefor && isSpaceRouteBefor);
                                    bool afterRouteCheck = (isFreeRouteAfter && isSpaceRouteAfter);

                                    if ((EventAttractions.Count == 1))
                                    {
                                        AddAttrationToTimeTable(rec, timeTableAttraction, row, rowSpan);
                                    }
                                    else if ((timeTableRouteBefor != null) && (timeTableRouteAfter != null) && (beforRouteCheck && afterRouteCheck))
                                    {
                                        AddAttractionAndRoutsToTimeTable(rec, row, rowSpan, rowSpanRouteBefor, rowSpanRouteAfter, timeTableAttraction, timeTableRouteBefor, timeTableRouteAfter, beforRouteTupel, afterRouteTupel, beforRouteCheck, afterRouteCheck);
                                    }
                                    else if ((timeTableRouteAfter == null) && (timeTableRouteBefor != null && beforRouteCheck))
                                    {
                                        AddAttrationToTimeTable(rec, timeTableAttraction, row, rowSpan);
                                        AddRouteToTimeTable(beforRouteTupel.Item1, rowSpanRouteBefor, timeTableRouteBefor);
                                    }
                                    else if ((timeTableRouteBefor == null) && (timeTableRouteAfter != null && afterRouteCheck))
                                    {
                                        AddAttrationToTimeTable(rec, timeTableAttraction, row, rowSpan);
                                        AddRouteToTimeTable(afterRouteTupel.Item1, rowSpanRouteAfter, timeTableRouteAfter);
                                    }
                                    else
                                    {
                                        ResetChanges(row, rec);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    rec.Fill = _normalColorBrush;
                                }
                            }
                        }
                    }
                }
                RouteMapLayer.Children.Clear();

                // add Routes to Routes Map Layer
                foreach (var route in EventTransports)
                {
                    DrawRoute(route.Value.Route.CreateMapPolylines(_routeColorBrush));
                }
            }
        }

        private void AddAttractionAndRoutsToTimeTable(Rectangle rec, int row, int rowSpan, int rowSpanRouteBefor, int rowSpanRouteAfter, TimeTableEventAttraction timeTableAttraction, TimeTableEventTransportation timeTableRouteBefor, TimeTableEventTransportation timeTableRouteAfter, Tuple<int, int, RouteModes> beforRouteTupel, Tuple<int, int, RouteModes> afterRouteTupel, bool beforRouteCheck, bool afterRouteChecck)
        {
            AddAttrationToTimeTable(rec, timeTableAttraction, row, rowSpan);

            if (timeTableRouteBefor != null && beforRouteCheck)
            {
                AddRouteToTimeTable(beforRouteTupel.Item1, rowSpanRouteBefor, timeTableRouteBefor);
            }

            if (timeTableRouteAfter != null && afterRouteChecck)
            {
                AddRouteToTimeTable(afterRouteTupel.Item1, rowSpanRouteAfter, timeTableRouteAfter);
            }
        }

        private void ResetChanges(int row, Rectangle rec)
        {
            EventAttractions.Remove(row);
            rec.Fill = _normalColorBrush;
        }

        #region TimeTable Attraction Element Methods
        private TimeTableEventAttraction CreateAttractionToTimeTableByDropRegion(Rectangle rec, Attraction attraction, int row, out int rowSpan)
        {
            EventHandler<TouchEventArgs> touchLableEvent = (sender, e) =>
            {
                Point positionPoint = e.GetTouchPoint(this).Position;
                var infoBox = new InfoBox(attraction);

                infoBox.Orientation = 0.0;
                positionPoint.X = positionPoint.X + infoBox.Width / 2;
                positionPoint.Y = positionPoint.Y + infoBox.Height / 2;

                infoBox.Center = positionPoint;

                InfoBoxContainer.Items.Add(infoBox);
            };

            var timeTableAttractionElement = CreateTimeTableElementAttraction(rec, attraction, row, out rowSpan, touchLableEvent);
            EventAttractions.Add(row, timeTableAttractionElement.Event);
            return timeTableAttractionElement;
        }

        private void AddAttrationToTimeTable(Rectangle rec, TimeTableEventAttraction timeTableAttractionElement, int row, int rowSpan)
        {
            RemoveOverlapedDropTargets(row, (row + rowSpan));
            TimeTableGrid.Children.Remove(rec);
            TimeTableGrid.Children.Add(timeTableAttractionElement);
            _gridInformationDictionary.Remove(new Tuple<string, int, int, int, int>(rec.Uid, row, 3, 3, 1));
            _gridInformationDictionary.Add(new Tuple<string, int, int, int, int>(timeTableAttractionElement.Uid, row, rowSpan, 3, 1), timeTableAttractionElement);
        }

        private int GetEndRowOfAttraction(int row)
        {
            var attraction = EventAttractions[row];
            int result = attraction.GetRowSpan();
            return result;
        }
        #endregion

        #region TimeTable Route Element Methods
        private TimeTableEventTransportation CreateRouteToTimeTableAttraction(Tuple<int, int, RouteModes> routeTuple, out int rowSpan)
        {
            var route = Routes[routeTuple];
            rowSpan = route.Duration / 60 / 15;
            rowSpan = (rowSpan == 0) ? 1 : rowSpan;

            var routeTimeTableElement = CreateTimeTableElementTransport(route, routeTuple.Item1, rowSpan);
            return routeTimeTableElement;
        }

        private void AddRouteToTimeTable(int startRow, int rowSpan, TimeTableEventTransportation routeTimeTableElement)
        {
            if (EventTransports.Keys.Any(k => k == startRow))
            {
                var toDelete = _gridInformationDictionary.FirstOrDefault(value => value.Key.Item2 == startRow && value.Key.Item1.Contains("TimeTableEventTransporation"));
                TimeTableGrid.Children.Remove(toDelete.Value);
                var delete = _gridInformationDictionary.Remove(toDelete.Key);
                EventTransports.Remove(startRow);

                EventTransports.Add(startRow, routeTimeTableElement.Event);
            }
            else
            {
                EventTransports.Add(startRow, routeTimeTableElement.Event);
            }

            RemoveOverlapedDropTargets(startRow, (startRow + rowSpan));
            TimeTableGrid.Children.Add(routeTimeTableElement);
            _gridInformationDictionary.Add(new Tuple<string, int, int, int, int>(routeTimeTableElement.Uid, startRow, rowSpan, 3, 1), routeTimeTableElement);
        }
        #endregion

        #region TimeTable Drop Area Check Methods
        private bool IsAreaInTimeTableFree(Tuple<int, int, RouteModes> routeTupel, int rowSpan)
        {
            int startRow = GetEndRowOfAttraction(routeTupel.Item1);
            bool result = IsAreaInTimeTableFree(startRow, (startRow + rowSpan));
            return result;
        }

        private bool IsAreaInTimeTableFree(int startrow, int stoprow)
        {
            bool result = true;

            for (int counter = startrow; counter < stoprow; counter++)
            {
                if (_gridInformationDictionary.Keys.Any(k => k.Item2 == counter))
                {
                    var gridInformationKey = _gridInformationDictionary.Keys.First(k => k.Item2 == counter);
                    var isDropTarget = (_gridInformationDictionary[gridInformationKey].Uid.Contains("DropTarget"));

                    if (!isDropTarget)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        private void RemoveOverlapedDropTargets(int startRow, int stopRow)
        {
            for (int counter = startRow; counter < stopRow; counter++)
            {
                if (_gridInformationDictionary.Keys.Any(k => k.Item2 == counter))
                {
                    var gridInformationKey = _gridInformationDictionary.Keys.First(k => k.Item2 == counter);
                    var uiElement = _gridInformationDictionary[gridInformationKey];
                    var isDropTarget = (uiElement.Uid.Contains("DropTarget"));

                    if (isDropTarget)
                    {
                        TimeTableGrid.Children.Remove(uiElement);
                        _gridInformationDictionary.Remove(gridInformationKey);
                    }
                }
            }
        }

        #region Create Time Table Elements Methods
        private TimeTableEventTransportation CreateTimeTableElementTransport(Route route, int row, int rowSpan)
        {
            var attraction = EventAttractions[row];
            var starTime = attraction.StopTime;

            var result = new TimeTableEventTransportation();
            var eventTransport = new EventTransport
            {
                Route = route,
                TransportTyp = route.RouteMode,
                Order = row,
                StarTime = starTime,
                StopTime = starTime.AddSeconds(route.Duration)
            };

            result.Event = eventTransport;

            Grid.SetRow(result, attraction.GetRowSpan() + row);
            Grid.SetColumn(result, 3);
            Grid.SetRowSpan(result, rowSpan);

            return result;
        }

        private TimeTableEventAttraction CreateTimeTableElementAttraction(FrameworkElement rec, Attraction attraction, int row, out int rowSpan, EventHandler<TouchEventArgs> touchAttractionLabelEvent)
        {
            var startTimeString = rec.Name.Substring("DropTarget".Length);
            int hours = Convert.ToInt32(startTimeString.Substring(0, 2));
            int minutes = Convert.ToInt32(startTimeString.Substring(2));
            var starTime = new DateTime(1, 1, 1, hours, minutes, 0);

            var result = new TimeTableEventAttraction(touchAttractionLabelEvent);
            var eventAttraction = new EventAttraction
            {
                Attraction = attraction,
                IsLocked = attraction.IsLoaded,
                Order = row,
                StarTime = starTime,
                StopTime = starTime.AddMinutes(attraction.DefaultDurationInMinutes)
            };

            result.Event = eventAttraction;
            result.TimeTable = this;
            rowSpan = result.GetRowSpan();

            Grid.SetRow(result, row);
            Grid.SetColumn(result, 3);
            Grid.SetRowSpan(result, rowSpan);

            return result;
        }
        #endregion

        #region Route Handling
        private void ResetRoutesAndCheckIfNewRoutesAreNeeded()
        {
            var beforAttraction = new KeyValuePair<int, EventAttraction>();

            if (EventAttractions.Count > 1)
            {
                foreach (var eventAttraction in EventAttractions)
                {
                    if (beforAttraction.Equals(new KeyValuePair<int, EventAttraction>()))
                    {
                        beforAttraction = eventAttraction;
                    }
                    else
                    {
                        Tuple<int, int, RouteModes> routeKey = null;
                        if (beforAttraction.Key < eventAttraction.Key)
                        {
                            routeKey = new Tuple<int, int, RouteModes>(beforAttraction.Key, eventAttraction.Key, RouteModes.Walking);
                        }
                        else
                        {
                            routeKey = new Tuple<int, int, RouteModes>(eventAttraction.Key, beforAttraction.Key, RouteModes.Walking);
                        }

                        if (!Routes.ContainsKey(routeKey))
                        {
                            String fromAdress = beforAttraction.Value.Attraction.Address;
                            String toAddress = eventAttraction.Value.Attraction.Address;

                            Route route = BingMapRestHelper.Route(fromAdress, toAddress, false, RouteModes.Walking);
                            Routes.Add(routeKey, route);
                        }

                        beforAttraction = eventAttraction;
                    }
                }
            }
        }

        private void DrawRoute(IEnumerable<MapPolyline> routePolylines)
        {
            foreach (var routePolyline in routePolylines)
            {
                RouteMapLayer.Children.Add(routePolyline);
            }
        }
        #endregion

        public void Reset()
        {
            //Remove all TimeTable Items
            TimeTableGrid.Children.Clear();

            // Create new Drop Targets
            AddAllRectangelDropTargets();

            // Cleare all managment Collections
            EventAttractions = new SortedDictionary<int, EventAttraction>();
            EventTransports = new SortedDictionary<int, EventTransport>();
            Routes = new SortedDictionary<Tuple<int, int, RouteModes>, Route>();

            //Clear Routes
            RouteMapLayer.Children.Clear();
        }

        public void RedrawTimeTableTimeChange(TimeTableEventAttraction tteAttraction)
        {
            var changedItem = _gridInformationDictionary.FirstOrDefault(item => item.Key.Item1.Equals(tteAttraction.Uid));
            if (!changedItem.Equals(new KeyValuePair<Tuple<String, int, int, int, int>, UIElement>()))
            {
                //Changed Attraction
                _gridInformationDictionary.Remove(changedItem.Key);

                EventAttractions.Remove(changedItem.Key.Item2);
                EventAttractions.Add(changedItem.Key.Item2, tteAttraction.Event);

                int rowSpan = tteAttraction.GetRowSpan();
                Grid.SetRowSpan(tteAttraction, rowSpan);
                var oldKey = changedItem.Key;
                var newKey = new Tuple<String, int, int, int, int>(oldKey.Item1, oldKey.Item2, rowSpan, oldKey.Item4, oldKey.Item5);
                _gridInformationDictionary.Add(newKey, tteAttraction);

                //Changed Route
                var routeItem = _gridInformationDictionary.FirstOrDefault(item => item.Key.Item1.Contains("TimeTableEventTransporation") && item.Key.Item2 == oldKey.Item2);
                if (!routeItem.Equals(new KeyValuePair<Tuple<String, int, int, int, int>, UIElement>()))
                {
                    _gridInformationDictionary.Remove(routeItem.Key);

                    var oldTransportEvent = EventTransports[routeItem.Key.Item2];
                    EventTransports.Remove(routeItem.Key.Item2);
                    var tteTransport = CreateTimeTableElementTransport(oldTransportEvent.Route, newKey.Item2, oldTransportEvent.GetRowSpan());
                    EventTransports.Add(changedItem.Key.Item2, tteTransport.Event);

                    var oldRouteKey = routeItem.Key;
                    var newRouteKey = new Tuple<String, int, int, int, int>(oldRouteKey.Item1, oldRouteKey.Item2, oldTransportEvent.GetRowSpan(), oldRouteKey.Item4, oldRouteKey.Item5);
                    _gridInformationDictionary.Add(newRouteKey, tteTransport);
                }

                //Add Or Remove DropTargates
                if (oldKey.Item3 < rowSpan)
                {
                    RemoveOverlapedDropTargets(newKey.Item2, newKey.Item2 + rowSpan);
                }
                else
                {
                    AddNewDropTargets(newKey.Item2 + rowSpan, oldKey.Item2 + oldKey.Item3, tteAttraction.Event.StopTime);
                }

                //Redraw TimeTable
                TimeTableGrid.Children.Clear();
                foreach (var item in _gridInformationDictionary)
                {
                    TimeTableGrid.Children.Add(item.Value);
                }
            }
        }

        private void AddNewDropTargets(int startRow, int stopRow, DateTime startTime)
        {
            int hours = startTime.Hour;
            int minutes = startTime.Minute / 15;
            for (int counter = startRow; counter <= stopRow; counter++)
            {
                if (minutes == 3)
                {
                    hours++;
                    minutes = 1;
                }
                String displayminutes = minutes * 15 + "";
                if (!_gridInformationDictionary.Any(item => item.Key.Item2 == counter))
                {
                    CreateDropTargetRectangle(hours, counter, displayminutes);
                }

                minutes++;
            }
        }
    }
}
