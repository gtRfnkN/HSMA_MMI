using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Surface.Presentation.Controls;
using CityGuide.Data.Route;

namespace CityGuide.Extensions
{
    public static class LocationExtensions
    {
        #region Location Math mehtods
        public static Location Subtract(this Location a, Location b)
        {
            var result = new Location
            {
                Latitude = a.Latitude > b.Latitude
                    ? a.Latitude - b.Latitude
                    : b.Latitude - a.Latitude,
                Longitude = a.Longitude > b.Longitude
                    ? a.Longitude - b.Longitude
                    : b.Longitude - a.Longitude
            };

            return result;
        }

        public static Location Addition(this Location a, Location b)
        {
            var result = new Location
            {
                Latitude = b.Latitude + a.Latitude,
                Longitude = b.Longitude + a.Longitude
            };

            return result;
        }

        public static Location Addition(this Location a, double b)
        {
            var result = new Location
            {
                Latitude = a.Latitude + b,
                Longitude = a.Longitude + b
            };

            return result;
        }
        #endregion

        #region Location by Events Methods
        public static Location GetLocationByEvent(this Location a,MouseEventArgs e, Map map, SurfaceWindow surfaceWindow)
        {
            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(surfaceWindow);
            //Convert the mouse coordinates to a locatoin on the map
            Location location = map.ViewportPointToLocation(mousePosition);
            return location;
        }

        public static Location GetLocationByEvent(this Location a, Point mousePosition, Map map)
        {
            //Convert the mouse coordinates to a locatoin on the map
            Location location = map.ViewportPointToLocation(mousePosition);
            return location;
        }

        public static Location GetLocationByEvent(this Location a, TouchEventArgs e, Map map, SurfaceWindow surfaceWindow)
        {
            //Get the mouse click coordinates
            TouchPoint touchPosition = e.GetTouchPoint(surfaceWindow);
            //Convert the mouse coordinates to a locatoin on the map
            Location location = map.ViewportPointToLocation(touchPosition.Position);
            return location;
        }

        public static Route Route(this Location fromLocation, Location toLocation, bool xml, RouteModes routeMode)
        {
            Route result = BingMapRestHelper.Route(fromLocation, toLocation, xml, routeMode);
            return result;
        }

        public static Route Route(this Location fromLocation, Location toLocation, RouteModes routeMode)
        {
            Route result = BingMapRestHelper.Route(fromLocation, toLocation, false, routeMode);
            return result;
        }

        public static Route Route(String fromLocation, String toLocation, RouteModes routeMode)
        {
            Location fromLocationLoc = BingMapRestHelper.Location(fromLocation);
            Location toLocationLoc = BingMapRestHelper.Location(fromLocation);

            Route result = BingMapRestHelper.Route(fromLocationLoc, toLocationLoc, false, routeMode);
            return result;
        }
        #endregion

        #region Location String Extentions

        public static String GetLocationStringWithDotsAndCommaSeperated(this Location location)
        {
           return location.Latitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," + location.Longitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        }
        #endregion
    }
}
