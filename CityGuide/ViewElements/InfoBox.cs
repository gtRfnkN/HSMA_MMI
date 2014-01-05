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

namespace CityGuide.ViewElements
{
    class InfoBox : ScatterViewItem
    {
        private SurfaceButton _closeButton;
        private Label _titleLabel;
        private Canvas _uiElements;
        private Image _attractionImage;
        private TextBox _descriptionTextBox;

        public InfoBox()
        {
            this.Width = 400;
            this.Height = 350;
            this.CanScale = false;
            this.Background = new SolidColorBrush(Colors.White);
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
            //TODO Init imageBox
            _attractionImage = new Image();
            _attractionImage.Height = 150;
            _attractionImage.Width = this.Width;
            _attractionImage.Source = new BitmapImage(new Uri("C:\\Users\\Patrick\\Documents\\GitHub\\HSMA_MMI\\CityGuide\\Resources\\wasserturm.jpg"));
            Canvas.SetLeft(_attractionImage, 0);
            Canvas.SetTop(_attractionImage, 40);
            //TODO Init TextBox
            _descriptionTextBox = new TextBox();
            _descriptionTextBox.Height = 160;
            _descriptionTextBox.Width = this.Width;
            _descriptionTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            _descriptionTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            _descriptionTextBox.Text = "Lustiger Beschreibungstext!";
            _descriptionTextBox.IsReadOnly = true;
            Canvas.SetLeft(_descriptionTextBox, 0);
            Canvas.SetTop(_descriptionTextBox, 190);
            //Add elements to Canvas
            _uiElements.Children.Add(_titleLabel);
            _uiElements.Children.Add(_closeButton);
            _uiElements.Children.Add(_attractionImage);
            _uiElements.Children.Add(_descriptionTextBox);
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
            : base()
        {

        }
    }
}
