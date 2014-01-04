using System;
using CityGuide.Data.Route;

namespace CityGuide.Data
{
    public class EventTransport : Event
    {
        public RouteModes TransportTyp { get; set; }
        public Route.Route Route { get; set; }
    }
}
