using System;
using System.Timers;

namespace CityGuide.Data
{
    public class CoolDownTimer
    {
        public String Name { get; set; }
        private int _hoursInit;
        public int Hours
        {
            get { return _hoursInit; }
            set { _hoursInit = value; }
        }

        private int _minutesInit;
        public int Minutes
        {
            get { return _minutesInit; }
            set { _minutesInit = value; }
        }

        private int _secondsInit;
        public int Secounds
        {
            get { return _secondsInit; }
            set { _secondsInit = value; }
        }

        public delegate void OnTimerFinishedDelegate();
        public event OnTimerFinishedDelegate OnTimerFinished;

        public delegate void OnTimerChanged(TimerEventArgs args);
        public event OnTimerChanged OnTimerTick;

        private readonly Timer _internelTimer = new Timer();
        private DateTime _start = DateTime.UtcNow;
        private DateTime _endTime = DateTime.UtcNow;
        private TimeSpan remainingTime;

        public CoolDownTimer(int hours = 0, int minutes = 0, int secounds = 0)
        {
            _internelTimer.Interval = 1000;
            _internelTimer.Elapsed += T_Tick;

            _minutesInit = minutes;
            _secondsInit = secounds;
            _hoursInit = hours;

            _endTime = _start.AddHours(_hoursInit).AddMinutes(_minutesInit).AddSeconds(_secondsInit);
        }

        private void T_Tick(object sender, EventArgs e)
        {
            remainingTime = _endTime - DateTime.UtcNow;
            Console.WriteLine(Name + ':' + remainingTime);
            if (remainingTime < TimeSpan.Zero)
            {
                _internelTimer.Stop();
                if (OnTimerFinished != null)
                {
                    OnTimerFinished();
                }
            }
            else
            {
                if (OnTimerTick != null)
                {
                    OnTimerTick(new TimerEventArgs(remainingTime));
                }
            }
        }

        public void PlayPause()
        {
            if (_internelTimer.Enabled)
            {
                _internelTimer.Stop();
            }
            else
            {
                _internelTimer.Start();
            }
        }

        /// <summary>
        /// First reset the timer to the init values and then start it. This method doesn't check if the timer is already running.
        /// </summary>
        public void StartWithReset()
        {
            Reset();
            _internelTimer.Start();
        }

        public void Stop()
        {
            if (_internelTimer.Enabled)
            {
                _internelTimer.Stop();
            }
        }

        public void Reset()
        {
            _start = DateTime.UtcNow;
            _endTime = _start.AddHours(_hoursInit).AddMinutes(_minutesInit).AddSeconds(_secondsInit);
        }

        public bool Enabled
        {
            get
            {
                return _internelTimer.Enabled;
            }
        }
    }

    public class TimerEventArgs : EventArgs
    {

        public string TimeLeft;
        public TimeSpan RemainingTime;

        public TimerEventArgs(TimeSpan remainingTime)
        {
            RemainingTime = remainingTime;
            TimeLeft = remainingTime.Hours + ":" + remainingTime.Minutes + ":" + remainingTime.Seconds;
        }

    }
}