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

namespace CityGuide.ViewElements
{
    /// <summary>
    /// Interaktionslogik für TimeTable.xaml
    /// </summary>
    public partial class TimeTable : UserControl
    {
        public MapLayer RoutMapLayer { get; set; }

        /// <summary>
        /// SortedDictionary<Tuple<Uid, row, rowSpan, colum, columSpan>, Grid UIElement>
        /// </summary>
        private readonly SortedDictionary<Tuple<String, int, int, int, int>, UIElement> _gridInformationDictionary;

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

        private readonly SolidColorBrush _normalColorBrush = new SolidColorBrush(Colors.Orange);
        private readonly SolidColorBrush _hoverSolidColorBrush = new SolidColorBrush(Colors.RoyalBlue);
        private readonly Color _routeColorBrush = Colors.Orange;

        public TimeTable()
        {
            InitializeComponent();

            _gridInformationDictionary = new SortedDictionary<Tuple<string, int, int, int, int>, UIElement>();
            int rowcounter = 0;
            for (int counter = 8; counter < 24; counter++)
            {
                String nameUID = (counter < 10) ? "DropTarget0" + counter + "15" : "DropTarget" + counter + "15";

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
                Grid.SetRowSpan(rec, 3);
                TimeTableGrid.Children.Add(rec);
                _gridInformationDictionary.Add(new Tuple<string, int, int, int, int>(rec.Uid, rowcounter, 3, 3, 1), rec);

                rowcounter += 3;
            }

            EventAttractions = new SortedDictionary<int, EventAttraction>();
            EventTransports = new SortedDictionary<int, EventTransport>();
            Routes = new SortedDictionary<Tuple<int, int, RouteModes>, Route>();
        }

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

        private void TimeTableDrop(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (e.Data.GetDataPresent("Attraction") && rectangle != null)
            {
                var attraction = e.Data.GetData("Attraction") as Attraction;
                if (attraction != null)
                {
                    var data = InitMockData.Init();
                    if (data != null)
                    {
                        int index;
                        if (TimeTableGrid.Children.Contains(rectangle, out index))
                        {
                            var rec = TimeTableGrid.Children[index] as Rectangle;
                            if (rec != null)
                            {
                                int row = Grid.GetRow(rec);
                                AddAttractionToTimeTableByDropRegion(rec, attraction, row);

                                DrawAllRoutes();

                                var beforRouteTupel = Routes.Keys.FirstOrDefault(k => k.Item2 == row);
                                if (beforRouteTupel != null)
                                {
                                    AddRouteToTimeTableAfterStartAttraction(beforRouteTupel);

                                }
                                var afterRouteTupel = Routes.Keys.FirstOrDefault(k => k.Item1 == row);
                                if (afterRouteTupel != null)
                                {
                                    AddRouteToTimeTableAfterStartAttraction(afterRouteTupel);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddAttractionToTimeTableByDropRegion(Rectangle rec, Attraction attraction, int row)
        {
            int rowSpan = attraction.DefaultDurationInMinutes / 15;
            rowSpan = (rowSpan == 0) ? 3 : rowSpan;

            bool isFree = IsAreaInTimeTableFree(row, (row + rowSpan));

            if (isFree)
            {
                RemoveOverlapedDropTargets(row, (row + rowSpan));

                var ttea = CreateTimeTableElementAttraction(rec, attraction, row, rowSpan);

                TimeTableGrid.Children.Remove(rec);
                TimeTableGrid.Children.Add(ttea);
                _gridInformationDictionary.Remove(new Tuple<string, int, int, int, int>(rec.Uid, row, 3, 3, 1));
                _gridInformationDictionary.Add(new Tuple<string, int, int, int, int>(ttea.Uid, row, rowSpan, 3, 1), ttea);
            }
            else
            {
                rec.Fill = _normalColorBrush;
            }
        }

        private void AddRouteToTimeTableAfterStartAttraction(Tuple<int, int, RouteModes> routeTuple)
        {
            var route = Routes[routeTuple];
            int rowSpan = route.Duration / 60 / 15;
            rowSpan = (rowSpan == 0) ? 1 : rowSpan;
            int startRow = GetEndRowOfAttraction(routeTuple.Item1);

            bool isFree = IsAreaInTimeTableFree(routeTuple);
            bool isSpace = routeTuple.Item2 - routeTuple.Item1 >= rowSpan;

            if (isFree && isSpace)
            {
                RemoveOverlapedDropTargets(startRow, (startRow + rowSpan));
                var ttet = CreateTimeTableElementTransport(route, routeTuple.Item1, rowSpan, startRow);
                TimeTableGrid.Children.Add(ttet);
                _gridInformationDictionary.Add(new Tuple<string, int, int, int, int>(ttet.Uid, startRow, rowSpan, 3, 1), ttet);
            }
        }

        private bool IsAreaInTimeTableFree(Tuple<int, int, RouteModes> routeTupel)
        {
            bool result = IsAreaInTimeTableFree(routeTupel.Item1, routeTupel.Item2);
            return result;
        }

        private bool IsAreaInTimeTableFree(int startrow, int stoprow)
        {
            bool result = true;

            for (int counter = startrow + 1; counter < stoprow; counter++)
            {
                if (_gridInformationDictionary.Keys.Any(k => k.Item2 == counter))
                {
                    var gridInformationKey = _gridInformationDictionary.Keys.First(k => k.Item2 == counter);
                    var isRectengel = (_gridInformationDictionary[gridInformationKey] is Rectangle);

                    if (!isRectengel)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private int GetEndRowOfAttraction(int row)
        {
            var attraction = EventAttractions[row];
            int result = attraction.Attraction.DefaultDurationInMinutes / 15 + row;
            return result;
        }

        private UIElement CreateTimeTableElementTransport(Route route, int row, int rowSpan, int startrow)
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
            EventTransports.Add(row, eventTransport);

            Grid.SetRow(result, startrow);
            Grid.SetColumn(result, 3);
            Grid.SetRowSpan(result, rowSpan);

            return result;
        }

        private UIElement CreateTimeTableElementAttraction(FrameworkElement rec, Attraction attraction, int row, int rowSpan)
        {
            var startTimeString = rec.Name.Substring("DropTarget".Length);
            int hours = Convert.ToInt32(startTimeString.Substring(0, 2));
            int minutes = Convert.ToInt32(startTimeString.Substring(2));
            var starTime = new DateTime(1, 1, 1, hours, minutes, 0);

            var result = new TimeTableEventAttraction();
            var eventAttraction = new EventAttraction
            {
                Attraction = attraction,
                IsLocked = attraction.IsLoaded,
                Order = row,
                StarTime = starTime,
                StopTime = starTime.AddMinutes(attraction.DefaultDurationInMinutes)
            };

            result.Event = eventAttraction;
            EventAttractions.Add(row, eventAttraction);

            Grid.SetRow(result, row);
            Grid.SetColumn(result, 3);
            Grid.SetRowSpan(result, rowSpan);

            return result;
        }

        private void RemoveOverlapedDropTargets(int startRow, int stopRow)
        {
            for (int counter = startRow; counter < stopRow; counter++)
            {
                if (_gridInformationDictionary.Keys.Any(k => k.Item2 == counter))
                {
                    var gridInformationKey = _gridInformationDictionary.Keys.First(k => k.Item2 == counter);
                    var uiElement = _gridInformationDictionary[gridInformationKey];
                    var isRectengel = (uiElement is Rectangle);

                    if (isRectengel)
                    {
                        TimeTableGrid.Children.Remove(uiElement);
                        _gridInformationDictionary.Remove(gridInformationKey);
                    }
                }
            }
        }

        private void DrawAllRoutes()
        {
            RoutMapLayer.Children.Clear();
            var beforAttraction = new KeyValuePair<int, EventAttraction>();
            foreach (var eventAttraction in EventAttractions)
            {
                if (beforAttraction.Equals(new KeyValuePair<int, EventAttraction>()))
                {
                    beforAttraction = eventAttraction;
                }
                else
                {
                    var routeKey = new Tuple<int, int, RouteModes>(beforAttraction.Key, eventAttraction.Key, RouteModes.Driving);
                    if (!Routes.ContainsKey(routeKey))
                    {
                        String fromAdress = beforAttraction.Value.Attraction.Address;
                        String toAddress = eventAttraction.Value.Attraction.Address;

                        Route route = BingMapRestHelper.Route(fromAdress, toAddress, false, RouteModes.Driving);
                        Routes.Add(routeKey, route);
                    }
                    Route drawRoute = Routes[routeKey];
                    DrawRoute(drawRoute.CreateMapPolylines(_routeColorBrush));

                    beforAttraction = eventAttraction;
                }
            }
        }

        private void DrawRoute(IEnumerable<MapPolyline> routePolylines)
        {
            foreach (var routePolyline in routePolylines)
            {
                RoutMapLayer.Children.Add(routePolyline);
            }
        }
    }
}
