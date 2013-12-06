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
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using SurfaceApplication1.Data;

namespace SurfaceApplication1
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private List<Ellipse> filterCircles;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            filterCircles = new List<Ellipse>();

            var waterTowerLocation = new Location(49.484076, 8.475525);
            var pinAttraction = new Attraction { Name = "Wasserturm", GeoLocation = waterTowerLocation, Teaser = "Das ist der Wasserturm" };

            Map.Children.Add(pinAttraction);
            MapLayer.SetPosition(pinAttraction, pinAttraction.GeoLocation);
        }

        private void tagDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // Create a red Ellipse.
                Ellipse filterCircle = new Ellipse();

                // Create a SolidColorBrush with a red color to fill the  
                // Ellipse with.
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values.  
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Color.FromArgb(40, 255, 255, 255);
                filterCircle.Fill = mySolidColorBrush;
                filterCircle.StrokeThickness = 2;
                filterCircle.Stroke = Brushes.White;

                // Set the width and height of the Ellipse.
                filterCircle.Width = 200;
                filterCircle.Height = 200;

                // set position
                Canvas _canvas = (Canvas)sender as Canvas;
                Point tp = e.GetTouchPoint(_canvas).Position;
                Canvas.SetLeft(filterCircle, tp.X - 100);
                Canvas.SetTop(filterCircle, tp.Y - 100);

                // add to scene
                wrapper.Children.Add(filterCircle);

                // append circle object with tag id
                filterCircle.Tag = e.TouchDevice.Id;
                filterCircles.Add(filterCircle);
            }
        }

        private void tagMove(object sender, TouchEventArgs e)
        {
            Ellipse filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (filterCircle != null && e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // update position
                Canvas _canvas = (Canvas)sender as Canvas;
                Point tp = e.GetTouchPoint(_canvas).Position;
                Canvas.SetLeft(filterCircle, tp.X - 100);
                Canvas.SetTop(filterCircle, tp.Y - 100);
            }
        }

        private void tagGone(object sender, TouchEventArgs e)
        {
            Ellipse filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (filterCircle != null && e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // remove from scene
                wrapper.Children.Remove(filterCircle);

                // remove from list
                filterCircles.Remove(filterCircle);
            }
        }

        private Ellipse getCircleByTag(int tag)
        {
            foreach (Ellipse circle in filterCircles)
            {
                if ((int)circle.Tag == tag)
                {
                    return circle;
                }
            }

            return null;
        }
    }
}