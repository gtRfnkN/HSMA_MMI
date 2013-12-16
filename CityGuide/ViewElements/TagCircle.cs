using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CityGuide.Data;
using Microsoft.Surface.Presentation.Input;

namespace CityGuide.ViewElements
{

    public class TagCircle
    {
        #region Fields
        public Filter Filter { get; set; }
        public long TagID{ get; set; }
        public bool IsDrawn { get; private set; }
        // canvas of the application to draw non-interactive elements
        public Canvas DrawCanvas { get; set; }

        // canvas to draw interactive elements + interactions
        public Canvas InteractCanvas{ get; set; }

        private readonly Ellipse _circle; // circle
        private readonly Rectangle _rect; // TextBlock wrapper
        private readonly TextBlock _text; // TextBlock for the radius
        private readonly Rectangle _dragger;

        private bool _firstDraw = true;
        
        // canvas to group non-interactive elements
        private readonly Canvas _drawContainer = new Canvas();

        // canvas to group interactive elements
        private readonly Canvas _interactContainer = new Canvas();

        // width and height of the textbox
        private const int TEXTBOX_WIDTH = 56;
        private const int TEXTBOX_HEIGHT = 24;
        private const int TEXTBLOCK_HEIGHT = 20;
        #endregion
        
        #region constructors
        public TagCircle(Filter filter)
        {
            this.IsDrawn = false;
            this.Filter = filter;
            // Create an ellipse
            _circle = new Ellipse
            {
                // fill the circle and add a stroke
                Fill = new SolidColorBrush(Filter.Color),
                StrokeThickness = 2,
                Stroke = Brushes.White,

                // set width and height
                Width = Filter.Radius,
                Height = Filter.Radius
            };

            // Create a rectangle
            _rect = new Rectangle
            {
                // fill the circle and add a stroke
                Fill = Brushes.White,
                StrokeThickness = 2,
                Stroke = Brushes.White,

                // set width and height
                Width = TEXTBOX_WIDTH,
                Height = TEXTBOX_HEIGHT,


                RadiusX = 5,
                RadiusY = 5
            };

            // Create a rectangle
            _text = new TextBlock
            {
                // set width and height
                Width = TEXTBOX_WIDTH,
                Height = TEXTBLOCK_HEIGHT,

                // set text output
                Text = Math.Round((Filter.Radius / 1000.0), 2) + " km",
                FontSize = 14,
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.Black
            };

            // Create a rectangle
            _dragger = new Rectangle
            {
                Fill = Brushes.Transparent,

                // set width and height
                Width = TEXTBOX_WIDTH * 1.5,
                Height = TEXTBOX_HEIGHT * 1.5
            };

            // touch handlers
            _dragger.TouchMove += RectMove;
            _dragger.TouchDown += RectClicked;
            _dragger.IsManipulationEnabled = true;
        }


        // constructor
        public TagCircle(double posX, double posY, double angle, long newTag, Filter filter)
            : this(filter)
        {
            // SAVE THE CANVAS AND THE TAG
            this.TagID = newTag;

            UpdateSize();
            Draw();

            // update the transformation
            UpdateTransform(posX, posY, angle);
        }
        #endregion

        #region Methods

        public void Draw()
        {
            if (_firstDraw)
            {
                // add the circle to the draw container
                _drawContainer.Children.Add(_circle);
                _drawContainer.Children.Add(_rect);
                _drawContainer.Children.Add(_text);

                _interactContainer.Children.Add(_dragger);

                // add grid to canvas
                DrawCanvas.Children.Add(_drawContainer);
                InteractCanvas.Children.Add(_interactContainer);
                _firstDraw = false;
                this.IsDrawn = true;
            }
            else
            {
                Redraw();
            }
        }

        public void Clear()
        {
            DrawCanvas.Children.Remove(_drawContainer);
            InteractCanvas.Children.Remove(_interactContainer);
            this.IsDrawn = false;
        }

        public void Redraw()
        {
            DrawCanvas.Children.Remove(_drawContainer);
            DrawCanvas.Children.Add(_drawContainer);
            this.IsDrawn = true;
        }

        // update the position and rotation
        public void UpdateTransform(double posX, double posY, double angle)
        {
            var m = new Matrix();
            m.Translate(posX, posY);
            m.RotateAt(angle, posX, posY);

            _drawContainer.RenderTransform = new MatrixTransform(m);
            _interactContainer.RenderTransform = new MatrixTransform(m);
        }

        private void RectClicked(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsTagRecognized())
            {
                Redraw();
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void RectMove(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsTagRecognized())
            {
                // get the position of the finger relative to the center
                Point tp = e.GetTouchPoint(_interactContainer).Position;
                Filter.Radius = (int)Math.Sqrt((tp.X) * (tp.X) + (tp.Y) * (tp.Y)) * 2;

                // update the element size
                UpdateSize();
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        // update the size of the elements by its radius
        public void UpdateSize()
        {
            /* TODO: DO NOT DRAG OUTSIDE
            double rectX = -(rect.Width / 2);
            double rectY = -(radius / 2) - (rect.Height / 3);
            if (rectX < 0 || rectX > draw.ActualWidth || rectY < 0 || rectY > draw.ActualHeight)
                return;*/

            // set width and height
            _circle.Width = Filter.Radius;
            _circle.Height = Filter.Radius;

            // set position of the circle
            Canvas.SetLeft(_circle, -(Filter.Radius / 2));
            Canvas.SetTop(_circle, -(Filter.Radius / 2));

            // set position of the rect
            Canvas.SetLeft(_rect, -(_rect.Width / 2));
            Canvas.SetTop(_rect, -(Filter.Radius / 2) - (_rect.Height / 3));

            // set position of the text
            Canvas.SetLeft(_text, -(_text.Width / 2));
            Canvas.SetTop(_text, -(Filter.Radius / 2) - (_text.Height / 3));

            // set position of the dragger
            Canvas.SetLeft(_dragger, -(_dragger.Width / 2));
            Canvas.SetTop(_dragger, -(Filter.Radius / 2) - (_dragger.Height / 3));

            // set text output
            _text.Text = Math.Round((Filter.Radius / 1000.0), 2) + " km";
        }
        #endregion
    }
}
