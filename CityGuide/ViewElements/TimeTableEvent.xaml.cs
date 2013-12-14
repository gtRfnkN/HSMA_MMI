using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            LockButton.TouchDown += TouchLockButton;
            AttrationNameLabel.TouchDown += TouchEventLabel;
            ResizeCanvas.TouchMove += TouchMoveResizeButton;
        }

        private void TouchLockButton(Object sender, TouchEventArgs e)
        {
            var button = sender as Button;
            bool state = !this.Event.IsLocked;
            if (state)
            {
                if (button != null) button.Content = "Unlock";
            }
            else
            {
                if (button != null) button.Content = "Lock";
            }
            this.Event.IsLocked = state;
        }

        private void TouchMoveResizeButton(Object sender, TouchEventArgs e)
        {
            //TODO: Add rezise and recoginition off time
        }

        private void TouchEventLabel(Object sender, TouchEventArgs e)
        {
            //TODO: Open Information for Event
        }
    }
}
