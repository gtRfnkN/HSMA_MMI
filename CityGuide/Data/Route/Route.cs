using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;

namespace CityGuide.Data.Route
{
    public enum RouteModes { Driving, Transit, Walking, }

    public class Route
    {
        #region Fields
        public Location StartLocation { get; set; }
        public Location EndeLocation { get; set; }

        public double Kilometer { get; set; }
        public int Duration { get; set; }// in secounds

        public RouteModes RouteMode { get; set; }

        public List<Location> PointLocations { get; set; }
        public List<RouteInstruction> RouteInstructions { get; set; }

        private List<MapPolyline> _routeLines;
        #endregion

        #region Construtors
        public Route(JObject jsonData, Location startLocation, Location endLocation, RouteModes routeMode, bool xml)
        {
            StartLocation = startLocation;
            EndeLocation = endLocation;

            Kilometer = jsonData["Response"]["ResourceSets"]["ResourceSet"]["Resources"]["Route"]["TravelDistance"].Value<double>();
            Duration = jsonData["Response"]["ResourceSets"]["ResourceSet"]["Resources"]["Route"]["TravelDuration"].Value<Int32>();
            RouteMode = routeMode;
            PointLocations = new List<Location>();
            RouteInstructions = new List<RouteInstruction>();

            JToken pointCoordinate = jsonData["Response"]["ResourceSets"]["ResourceSet"]["Resources"]["Route"]["RoutePath"]["Line"]["Point"][0];
            while (pointCoordinate != null)
            {
                var latitude = pointCoordinate["Latitude"].Value<double>();
                var longitude = pointCoordinate["Longitude"].Value<double>();

                var location = new Location(latitude, longitude);
                PointLocations.Add(location);

                pointCoordinate = pointCoordinate.Next;
            }

            JToken routeInstructionToken = jsonData["Response"]["ResourceSets"]["ResourceSet"]["Resources"]["Route"]["RouteLeg"]["ItineraryItem"][0];
            while (routeInstructionToken != null)
            {
                var introduction = new RouteInstruction(routeInstructionToken, true);
                RouteInstructions.Add(introduction);

                routeInstructionToken = routeInstructionToken.Next;
            }
        }

        public Route(JObject jsonData, Location startLocation, Location endLocation, RouteModes routeMode)
        {
            StartLocation = startLocation;
            EndeLocation = endLocation;

            Kilometer = jsonData["resourceSets"][0]["resources"][0]["travelDistance"].Value<double>();
            Duration = jsonData["resourceSets"][0]["resources"][0]["travelDuration"].Value<Int32>();
            RouteMode = routeMode;

            PointLocations = new List<Location>();
            RouteInstructions = new List<RouteInstruction>();

            JToken pointCoordinate = jsonData["resourceSets"][0]["resources"][0]["routePath"]["line"]["coordinates"][0];
            while (pointCoordinate != null)
            {
                var latitude = pointCoordinate[0].Value<double>();
                var longitude = pointCoordinate[1].Value<double>();

                var location = new Location(latitude, longitude);
                PointLocations.Add(location);

                pointCoordinate = pointCoordinate.Next;
            }

            JToken routeInstructionToken = jsonData["resourceSets"][0]["resources"][0]["routeLegs"][0]["itineraryItems"][0];
            while (routeInstructionToken != null)
            {
                var introduction = new RouteInstruction(routeInstructionToken);
                RouteInstructions.Add(introduction);

                routeInstructionToken = routeInstructionToken.Next;
            }
        }
        #endregion

        #region Methods
        private static MapPolyline CreateMapPolylines(Location start, Location finish, Color color)
        {
            var line = new MapPolyline
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 5.0,
                Locations = new LocationCollection { start, finish }
            };
            return line;
        }

        public List<MapPolyline> CreateMapPolylines(Color color)
        {
            if (_routeLines == null || _routeLines.Count == 0)
            {
                _routeLines = new List<MapPolyline>();

                bool first = true;
                var locationBefor = new Location();
                foreach (Location pointLocation in PointLocations)
                {
                    if (first)
                    {
                        locationBefor = pointLocation;
                        first = false;
                    }
                    else
                    {
                        _routeLines.Add(CreateMapPolylines(locationBefor, pointLocation, color));
                        locationBefor = pointLocation;
                    }
                }
            }

            return _routeLines;
        }
        #endregion
    }
}
