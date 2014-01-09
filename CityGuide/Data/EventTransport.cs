using System;
using CityGuide.Data.Route;

namespace CityGuide.Data
{
    public class EventTransport : Event
    {
        public RouteModes TransportTyp { get; set; }
        public Route.Route Route { get; set; }

        public override int GetRowSpan()
        {
            int result = Convert.ToInt32(DurationTime().TotalMinutes) / 15;
            result = (result == 0) ? 1 : result;
            return result;
        }
    }
}
