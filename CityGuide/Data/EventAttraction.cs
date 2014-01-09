using System;
namespace CityGuide.Data
{
    public class EventAttraction : Event
    {
        public Attraction Attraction { get; set; }

        public override int GetRowSpan()
        {
            int result = Convert.ToInt32(DurationTime().TotalMinutes) / 15;
            result = (result == 0) ? 3 : result;
            return result;
        }
    }
}
