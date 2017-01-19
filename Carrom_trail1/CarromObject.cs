using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Carrom_trail1
    {
    public class CarromObject
        {
        public int radius;
        public int location_X, location_Y;

        public CarromObject (int radius, Point origin)
            {
            this.radius = radius;
            location_X = (int)Math.Round (origin.X - radius);
            location_Y = (int)Math.Round (origin.Y - radius);
            }
        }
    }
