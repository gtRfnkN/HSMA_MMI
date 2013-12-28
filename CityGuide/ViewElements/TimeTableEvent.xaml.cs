using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CityGuide.Data;

namespace CityGuide.ViewElements
{
    /// <summary>
    /// Interaktionslogik für TimeTableEvent.xaml
    /// </summary>
    public partial class TimeTableEvent : UserControl
    {
        public Event Event { get; set; }

        public TimeTableEvent()
        {
            InitializeComponent();
                        
            //Lock button Events
            LockButton.TouchDown += TouchLockButton;
            LockButton.MouseUp += MouseClickLockButton;

            AttrationNameLabel.TouchDown += TouchEventLabel;
            ResizeCanvas.TouchMove += TouchMoveResizeButton;
        }

        #region Lock Button Events & Methods
        private void TouchLockButton(Object sender, TouchEventArgs e)
        {
            var button = sender as Button;
            LockEvent(button);
        }

        private void MouseClickLockButton(Object sender, MouseButtonEventArgs e)
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

        private static void TouchMoveResizeButton(Object sender, TouchEventArgs e)
        {
            //TODO: Add rezise and recoginition off time
        }

        private void TouchEventLabel(Object sender, TouchEventArgs e)
        {
            if(Event.GetType() == typeof(EventAttraction)){
                var eventAttraction = Event as EventAttraction;
                //TODO: Open Information for Event
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
