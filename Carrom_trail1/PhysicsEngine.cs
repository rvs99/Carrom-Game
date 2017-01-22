using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Carrom
    {
    public class PhysicsEngine
        {
        //static List<CarromObject> listOfCoins;
        //Striker striker;
        //static List<CarromObject> listOfPockets;
        static DispatcherTimer timer, changeStrikerValues, changeCoinValues;
        static double strikerForce, strikerAngle;
        static List<Point> allPoints;

        public static void Start ()
            {
            //listOfCoins = new List<CarromObject> (19);
            //listOfPockets = new List<CarromObject> (4);
            timer = new DispatcherTimer ();
            timer.Interval = TimeSpan.FromMilliseconds (10);
            timer.Tick += UpdateUI;
            timer.Start ();
            allPoints = new List<Point> ();
            }

        private static void UpdateUI (object sender, EventArgs e)
            {
            //Update all coins

            //Update striker
            Game.striker.Update ();
            Game.queen.Update ();
            }

        //public void AddObject (CarromObject obj)
        //    {
        //    try
        //        {
        //        if (obj as Coin != null)
        //            {
        //            listOfCoins.Add (obj as Coin);
        //            }
        //        else if (obj as Striker != null)
        //            {
        //            striker = obj as Striker;
        //            }
        //        else if (obj as Pocket != null)
        //            {
        //            listOfPockets.Add( obj as Striker);
        //            }
        //        }
        //    catch (Exception)
        //        {

        //        throw;
        //        }
            
        //    }

        //Will be used to move a CarromObject
        //Two scenarios 
        //  1. Apply force, direction and CarromObject will stop at perticular point
        //  2. Apply force, direction and CarromObject will collide with other CarromObjects (Coins or Striker or Pocket) or Edge

        public static void HitStriker (ref Striker obj, double force, double angle)
            {
            //int x = (int)Math.Round (obj.GetOrigin ().X + GetStoppingDistance (force) * Math.Cos (angle));
            //int y = (int)Math.Round (obj.GetOrigin ().Y + GetStoppingDistance (force) * Math.Sin (angle));
            //allPoints = GetAllPointsBetweenTwoPoints (obj.GetOrigin (), new Point (x, y));

            SetStrikerInput (force, angle);

            changeStrikerValues = new DispatcherTimer ();
            changeStrikerValues.Interval = TimeSpan.FromMilliseconds (10);
            changeStrikerValues.Tick += ChangeStrikerValues;
            changeStrikerValues.Start ();
            //change

            //foreach (var p in allPoints)
            //    {
            //if (!obj.GetOrigin ().Equals (new Point (x, y)))
            //    {
            //}
            //Thread.Sleep (100);
            //    }
            //while (obj.GetOrigin ().Equals (new Point (x, y)))
            //    {
            //    }
            //Move to perticular direction
            //Check if it collides with others
            //--------If yes, recalculate current objects direction and force
            //--------and find other collided objects and call HitCarromObject with its direction and force
            //--------If no, continue
            }

        public static void HitCoin (ref Coin obj, double force, double angle)
            {
            //int x = (int)Math.Round (obj.GetOrigin ().X + GetStoppingDistance (force) * Math.Cos (angle));
            //int y = (int)Math.Round (obj.GetOrigin ().Y + GetStoppingDistance (force) * Math.Sin (angle));
            //allPoints = GetAllPointsBetweenTwoPoints (obj.GetOrigin (), new Point (x, y));

            SetCoinInput (force, angle);

            changeCoinValues = new DispatcherTimer ();
            changeCoinValues.Interval = TimeSpan.FromMilliseconds (10);
            changeCoinValues.Tick += ChangeCoinValues;
            changeCoinValues.Start ();
            //change

            //foreach (var p in allPoints)
            //    {
            //if (!obj.GetOrigin ().Equals (new Point (x, y)))
            //    {
            //}
            //Thread.Sleep (100);
            //    }
            //while (obj.GetOrigin ().Equals (new Point (x, y)))
            //    {
            //    }
            //Move to perticular direction
            //Check if it collides with others
            //--------If yes, recalculate current objects direction and force
            //--------and find other collided objects and call HitCarromObject with its direction and force
            //--------If no, continue
            }

        public static void SetStrikerInput (double force, double angle)
            {
            strikerForce = force;
            strikerAngle = angle;
            Point strikerOrigin = Game.striker.GetOrigin ();

            int x = (int)Math.Round (strikerOrigin.X + GetStoppingDistance (force) * Math.Cos (angle));
            int y = (int)Math.Round (strikerOrigin.Y + GetStoppingDistance (force) * Math.Sin (angle));
            allPoints = GetAllPointsBetweenTwoPoints (strikerOrigin, new Point (x, y));
            }

        public static void SetCoinInput (double force, double angle)
            {
            coinForce = force;
            coinAngle = angle + 3.14159;
            Point coinOrigin = Game.queen.GetOrigin ();
            allPoints.Clear ();
            i = 0;
            int x = (int)Math.Round (coinOrigin.X + GetStoppingDistance (force) * Math.Cos (coinAngle));
            int y = (int)Math.Round (coinOrigin.Y + GetStoppingDistance (force) * Math.Sin (coinAngle));
            allPoints = GetAllPointsBetweenTwoPoints (coinOrigin, new Point (x, y));
            }

        static int i = 0;
        private static double coinForce;
        private static double coinAngle;

        private static void ChangeStrikerValues (object sender, EventArgs e)
            {
            try
                {
                CollisionResult collisionResult = isCollided (Game.striker);
                if (collisionResult == CollisionResult.None)
                    {
                    Game.striker.SetOrigin (allPoints.ElementAt (i++));
                    }
                else
                    {
                    changeStrikerValues.Stop ();

                    switch (collisionResult)
                        {
                        case CollisionResult.Queen:
                            Point strikerOrigin = Game.striker.GetOrigin ();
                            Point coinOrigin = Game.queen.GetOrigin ();
                            HitCoin (ref Game.queen, 50, 3.14159 - AngleBetweenTwoLines (strikerOrigin, coinOrigin, new Point (0, 740), new Point (740, 740)));


                            break;
                        }
                    }
                }
            catch (ArgumentOutOfRangeException)
                {
                changeStrikerValues.Stop ();
                }
            
            }

        private static void ChangeCoinValues (object sender, EventArgs e)
            {
            try
                {
                Game.queen.SetOrigin (allPoints.ElementAt (i++));
                CollisionResult collisionResult = isCollided (Game.queen);
                switch (collisionResult)
                    {
                    case CollisionResult.None:
                        Game.queen.SetOrigin (allPoints.ElementAt (i++));
                        break;
                    case CollisionResult.Pocket:
                        Game.queen.GetBaseElement ().Fill = new SolidColorBrush (Colors.White);

                        break;
                    }
                    
                    //}
                //else
                    //{
                  //  changeCoinValues.Stop ();
                    //Point strikerOrigin = Game.striker.GetOrigin ();
                    //Point coinOrigin = Game.queen.GetOrigin ();
                    //HitStriker (ref Game.queen, 50, AngleBetweenTwoLines (strikerOrigin, coinOrigin, new Point (0, 740), new Point (740, 740)));
                    //}
                }
            catch (ArgumentOutOfRangeException)
                {
                changeCoinValues.Stop ();
                }

            }

        public static Point GetPointFrom (Point p, double force, double angle, int timeInMillis)
            {
            Point result = new Point ();
            double distance = 0 * timeInMillis + (1 / 2 )*((-0.25 * 9.8) * (timeInMillis * timeInMillis));
            result.X = (int)Math.Round (p.X + distance * Math.Cos (angle));
            result.Y = (int)Math.Round (p.Y + distance * Math.Sin (angle));
            return result;
            }

        public enum CollisionResult
            {
            None,
            Edge,
            Coin,
            Striker,
            Queen,
            Pocket
            };

        public static CollisionResult isCollided (CarromObject obj)
            {
            //Check if is collided with Edge

            //Check if it is in pocket
            for (int i = 0; i < 4; i++)
                {
                var pocketDx = obj.GetOrigin ().X - Game.pockets[i].GetOrigin ().X;
                var pocketDy = obj.GetOrigin ().Y - Game.pockets[i].GetOrigin ().Y;

                var pocketDistance = Math.Sqrt (pocketDx * pocketDx + pocketDy * pocketDy);

                if (pocketDistance <  Game.pockets[0].Radius - 2)
                    {
                    obj.GetBaseElement ().Fill = new SolidColorBrush (Colors.Black);
                    changeCoinValues.Stop ();
                    return CollisionResult.Pocket;
                    }
                }
            

            //Check if it is collided with other Coin
            var dx = obj.GetOrigin().X - Game.queen.GetOrigin().X;
            var dy = obj.GetOrigin ().Y - Game.queen.GetOrigin ().Y;

            var distance = Math.Sqrt (dx * dx + dy * dy);

            if (distance < obj.Radius + Game.queen.Radius)
                {
                obj.GetBaseElement ().Fill = new SolidColorBrush(Colors.Red);
                return CollisionResult.Queen;
                }

            //Check if it is going inside Pocket
            return CollisionResult.None;
            }

        public static double GetStoppingDistance (double force)
            {
            double coefficientOfFriction = 0.25 * 9.8;
            double stoppingDistance = 0.0F;

            stoppingDistance = (force * force) / (2 * coefficientOfFriction);

            return stoppingDistance;
            }

        public static List<Point> GetAllPointsBetweenTwoPoints (Point one, Point two)
            {
            List<Point> allPoints = new List<Point> ();
            int w = (int) Math.Round (two.X - one.X);
            int h = (int)Math.Round (two.Y - one.Y);
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0)
                dx1 = -1;
            else if (w > 0)
                dx1 = 1;
            if (h < 0)
                dy1 = -1;
            else if (h > 0)
                dy1 = 1;
            if (w < 0)
                dx2 = -1;
            else if (w > 0)
                dx2 = 1;
            int longest = Math.Abs (w);
            int shortest = Math.Abs (h);
            if (!(longest > shortest))
                {
                longest = Math.Abs (h);
                shortest = Math.Abs (w);
                if (h < 0)
                    dy2 = -1;
                else if (h > 0)
                    dy2 = 1;
                dx2 = 0;
                }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
                {
                allPoints.Add (one);
                numerator += shortest;
                if (!(numerator < longest))
                    {
                    numerator -= longest;
                    one.X += dx1;
                    one.Y += dy1;
                    }
                else
                    {
                    one.X += dx2;
                    one.Y += dy2;
                    }
                }
            return allPoints;
            }

        public static double AngleBetweenTwoLines (Point a, Point b, Point c, Point d)
            {
            //double m1 = (b.Y - a.Y) / (b.X - a.X);
            //double m2 = (d.Y - c.Y) / (d.X - c.X);

            //double angle = Math.Atan (Math.Abs((m2-m1)/(1+(m2*m1))));


            //return angle;

            double theta1 = Math.Atan2 (b.Y - a.Y, b.X - a.X);
            double theta2 = Math.Atan2 (d.Y - c.Y, d.X - c.X);

            double diff = Math.Abs (theta1 - theta2);

            double angle = Math.Min (diff, Math.Abs (180 - diff));
            return angle;
            }
        }
    }
