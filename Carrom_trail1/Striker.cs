using System;
using System.Windows;
using System.Windows.Media;

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
        public Player Player
            {
            get; set;
            }

        public Striker (int radius, Color color) : base (radius)
            {
            carromObject.Fill = new SolidColorBrush (color);
            }

        //Will be used to move a Striker
        //Two scenarios 
        //  1. Apply force, direction and Striker will stop at perticular point
        //  2. Apply force, direction and Striker will collide with Edge or another coins
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
            //if (currentVelocity <= 0.001)
            //    Game.striker.SetOrigin (new Point (370, 633));
            }
        }
    }
