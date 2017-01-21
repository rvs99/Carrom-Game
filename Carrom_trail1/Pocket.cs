using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Carrom
    {
    class Pocket : CarromObject
        {
        public Pocket (int radius, Point p) : base (radius)
            {
            carromObject.Fill = new SolidColorBrush (Colors.Black);
            SetPocketOrigin (p);
            }

        public Pocket (int radius, int x, int y) : base (radius)
            {
            carromObject.Fill = new SolidColorBrush (Colors.Black);
            SetPocketOrigin (x, y);
            }

        public void SetPocketOrigin (Point p)
            {
            SetOrigin (p);
            }

        public void SetPocketOrigin (int x, int y)
            {
            SetOrigin (new Point (x, y));
            }
        }
    }
