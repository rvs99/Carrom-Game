using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Carrom_trail1
    {
    class Pocket : CarromObject
        {
        Ellipse pocket;
        
        public Pocket (int radius, int location_X, int location_Y) : base (radius, new Point(location_X, location_Y))
            {
            pocket = new Ellipse ();
            pocket.Height = pocket.Width = radius;
            pocket.Fill = new SolidColorBrush (Colors.Black);
            }
        }
    }
