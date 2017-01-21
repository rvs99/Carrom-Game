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
    class Striker : CarromObject
        {
        //assign this value from config file
        int weight;

        public Striker (int radius, Color color) : base (radius)
            {
            carromObject.Fill = new SolidColorBrush (color);
            }

        public void SetStrikerOrigin (Point p)
            {
            base.SetOrigin (p);
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
