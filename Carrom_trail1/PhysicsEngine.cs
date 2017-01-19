using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrom_trail1
    {
    public class PhysicsEngine
        {



        public void AddObject (CarromObject obj)
            {
            }

        //Will be used to move a CarromObject
        //Two scenarios 
        //  1. Apply force, direction and CarromObject will stop at perticular point
        //  2. Apply force, direction and CarromObject will collide with other CarromObjects (Coins or Striker or Pocket) or Edge
        public void HitCarromObject (CarromObject obj, float force, float angle)
            {

            //while ( obj not reaches its final point )
                //Move to perticular direction

            //Check if it collides with others
                
            //--------If yes, recalculate current objects direction and force
            //--------and find other collided objects and call HitCarromObject with its direction and force

            //--------If no, continue
            }

        public bool isCollided (CarromObject obj)
            {
            //Check if is collided with Edge

            //Check if it is collided with other Coin

            //Check if it is going inside Pocket
            return true;
            }


        }
    }
