﻿using System;

namespace SurfaceApplication1.Data
{
    public abstract class Event
    {
        #region Fields
        public DateTime StarTime { get; set; }
        public DateTime StopTime { get; set; }
        public Boolean IsLocked { get; set; }
        public int Order { get; set; }
        #endregion

        #region Methods
        public TimeSpan DurationTime()
        {
            return StopTime - StarTime;
        }
        #endregion
    }
}