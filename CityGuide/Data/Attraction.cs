using System;
using Microsoft.Maps.MapControl.WPF;

namespace CityGuide.Data
{
    public class Attraction : Pushpin {
        #region Fields
        public int ID { get; set; }
        public Filter Filter { get; set; }

        public String Titel { get; set; }
        public String TitelPhotoPath { get; set; }

        public String Address { get; set; }
        public String Teaser { get; set; }
        public String Information { get; set; }
        public String OpeningHours { get; set; }
        
        public int DefaultDurationInMinutes { get; set; }

        public Boolean IsHighlighted { get; set; }
        public Boolean IsFilterd { get; set; }

        public Boolean IsSpezialSunrise { get; set; }
        public Boolean IsSpezialSunset { get; set; }
        public int[] SpezialTimes { get; set; }

        public int Interest { get; set; }
        #endregion
    }
}
