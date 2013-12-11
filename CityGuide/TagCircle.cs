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

    public class TagCircle
    {
        private Ellipse circle; // circle
        private Rectangle rect; // TextBlock wrapper
        private TextBlock text; // TextBlock for the radius
        private Rectangle dragger;
        private int tag;        // tag

        // canvas of the application to draw non-interactive elements
        private Canvas drawCanvas;

        // canvas to draw interactive elements + interactions
        private Canvas interactCanvas;

        // canvas to group non-interactive elements
        private Canvas drawContainer;

        // canvas to group interactive elements
        private Canvas interactContainer;

        // current search radius in pixel units
        private int radius = 200;

        // width and height of the textbox
        private int TEXTBOX_WIDTH = 56;
        private int TEXTBOX_HEIGHT = 24;
        private int TEXTBLOCK_HEIGHT = 20;

        // constructor
        public TagCircle(double posX, double posY, Canvas _draw, Canvas _interact, int newTag, double rotation, Color newColor)
        {
            // SAVE THE CANVAS AND THE TAG
            drawCanvas = _draw;
            interactCanvas = _interact;
            tag = newTag;

            // CREATE DRAWING CONTAINERS TO HOLD THE SINGLE ELEMENTS
            drawContainer = new Canvas();
            interactContainer = new Canvas();

            // CIRCLE FOR THE TAG VISUALISATION
            // Create an ellipse
            circle = new Ellipse();

            // fill the circle and add a stroke
            circle.Fill = new SolidColorBrush(newColor);
            circle.StrokeThickness = 2;
            circle.Stroke = Brushes.White;

            // set width and height
            circle.Width = radius;
            circle.Height = radius;

            // set position
            Canvas.SetLeft(circle, -(radius / 2));
            Canvas.SetTop(circle, -(radius / 2));


            // RECTANGLE HOLDING THE RANGE TEXT
            // Create a rectangle
            rect = new Rectangle();

            // fill the circle and add a stroke
            rect.Fill = Brushes.White;
            rect.StrokeThickness = 2;
            rect.Stroke = Brushes.White;

            // set width and height
            rect.Width = TEXTBOX_WIDTH;
            rect.Height = TEXTBOX_HEIGHT;

            // set position
            Canvas.SetLeft(rect, -(rect.Width / 2));
            Canvas.SetTop(rect, -(radius / 2) - (rect.Height / 3));

            rect.RadiusX = 5;
            rect.RadiusY = 5;


            // TEXTBLOCK SHOWING THE RANGE
            // Create a rectangle
            text = new TextBlock();

            // set width and height
            text.Width = TEXTBOX_WIDTH;
            text.Height = TEXTBLOCK_HEIGHT;

            // set position of the text
            Canvas.SetLeft(text, -(text.Width / 2));
            Canvas.SetTop(text, -(radius / 2) - (text.Height / 3) + 1);

            // set text output
            text.Text = Math.Round((radius / 1000.0), 2) + " km";
            text.FontSize = 14;
            text.TextAlignment = TextAlignment.Center;
            text.Foreground = Brushes.Black;


            // RECTANGLE HOLDING THE RANGE TEXT
            // Create a rectangle
            dragger = new Rectangle();

            // 
            dragger.Fill = Brushes.Transparent;

            // set width and height
            dragger.Width = TEXTBOX_WIDTH * 1.5;
            dragger.Height = TEXTBOX_HEIGHT * 1.5;

            // set position
            Canvas.SetLeft(dragger, -(dragger.Width / 2));
            Canvas.SetTop(dragger, -(radius / 2) - (dragger.Height / 3));

            // touch handlers
            dragger.TouchMove += rectMove;
            dragger.TouchDown += rectClicked;
            dragger.IsManipulationEnabled = true;

            draw();

            // update the transformation
            updateTransform(posX, posY, rotation);
        }

        // get the tag
        public int getTag()
        {
            return tag;
        }

        public void draw()
        {
            // add the circle to the draw container
            drawContainer.Children.Add(circle);
            drawContainer.Children.Add(rect);
            drawContainer.Children.Add(text);

            interactContainer.Children.Add(dragger);

            // add grid to canvas
            drawCanvas.Children.Add(drawContainer);
            interactCanvas.Children.Add(interactContainer);
        }

        public void clear()
        {
            drawCanvas.Children.Remove(drawContainer);
            interactCanvas.Children.Remove(interactContainer);
        }

        public void redraw()
        {
            drawCanvas.Children.Remove(drawContainer);
            drawCanvas.Children.Add(drawContainer);
        }

        // update the position and rotation
        public void updateTransform(double posX, double posY, double angle)
        {
            Matrix m = new Matrix();
            m.Translate(posX, posY);
            m.RotateAt(angle, posX, posY);

            drawContainer.RenderTransform = new MatrixTransform(m);
            interactContainer.RenderTransform = new MatrixTransform(m);
        }

        private void rectClicked(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsTagRecognized())
            {
                redraw();
            }
            else
            {
                e.Handled = false;
            }
        }

        private void rectMove(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsTagRecognized())
            {
                // get the position of the finger relative to the center
                Point tp = e.GetTouchPoint(interactContainer).Position;
                radius = (int)Math.Sqrt((tp.X) * (tp.X) + (tp.Y) * (tp.Y)) * 2;

                // update the element size
                updateSize();
            }
            else
            {
                e.Handled = false;
            }
        }

        // update the size of the elements by its radius
        public void updateSize()
        {
            /* TODO: DO NOT DRAG OUTSIDE
            double rectX = -(rect.Width / 2);
            double rectY = -(radius / 2) - (rect.Height / 3);
            if (rectX < 0 || rectX > draw.ActualWidth || rectY < 0 || rectY > draw.ActualHeight)
                return;*/

            // set width and height
            circle.Width = radius;
            circle.Height = radius;

            // set position of the circle
            Canvas.SetLeft(circle, -(radius / 2));
            Canvas.SetTop(circle, -(radius / 2));

            // set position of the rect
            Canvas.SetLeft(rect, -(rect.Width / 2));
            Canvas.SetTop(rect, -(radius / 2) - (rect.Height / 3));

            // set position of the text
            Canvas.SetLeft(text, -(text.Width / 2));
            Canvas.SetTop(text, -(radius / 2) - (text.Height / 3));

            // set position of the dragger
            Canvas.SetLeft(dragger, -(dragger.Width / 2));
            Canvas.SetTop(dragger, -(radius / 2) - (dragger.Height / 3));

            // set text output
            text.Text = Math.Round((radius / 1000.0), 2) + " km";
        }
    }
}
