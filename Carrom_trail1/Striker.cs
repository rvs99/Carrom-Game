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
    public class Striker : CarromObject
        {
        //assign this value from config file
        int weight;
        public int initialHitTime;
        public double currentVelocity;
        public double intialVelocity;
        public Border LastHitBorder
            {
            get; set;
            }

        public Striker (int radius, Color color) : base (radius)
            {
            carromObject.Fill = new SolidColorBrush (color);
            }

        //public void SetStrikerOrigin (Point p)
        //    {
        //    base.SetOrigin (p);
        //    }

        //Will be used to move a Striker
        //Two scenarios 
        //  1. Apply force, direction and Striker will stop at perticular point
        //  2. Apply force, direction and Striker will collide with Edge or another coins
        public void Move (float force, float angle)
            {

            }

            public void Move (double angle, int timeInMillis)
            {
            timeInMillis = initialHitTime += 3;
            Point result = new Point ();

            double distance = (0.5) * (currentVelocity + intialVelocity) * timeInMillis;
            result.X = origin_X + (int)Math.Round (distance * Math.Cos (angle));
            result.Y = origin_Y + (int)Math.Round (distance * Math.Sin (angle));
            intialVelocity = currentVelocity;
            currentVelocity *= 0.9;
            SetOrigin (result);
            }
        }
    }
