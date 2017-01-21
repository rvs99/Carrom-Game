using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrom
    {
    public class PhysicsEngine
        {

        List<CarromObject> listOfCoins = new List<CarromObject> (19);
        Striker striker;
        List<CarromObject> listOfPockets = new List<CarromObject> (4);


        public void AddObject (CarromObject obj)
            {
            try
                {
                if (obj as Coin != null)
                    {
                    listOfCoins.Add (obj as Coin);
                    }
                else if (obj as Striker != null)
                    {
                    striker = obj as Striker;
                    }
                else if (obj as Pocket != null)
                    {
                    listOfPockets.Add( obj as Striker);
                    }
                }
            catch (Exception)
                {

                throw;
                }
            
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

        public enum CollisionResult
            {
            None,
            Edge,
            Coin,
            Striker
            }

        public CollisionResult isCollided (CarromObject obj)
            {
            //Check if is collided with Edge

            //Check if it is collided with other Coin


            //Check if it is going inside Pocket
            return CollisionResult.None;
            }

        public double GetStoppingDistance (CarromObject obj, double force)
            {
            double coefficientOfFriction = 0.25 * 9.8;
            double stoppingDistance = 0.0F;

            stoppingDistance = (force * force) / (2 * coefficientOfFriction);

            return stoppingDistance;
            }

        }
    }
