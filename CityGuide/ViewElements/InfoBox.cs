using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CityGuide.Data;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

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
            this.Width = 250;
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
            _titleLabel = new Label();
            _titleLabel.Width = this.Width;
            _titleLabel.Height = 20;
            _titleLabel.Content = "toller titel";
            _titleLabel.Background = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(_titleLabel, 0);
            Canvas.SetTop(_titleLabel, 0);
            _closeButton = new SurfaceButton();
            _closeButton.Content = "X";
            _closeButton.MinHeight = 10;
            _closeButton.MinWidth = 10;
            _closeButton.Width = 10;
            _closeButton.Height = 10;
            _closeButton.Background = new SolidColorBrush(Colors.Green);
            Canvas.SetLeft(_closeButton, this.Width - 11);
            Canvas.SetTop(_closeButton, -1);
            UiElements.Children.Add(_titleLabel);
            UiElements.Children.Add(_closeButton);
        }

        public InfoBox(Attraction attraction)
            : base()
        {

        }
    }
}
