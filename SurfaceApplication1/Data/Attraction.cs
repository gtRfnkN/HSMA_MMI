using System;

namespace SurfaceApplication1.Data
{
    public class Attraction
    {
        #region Fields
        public int ID { get; set; }
        public String GeoLocation { get; set; }
        public Filter Filter { get; set; }

        public String Name { get; set; }
        public String TitalPhotoPath { get; set; }

        public String Address { get; set; }
        public String Teaser { get; set; }
        public String Information { get; set; }
        public String OpeningHours { get; set; }


        public int DefaultTimeInMinutes { get; set; }

        public Boolean IsHighlighted { get; set; }
        public Boolean IsFilterd { get; set; }

        public Boolean IsSpezialSunrise { get; set; }
        public Boolean IsSpezialSunset { get; set; }
        public int[] SpezialTimes { get; set; }
        #endregion
    }
}
