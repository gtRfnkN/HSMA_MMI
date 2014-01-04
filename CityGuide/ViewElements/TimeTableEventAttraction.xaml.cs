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

        public TimeTableEventAttraction()
        {
            InitializeComponent();

            //Lock button Events
            LockButton.TouchDown += TouchLockButton;
            LockButton.Click += MouseClickLockButton;

            AttrationNameLabel.Foreground = new SolidColorBrush(Colors.Black);
            AttrationNameLabel.TouchDown += TouchEventLabel;

            ResizeCanvas.TouchMove += TouchMoveResizeButton;
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

        private static void TouchMoveResizeButton(Object sender, TouchEventArgs e)
        {
            //TODO: Add rezise and recoginition off time
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

        private void TouchEventLabel(Object sender, TouchEventArgs e)
        {
            if (Event.GetType() == typeof(EventAttraction))
            {
                var eventAttraction = Event as EventAttraction;
                //TODO: Open Information for Event
            }
        }
    }
}
