using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CityGuide.Data;
using CityGuide.Data.Route;

namespace CityGuide.ViewElements
{
    /// <summary>
    /// Interaktionslogik für TimeTableEvent.xaml
    /// </summary>
    public partial class TimeTableEventTransportation : UserControl
    {
        private EventTransport _event;
        public EventTransport Event
        {
            get { return _event; }
            set
            {
                if (value != null)
                {
                    _event = value;
                    var eventTransport = _event as EventTransport;
                    TransportNameLabel.Content = "Auto " + eventTransport.DurationTime();
                    TransportNameLabel.Background = new SolidColorBrush(Colors.Orange);

                    if ((_event.Route.Duration /60/15) <= 1)
                    {
                        TransportNameLabel.FontSize = 8.0;
                        TransportNameLabel.FontWeight = FontWeights.Bold;
                    }
                    
                    String nameUID = "TimeTableEventTransporation" + eventTransport.TransportTyp + eventTransport.Order;
                    Uid = nameUID;
                    Name = nameUID;
                }
            }
        }

        public TimeTableEventTransportation()
        {
            InitializeComponent();

            TransportNameLabel.Foreground = new SolidColorBrush(Colors.Black);
            TransportNameLabel.TouchDown += TouchEventLabel;
        }

        private void TouchEventLabel(Object sender, TouchEventArgs e)
        {
           //TODO: Open Information for Event
        }
    }
}
