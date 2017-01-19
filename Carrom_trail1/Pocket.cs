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
        
        public Pocket (int radius, Point origin) : base (radius, origin)
            {
            pocket = new Ellipse ();
            pocket.Height = pocket.Width = radius;
            pocket.Fill = new SolidColorBrush (Colors.Black);
            }
        }
    }
