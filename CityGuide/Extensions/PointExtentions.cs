using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CityGuide.Extensions
{
    public static class PointExtensions
    {
        public static bool IsInTolleranz(this Point point, Point checkPoint, Point toleranz)
        {
            bool result = true;

            var diff = point - checkPoint;
            if (diff.X > toleranz.X)
            {
                result = false;
            }

            if (diff.Y > toleranz.Y)
            {
                result = false;
            }

            return result;
        }

        public static bool IsInTolleranz(this Point point, Point checkPoint, double toleranz)
        {
            var toleranzPoint = new Point { X = toleranz, Y = toleranz };
            bool result = IsInTolleranz(point, checkPoint, toleranzPoint);

            return result;
        }

        public static bool IsInTolleranz(this Point point, Point checkPoint, double toleranzX, double toleranzY)
        {
            var toleranzPoint = new Point { X = toleranzX, Y = toleranzY };
            bool result = IsInTolleranz(point, checkPoint, toleranzPoint);

            return result;
        }
    }
}
