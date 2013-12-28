using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;

namespace CityGuide.Data.Route
{
    public class RouteInstruction
    {
        #region Fields
        public double Kilometer { get; set; }
        public int Duration { get; set; } // in secounds

        public Location Location { get; set; }

        public string CompassDirection { get; set; }
        public string ManeuverType { get; set; }
        public string Instruction { get; set; }
        #endregion

        #region Constructor
        public RouteInstruction(JToken jsonData, bool xml)
        {
            Kilometer = jsonData["TravelDistance"].Value<double>();
            Duration = jsonData["TravelDuration"].Value<int>();

            JToken jLocationDataToken = jsonData["ManeuverPoint"];
            var latitude = jLocationDataToken["Latitude"].Value<double>();
            var longitude = jLocationDataToken["Longitude"].Value<double>();
            Location = new Location(latitude, longitude);

            CompassDirection = jsonData["CompassDirection"].Value<string>();
            ManeuverType = jsonData["Instruction"]["@maneuverType"].Value<string>();
            Instruction = jsonData["Instruction"]["#text"].Value<string>();
        }

        public RouteInstruction(JToken jsonData)
        {
            Kilometer = jsonData["travelDistance"].Value<double>();
            Duration = jsonData["travelDuration"].Value<int>();

            JToken jLocationDataToken = jsonData["maneuverPoint"]["coordinates"];
            var latitude = jLocationDataToken[0].Value<double>();
            var longitude = jLocationDataToken[1].Value<double>();
            Location = new Location(latitude, longitude);

            CompassDirection = jsonData["compassDirection"].Value<string>();
            ManeuverType = jsonData["instruction"]["maneuverType"].Value<string>();
            Instruction = jsonData["instruction"]["text"].Value<string>();
        }
        #endregion
    }
}
