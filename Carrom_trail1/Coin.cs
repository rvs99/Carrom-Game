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
    class Coin : CarromObject
        {
        //assign this value from config file
        int weight;
        public Ellipse coin;
        Player player;

        public void AssignPlayer (Player player)
            {
            this.player = player;
            }

        //Constructor
        public Coin (int radius, int location_X, int location_Y, Color color) : base (radius, new Point(location_X,location_Y))
            {
            coin = new Ellipse ();
            coin.Height = coin.Width = radius;
            coin.Fill = new SolidColorBrush (color);
            }

        //Will be used to move a Coin
        //Two scenarios 
        //  1. Apply force, direction and Coin will stop at perticular point
        //  2. Apply force, direction and Coin will collide with other Coins or Edge or Striker
        public void Move (float force, float angle)
            {

            }
        }
    }
