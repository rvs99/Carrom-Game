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
    public class Coin : CarromObject
        {
        //assign this value from config file
        public double Weight
            {
            get; set;
            }
        public double currentVelocity;
        public int initialHitTime;
        public double intialVelocity;
        private bool pocketed;
        public bool IsPocketed
            {
            get
                {
                return pocketed;
                }
            set
                {
                if (value == true)
                    {
                    Game.carromBoard.Children.Remove (this.GetBaseElement ());
                    pocketed = true;
                    }
                }
            }

        public Border LastHitBorder
            {
            get; set;
            }

        public bool IsQueen
            {
            get; set;
            }
        public Player Player
            {
            get; set;
            }
        public int CoinNumber
            {
            get; set;
            }

        public void AssignPlayer (Player player)
            {
            Player = player;
            }

        //Constructor
        public Coin (int radius, Color color) : base (radius)
            {
            carromObject.Fill = new SolidColorBrush (color);
            }

        public void SetCoinOrigin (Point p)
            {
            base.SetOrigin (p);
            }

        //Will be used to move a Coin
        //Two scenarios 
        //  1. Apply force, direction and Coin will stop at perticular point
        //  2. Apply force, direction and Coin will collide with other Coins or Edge or Striker
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
