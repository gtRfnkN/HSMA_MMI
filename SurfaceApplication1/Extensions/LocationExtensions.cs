using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Maps.MapControl.WPF;

namespace SurfaceApplication1.Extensions
{
    public static class LocationExtensions
    {
        public static Location Subtract(this Location a, Location b)
        {
            var result = new Location();
            result.Latitude = a.Latitude > b.Latitude ?
                a.Latitude - b.Latitude :
                b.Latitude - a.Latitude;

            result.Longitude = a.Longitude > b.Longitude ?
               a.Longitude - b.Longitude :
               b.Longitude - a.Longitude;
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
                Longitude = a.Longitude +b
            };

            return result;
        }
    }
}
