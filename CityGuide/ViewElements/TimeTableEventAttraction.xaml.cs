using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CityGuide.Data;

namespace CityGuide.ViewElements
{
    /// <summary>
    /// Interaktionslogik für TimeTableEvent.xaml
    /// </summary>
    public partial class TimeTableEventAttraction : UserControl
    {
        private EventAttraction _event;
        public EventAttraction Event
        {
            get { return _event; }
            set
            {
                if (value != null)
                {
                    _event = value;

                    var eventAttraction = _event as EventAttraction;
                    AttrationNameLabel.Content = eventAttraction.Attraction.Titel;
                    AttrationNameLabel.Background = new SolidColorBrush(eventAttraction.Attraction.Filter.Color);

                    AttrationNameLabel.FontStretch = FontStretches.Condensed;

                    String nameUID = "TimeTableEventAttraction" + eventAttraction.Attraction.Name + eventAttraction.Order;
                    Uid = nameUID;
                    Name = nameUID;
                }
            }
        }

        public TimeTable TimeTable { get; set; }
        private bool MunitsAdded { get; set; }

        public TimeTableEventAttraction(EventHandler<TouchEventArgs> TouchEventLabel)
        {
            InitializeComponent();

            //Lock button Events
            LockButton.TouchDown += TouchLockButton;
            LockButton.Click += MouseClickLockButton;

            AttrationNameLabel.Foreground = new SolidColorBrush(Colors.Black);
            AttrationNameLabel.TouchDown += TouchEventLabel;

            ResizeCanvas.TouchMove += TouchMoveResizeButton;

            MunitsAdded = false;
        }

        #region Lock Button Events & Methods
        private void TouchLockButton(Object sender, TouchEventArgs e)
        {
            var button = sender as Button;
            LockEvent(button);
        }

        private void MouseClickLockButton(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            LockEvent(button);
        }

        private void LockEvent(Button button)
        {
            bool state = !Event.IsLocked;
            if (state)
            {
                if (button != null) button.Content = "Unlock";
            }
            else
            {
                if (button != null) button.Content = "Lock";
            }
            Event.IsLocked = state;
        }
        #endregion

        private void TouchMoveResizeButton(Object sender, TouchEventArgs e)
        {
            var parent = Parent as TimeTable;

            if (!MunitsAdded)
            {
               Event.StopTime = Event.StopTime.AddMinutes(30);
                MunitsAdded = true;
                TimeTable.RedrawTimeTableTimeChange(this);
            }
            else
            {
                Event.StopTime = Event.StopTime.AddMinutes(-30);
                MunitsAdded = false;
                TimeTable.RedrawTimeTableTimeChange(this);
            }
        }

        public int GetRowSpan()
        {
            return Event.GetRowSpan();
        }
    }
}
