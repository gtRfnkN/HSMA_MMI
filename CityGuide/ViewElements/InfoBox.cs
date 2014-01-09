using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CityGuide.Data;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;

namespace CityGuide.ViewElements
{
    class InfoBox : ScatterViewItem
    {
        private SurfaceButton _closeButton;
        private Label _titleLabel;
        private Canvas _uiElements;
        private Image _attractionImage;
        private TextBox _descriptionTextBox;
        private TextBox _openingHoursTextBox;
        private TextBox _informationTextBox;

        private Attraction _attraction;
        private Point _startPoint;

        public InfoBox()
        {
            this.Width = 450;
            this.Height = 350;
            this.ZIndex = 1000;
            this.CanScale = false;
            this.Background = new SolidColorBrush(Colors.White);
            this.Deceleration = 0.002;
            _uiElements = new Canvas();
            InitUiElements();
            this.AddChild(_uiElements);
        }

        private void InitUiElements()
        {

            this.Padding = new System.Windows.Thickness(0);
            //Init titleLabel
            _titleLabel = new Label();
            _titleLabel.Width = this.Width;
            _titleLabel.Height = 40;
            _titleLabel.Content = "toller titel";
            _titleLabel.Background = new SolidColorBrush(Colors.Black);
            _titleLabel.TouchDown += LabelTouchDown;
            _titleLabel.TouchMove += LableTouchMove;
            _titleLabel.MouseDown += LabelMouseDown;
            _titleLabel.MouseMove += LableMouseMove;
            Canvas.SetLeft(_titleLabel, 0);
            Canvas.SetTop(_titleLabel, 0);
            //Init closeButton
            var triangle = new Polygon();
            triangle.Points.Add(new System.Windows.Point(0, 0));
            triangle.Points.Add(new System.Windows.Point(40, 40));
            triangle.Points.Add(new System.Windows.Point(40, 0));
            _closeButton = new SurfaceButton();
            _closeButton.Content = triangle;
            _closeButton.MinHeight = 10;
            _closeButton.MinWidth = 10;
            _closeButton.Width = 40;
            _closeButton.Height = 40;
            _closeButton.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(_closeButton_TouchUp);
            _closeButton.MouseDown += new System.Windows.Input.MouseButtonEventHandler(_closeButton_MouseUp);
            _closeButton.Click += new System.Windows.RoutedEventHandler(_closeButton_Click);
            _closeButton.Background = new SolidColorBrush(Colors.Blue);
            Canvas.SetLeft(_closeButton, this.Width - _closeButton.Width - 1);
            Canvas.SetTop(_closeButton, -1);
            //Init imageBox
            _attractionImage = new Image();
            _attractionImage.Height = 150;
            _attractionImage.Width = this.Width;
            _attractionImage.Source = new BitmapImage(new Uri("/Resources/wasserturm.jpg", UriKind.Relative));
            _attractionImage.Stretch = Stretch.Fill;
            Canvas.SetLeft(_attractionImage, 0);
            Canvas.SetTop(_attractionImage, 40);
            //Init TextBox
            _descriptionTextBox = new TextBox();
            _descriptionTextBox.Height = 120;
            _descriptionTextBox.Width = this.Width;
            _descriptionTextBox.TextWrapping = System.Windows.TextWrapping.Wrap;
            //_descriptionTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //_descriptionTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            _descriptionTextBox.Text = "Lustiger Beschreibungstext!";
            _descriptionTextBox.IsReadOnly = true;
            _descriptionTextBox.BorderThickness = new System.Windows.Thickness(0);
            Canvas.SetLeft(_descriptionTextBox, 0);
            Canvas.SetTop(_descriptionTextBox, 250);
            //TODO Öffnungszeiten Box
            _openingHoursTextBox = new TextBox();
            _openingHoursTextBox.Height = 60;
            _openingHoursTextBox.Width = this.Width / 2;
            _openingHoursTextBox.IsReadOnly = true;
            _openingHoursTextBox.FontSize = 10.0;
            _openingHoursTextBox.Background = new SolidColorBrush(Colors.Black);
            _openingHoursTextBox.Foreground = new SolidColorBrush(Colors.White);
            _openingHoursTextBox.Text = "Öffnungszeiten";
            _openingHoursTextBox.BorderThickness = new System.Windows.Thickness(0);
            Canvas.SetLeft(_openingHoursTextBox, 0);
            Canvas.SetTop(_openingHoursTextBox, 190);
            //TODO Anschrift
            _informationTextBox = new TextBox();
            _informationTextBox = new TextBox();
            _informationTextBox.Height = 60;
            _informationTextBox.Width = this.Width /2;
            _informationTextBox.Text = "Informationen";
            _informationTextBox.IsReadOnly = true;
            _informationTextBox.FontSize = 10.0;
            _informationTextBox.Background = new SolidColorBrush(Colors.Black);
            _informationTextBox.Foreground = new SolidColorBrush(Colors.White);
            _informationTextBox.BorderThickness = new System.Windows.Thickness(0);
            Canvas.SetLeft(_informationTextBox, this.Width/2);
            Canvas.SetTop(_informationTextBox, 190);
            //Add elements to Canvas
            _uiElements.Children.Add(_titleLabel);
            _uiElements.Children.Add(_closeButton);
            _uiElements.Children.Add(_attractionImage);
            _uiElements.Children.Add(_descriptionTextBox);
            _uiElements.Children.Add(_openingHoursTextBox);
            _uiElements.Children.Add(_informationTextBox);
        }

        void _closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            closeInfoBox();
        }

        void _closeButton_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            closeInfoBox();
        }

        void _closeButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            closeInfoBox();
        }

        private void closeInfoBox()
        {
            ((ScatterView)this.Parent).Items.Remove(this);
        }

        public InfoBox(Attraction attraction)
            : this()
        {
            _titleLabel.Content = attraction.Titel;
            _descriptionTextBox.Text = attraction.Teaser;
            _openingHoursTextBox.Text = attraction.OpeningHours;
            _informationTextBox.Text = attraction.Information;
            _attraction = attraction;
        }

        #region DragDrop Methods
        private void LabelTouchDown(object sender, TouchEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.TouchDevice.GetTouchPoint(this).Position;
            e.Handled = true;
        }

        private void LableTouchMove(object sender, TouchEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.TouchDevice.GetTouchPoint(this).Position;
            Vector diff = _startPoint - mousePos;
            //TODO: change to Infobox Object

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                //TODO: get Attraction from Infobox
                // Initialize the drag & drop operation
                var dragData = new DataObject("Attraction", _attraction);
                DragDrop.DoDragDrop(dragSource: this, data: dragData, allowedEffects: DragDropEffects.Move);
            }
            e.Handled = true;
        }

        private void LabelMouseDown(object sender, MouseEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void LableMouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(this);
            Vector diff = _startPoint - mousePos;
            //TODO: change to Infobox Object

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                //TODO: get Attraction from Infobox
                // Initialize the drag & drop operation
                var dragData = new DataObject("Attraction", _attraction);
                DragDrop.DoDragDrop(dragSource: this, data: dragData, allowedEffects: DragDropEffects.Move);
            }
            e.Handled = true;
        }
        #endregion 
    }
}
