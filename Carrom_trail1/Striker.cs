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
    class Striker : CarromObject
        {
        //assign this value from config file
        int weight;
        Ellipse striker;

        public Striker (int radius, int location_X, int location_Y, Color color) : base (radius, new Point (location_X, location_Y))
            {
            striker = new Ellipse ();
            striker.Height = striker.Width = radius;
            striker.Fill = new SolidColorBrush (color);
            }

        //Will be used to move a Striker
        //Two scenarios 
        //  1. Apply force, direction and Striker will stop at perticular point
        //  2. Apply force, direction and Striker will collide with Edge or another coins
        public void Move (float force, float angle)
            {

            }
        }
    }
