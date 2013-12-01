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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace SurfaceApplication1
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private List<TagCircle> filterCircles;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            filterCircles = new List<TagCircle>();
        }

        private void tagDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                Canvas _canvas = (Canvas)sender as Canvas;
                Point tp = e.GetTouchPoint(_canvas).Position;
                TagCircle circle = new TagCircle(tp.X, tp.Y, wrapper, e.TouchDevice.Id);
                filterCircles.Add(circle);
            }
        }

        private void tagMove(object sender, TouchEventArgs e)
        {
            TagCircle filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (filterCircle != null && e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // update position
                Canvas _canvas = (Canvas)sender as Canvas;
                Point tp = e.GetTouchPoint(_canvas).Position;
                filterCircle.updatePosition(tp.X, tp.Y);
            }
        }

        private void tagGone(object sender, TouchEventArgs e)
        {
            TagCircle filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (filterCircle != null && e.TouchDevice.GetIsTagRecognized() && e.TouchDevice.GetTagData().Value == 0xA5)
            {
                // remove from scene
                filterCircle.clear();

                // remove from list
                filterCircles.Remove(filterCircle);
            }
        }

        private TagCircle getCircleByTag(int tag)
        {
            foreach (TagCircle circle in filterCircles)
            {
                if (circle.getTag() == tag)
                {
                    return circle;
                }
            }

            return null;
        }
    }

    public class TagCircle
    {
        private Ellipse circle; // circle
        private int tag;        // tag

        private Canvas canvas;

        private int radius = 200;

        // constructor
        public TagCircle(double posX, double posY, Canvas c, int newTag)
        {
            // Create a red Ellipse.
            circle = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the  
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values.  
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(40, 255, 255, 255);
            circle.Fill = mySolidColorBrush;
            circle.StrokeThickness = 2;
            circle.Stroke = Brushes.White;

            // Set the width and height of the Ellipse.
            circle.Width = radius;
            circle.Height = radius;

            // set position
            Canvas.SetLeft(circle, posX - (radius / 2));
            Canvas.SetTop(circle, posY - (radius / 2));

            // add to scene
            canvas = c;
            canvas.Children.Add(circle);

            // set tag
            tag = newTag;
        }

        // get the tag
        public int getTag()
        {
            return tag;
        }

        // update the circle position
        public void updatePosition(double posX, double posY)
        {
            Canvas.SetLeft(circle, posX - (radius / 2));
            Canvas.SetTop(circle, posY - (radius / 2));
        }

        public void clear()
        {
            canvas.Children.Remove(circle);
        }
    }
}