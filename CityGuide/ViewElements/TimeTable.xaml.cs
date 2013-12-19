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
    /// Interaktionslogik für TimeTable.xaml
    /// </summary>
    public partial class TimeTable : UserControl
    {
        public TimeTable()
        {
            InitializeComponent();
            DropTarget0815.Drop += TimeTableDrop;
            DropTarget0815.DragOver += TimeTableDragOver;
        }

        private static void TimeTableDragOver(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = new SolidColorBrush(Colors.RoyalBlue);
            }
        }

        private static void TimeTableDrop(object sender, DragEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (e.Data.GetDataPresent("Attraction") && rectangle != null)
            {
                var attraction = e.Data.GetData("Attraction") as Attraction;
                if (attraction != null)
                {
                    rectangle.Fill = new SolidColorBrush(Colors.Red);
                }
            }
        }
    }
}
