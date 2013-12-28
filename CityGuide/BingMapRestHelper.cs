using System;
using System.IO;
using System.Net;
using System.Xml;
using CityGuide.Data.Route;
using CityGuide.Extensions;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CityGuide
{
    public class BingMapRestHelper
    {
        private const String BingMapKey = "AtQ9U9V-4N1Z8Btk44H3T6gU2_7Q12BxUW3SZmyaE-BHbhRJwXfHSAkc_HKXZU4Q";

        // Submit a REST Services or Spatial Data Services request and return the response
        private static XmlDocument GetXmlResponse(string requestUrl)
        {
            var xmlDoc = new XmlDocument();
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            var request = WebRequest.Create(requestUrl) as HttpWebRequest;
            if (request != null)
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));

                    if (response != null) { xmlDoc.Load(response.GetResponseStream()); }
                }
            return xmlDoc;
        }

        private static JObject GetJaonResponse(string requestUrl)
        {
            var jsonObject = new JObject();
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            var request = WebRequest.Create(requestUrl) as HttpWebRequest;
            if (request != null)
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));

                    if (response != null && response.GetResponseStream() != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        jsonObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                    }
                }
            return jsonObject;
        }

        // Geocode an address and return a latitude and longitude
        public static Location Location(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + BingMapKey;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            //Create namespace manager
            var nsmgr = new XmlNamespaceManager(geocodeResponse.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            XmlNodeList locationElements = geocodeResponse.SelectNodes("//rest:Location", nsmgr);
            //Get the geocode location points that are used for display (UsageType=Display)
            XmlNodeList displayGeocodePoints =
                    locationElements[0].SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);
            string latitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Latitude", nsmgr).InnerText.Replace('.', ',');
            string longitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Longitude", nsmgr).InnerText.Replace('.', ',');

            Double latitudeDouble = Convert.ToDouble(latitude);
            Double longitudeDouble = Convert.ToDouble(longitude);
            var location = new Location(latitudeDouble, longitudeDouble);

            return location;
        }

        public static Route Route(String from, String to, bool xml, RouteModes routeMode)
        {
            Location fromLocation = Location(from);
            Location toLocation = Location(to);

            return Route(fromLocation, toLocation, xml, routeMode);
        }

        public static Route Route(Location fromLocation, Location toLocation, bool xml, RouteModes routeMode)
        {
            String from = fromLocation.GetLocationStringWithDotsAndCommaSeperated();
            String to = toLocation.GetLocationStringWithDotsAndCommaSeperated();

            //Create the Request URL for the routing service
            String restRequest;
            JObject jObject;
            Route route;
            if (xml)
            {
                restRequest = string.Format(
                    "http://dev.virtualearth.net/REST/V1/Routes/{3}?waypoint.0={0}&waypoint.1={1}&rpo=Points&o=xml&key={2}"
                    , from, to, BingMapKey, routeMode);

                //Make the request and get the response
                XmlDocument routeXmlDocument = GetXmlResponse(restRequest);

                string json = JsonConvert.SerializeXmlNode(routeXmlDocument);
                jObject = JObject.Parse(json);
                route = new Route(jObject, fromLocation, toLocation, routeMode, true);
            }
            else
            {
                restRequest = string.Format(
                "http://dev.virtualearth.net/REST/V1/Routes/{3}?waypoint.0={0}&waypoint.1={1}&rpo=Points&key={2}"
                , from, to, BingMapKey, routeMode);

                jObject = GetJaonResponse(restRequest);
                route = new Route(jObject, fromLocation, toLocation, routeMode);
            }

            return route;
        }
    }
}
