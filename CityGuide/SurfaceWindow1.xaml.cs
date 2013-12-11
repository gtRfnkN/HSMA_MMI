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
                if (circle.getTag() == tag)
                    return circle;
            return null;
        }
    }
}