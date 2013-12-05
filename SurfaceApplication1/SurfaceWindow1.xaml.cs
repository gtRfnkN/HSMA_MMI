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
        private Dictionary<long, Color> filterColors;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            filterCircles = new List<TagCircle>();

            filterColors = new Dictionary<long, Color>();
            filterColors.Add(0x00, Color.FromArgb(90, 16, 143, 151));
            filterColors.Add(0x01, Color.FromArgb(90, 255, 139, 107));
            filterColors.Add(0x02, Color.FromArgb(90, 255, 227, 159));
            filterColors.Add(0x03, Color.FromArgb(90, 22, 134, 109));
            filterColors.Add(0x64, Color.FromArgb(90, 16, 54, 54));
        }

        private void tagDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized() && filterColors.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                Point tp = e.GetTouchPoint(cnv_Interact).Position;
                TagCircle circle = new TagCircle(tp.X, tp.Y, cnv_Draw, cnv_Interact, e.TouchDevice.Id, e.TouchDevice.GetOrientation(cnv_Draw), filterColors[e.TouchDevice.GetTagData().Value]);
                filterCircles.Add(circle);
            }
        }

        private void tagMove(object sender, TouchEventArgs e)
        {
            TagCircle filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (e.TouchDevice.GetIsTagRecognized() && filterColors.ContainsKey(e.TouchDevice.GetTagData().Value))
            {
                // update position
                Point tp = e.GetTouchPoint(cnv_Draw).Position;
                filterCircle.updateTransform(tp.X, tp.Y, e.TouchDevice.GetOrientation(cnv_Draw));
            }
        }

        private void tagGone(object sender, TouchEventArgs e)
        {
            TagCircle filterCircle = getCircleByTag(e.TouchDevice.Id);
            if (e.TouchDevice.GetIsTagRecognized() && filterColors.ContainsKey(e.TouchDevice.GetTagData().Value))
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
        private Rectangle rect; // TextBlock wrapper
        private TextBlock text; // TextBlock for the radius
        private int tag;        // tag

        // canvas of the application to draw non-interactive elements
        private Canvas draw;
        // canvas to draw interactive elements + interactions
        private Canvas interact;
        // canvas to group non-interactive elements
        private Canvas drawContainer;
        // canvas to group interactive elements
        private Canvas interactContainer;

        // current search radius in pixel units
        private int radius = 200;

        // width and height of the textbox
        private int TEXTBOX_WIDTH = 50;
        private int TEXTBOX_HEIGHT = 30;
        private int TEXTBLOCK_HEIGHT = 20;

        // constructor
        public TagCircle(double posX, double posY, Canvas _draw, Canvas _interact, int newTag, double rotation, Color newColor)
        {
            // save the canvas and tag for later use
            draw = _draw;
            interact = _interact;
            tag = newTag;

            // create containers
            drawContainer = new Canvas();
            interactContainer = new Canvas();

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

            // add to grid
            drawContainer.Children.Add(circle);

            // Create a rectangle
            rect = new Rectangle();

            // fill the circle and add a stroke
            rect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            rect.StrokeThickness = 2;
            rect.Stroke = Brushes.White;

            // set width and height
            rect.Width = TEXTBOX_WIDTH;
            rect.Height = TEXTBOX_HEIGHT;

            // set position
            Canvas.SetLeft(rect, -(rect.Width / 2));
            Canvas.SetTop(rect, -(radius / 2) - (rect.Height / 2));

            //TODO: ADD Z INDEX

            // touch handlers
            rect.TouchMove += rectMove;
            rect.IsManipulationEnabled = true;

            // add to grid
            interactContainer.Children.Add(rect);

            // Create a rectangle
            text = new TextBlock();

            // set width and height
            text.Width = TEXTBOX_WIDTH;
            text.Height = TEXTBLOCK_HEIGHT;

            // set position
            Canvas.SetLeft(text, -(rect.Width / 2));
            Canvas.SetTop(text, -(radius / 2) - (rect.Height / 2));

            // set text output
            text.Text = Math.Round((radius/1000.0), 2)+"km";
            text.FontSize = 14;
            text.TextAlignment = TextAlignment.Center;

            // touch handlers
            text.TouchMove += rectMove;
            text.IsManipulationEnabled = true;

            // add to grid
            interactContainer.Children.Add(text);

            // transform the containers
            Matrix m = new Matrix();
            m.Translate(posX, posY);
            m.RotateAt(rotation, posX, posY);

            drawContainer.RenderTransform = new MatrixTransform(m);
            interactContainer.RenderTransform = new MatrixTransform(m);

            // add grid to canvas
            draw.Children.Add(drawContainer);
            interact.Children.Add(interactContainer);
        }

        // get the tag
        public int getTag()
        {
            return tag;
        }

        // update the rotation
        public void updateTransform(double posX, double posY, double angle)
        {
            Matrix m = new Matrix();
            m.Translate(posX, posY);
            m.RotateAt(angle, posX, posY);

            drawContainer.RenderTransform = new MatrixTransform(m);
            interactContainer.RenderTransform = new MatrixTransform(m);
        }

        public void clear()
        {
            draw.Children.Remove(drawContainer);
            interact.Children.Remove(interactContainer);
        }

        private void rectMove(object sender, TouchEventArgs e)
        {
            // get the position of the finger relative to the center
            Point tp = e.GetTouchPoint(interactContainer).Position;
            radius = (int)Math.Sqrt((tp.X) * (tp.X) + (tp.Y) * (tp.Y)) * 2;

            // update the element size
            updateSize();
        }

        // update the size of the elements by its radius
        public void updateSize()
        {
            // set width and height
            circle.Width = radius;
            circle.Height = radius;

            // set position of the circle
            Canvas.SetLeft(circle, -(radius / 2));
            Canvas.SetTop(circle, -(radius / 2));

            // set position of the rect
            Canvas.SetLeft(rect, -(rect.Width / 2));
            Canvas.SetTop(rect, -(radius / 2) - (rect.Height / 2));

            // set position of the text
            Canvas.SetLeft(text, -(text.Width / 2));
            Canvas.SetTop(text, -(radius / 2) - (text.Height / 2));

            // set text output
            text.Text = Math.Round((radius / 1000.0), 2) + "km";
        }
    }
}