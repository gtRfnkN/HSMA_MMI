using System;
using System.Collections.Generic;

namespace CityGuide.Data
{
    public class TimeTable
    {
        #region Fields
        public DateTime SunsetTime { get; set; }
        public DateTime SunriseTime { get; set; }

        public List<Event> Events { get; set; }  
        #endregion

        #region Methods
        public void OrganizeTimeTable()
        {
            //TODO:
        }
        #endregion
    }
}
