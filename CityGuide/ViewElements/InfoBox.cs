using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CityGuide.Data;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Shapes;

namespace CityGuide.ViewElements
{
    class InfoBox : ScatterViewItem
    {
        //private Label _titleLabel;
        private SurfaceButton _closeButton;
        private Label _titleLabel;
        private Canvas UiElements;

        public InfoBox()
        {
            this.Width = 450;
            this.Height = 350;
            this.CanScale = false;
            this.Background = new SolidColorBrush(Colors.White);
            UiElements = new Canvas();
            InitUiElements();
            this.AddChild(UiElements);
        }

        private void InitUiElements()
        {
            this.Padding = new System.Windows.Thickness(0);
            //Format label
            _titleLabel = new Label();
            _titleLabel.Width = this.Width;
            _titleLabel.Height = 40;
            _titleLabel.Content = "toller titel";
            _titleLabel.Background = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(_titleLabel, 0);
            Canvas.SetTop(_titleLabel, 0);
            //Format CloseButton
            var triangle = new Polygon();
            triangle.Points.Add(new System.Windows.Point(0,0));
            triangle.Points.Add(new System.Windows.Point(40,40));
            triangle.Points.Add(new System.Windows.Point(40,0));
            _closeButton = new SurfaceButton();
            _closeButton.Content = triangle;
            _closeButton.MinHeight = 10;
            _closeButton.MinWidth = 10;
            _closeButton.Width = 40;
            _closeButton.Height = 40;
            //_closeButton.Background = new SolidColorBrush(Colors.Green);
            Canvas.SetLeft(_closeButton, this.Width - _closeButton.Width - 1);
            Canvas.SetTop(_closeButton, -1);
            //Add elements to Canvas
            UiElements.Children.Add(_titleLabel);
            UiElements.Children.Add(_closeButton);
        }

        public InfoBox(Attraction attraction)
            : base()
        {

        }
    }
}
