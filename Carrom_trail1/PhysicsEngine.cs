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
        static DispatcherTimer timer;
        static Point pointOfIntersection = new Point ();

        public static void Start ()
            {
            timer = new DispatcherTimer ();
            timer.Interval = TimeSpan.FromMilliseconds (10);
            timer.Tick += UpdateUI;
            timer.Start ();
            }

        private static void UpdateUI (object sender, EventArgs e)
            {
            //Update striker
            Game.striker.Update ();

            //Update all coins
            for (int i = 0; i < 19; i++)
                {
                if (!Game.coins[i].IsPocketed)
                    {
                    Game.coins[i].Update ();
                    }
                }
            }

        //Will be used to move a CarromObject
        //Two scenarios 
        //  1. Apply force, direction and CarromObject will stop at perticular point
        //  2. Apply force, direction and CarromObject will collide with other CarromObjects (Coins or Striker or Pocket) or Edge
        public void HitStriker (double force, double strikerAngle)
            {
            Game.striker.currentVelocity = force /4;
            Game.striker.intialVelocity = Game.striker.currentVelocity;
            Game.striker.initialHitTime = 0;

            DispatcherTimer changeStrikerValues = new DispatcherTimer ();
            changeStrikerValues.Interval = TimeSpan.FromMilliseconds (10);
            changeStrikerValues.Tick += (sender, e) => {
            //Striker displacement code
                try
                    {
                    List<Coin> collidedCoins = new List<Coin> ();
                    List<CollisionResult> collisionResult = isCollided (Game.striker, ref collidedCoins);

                    //if collisionResult[0] is None
                    if (collisionResult[0] == CollisionResult.None)
                        {
                        if (Game.striker.currentVelocity <= 0.0F)
                            {
                            changeStrikerValues.Stop ();
                            }
                        else
                            {
                            //Game.striker.SetOrigin (GetPointFrom (Game.striker.GetOrigin (), strikerAngle, Game.striker.initialHitTime));
                            Game.striker.Move (strikerAngle, 0);
                            }
                        }
                    //If collisionResult contains other than None
                    else
                        {
                        Point initialStrikerPoint = Game.striker.GetInitialPoint ();
                        Point strikerOrigin = Game.striker.GetOrigin ();
                        Point coinOrigin;
                        double coinAngle = 0.0F;
                        foreach (var collisionObject in collisionResult)
                            {
                            switch (collisionObject)
                                {
                                case CollisionResult.Queen:
                                    coinOrigin = Game.coins[18].GetOrigin ();

                                    //Move coin to new direction so that collide will not detect again
                                    //Game.coins[18].SetOrigin (GetPointFrom (Game.striker.GetOrigin (), strikerAngle, Game.striker.initialHitTime));
                                    // temporary direction to coin
                                    //Game.coins[18].Move (1.5708 + strikerAngle, 0);

                                    //Find angle between line through both origins and bottom line
                                    HitCoin (ref Game.coins[18], Game.striker.currentVelocity * 0.8 , 3.14159 - AngleBetweenTwoLines (strikerOrigin, coinOrigin, new Point (0, 740), new Point (740, 740)));
                                    break;

                                case CollisionResult.Coin:
                                    foreach (var collidedCoin in collidedCoins)
                                        {
                                        coinOrigin = collidedCoin.GetOrigin ();
                                        Game.striker.initialHitTime = 0;
                                        Game.coins[collidedCoin.CoinNumber - 1].initialHitTime = 0;
                                        coinAngle = AngleBetweenTwoLines (Game.striker.GetInitialPoint (), Game.striker.GetOrigin (), Game.striker.GetOrigin (), Game.coins[collidedCoin.CoinNumber - 1].GetOrigin ());
                                        //Move coin to new direction so that collide will not detect again
                                        //Game.coins[collidedCoin.CoinNumber].SetOrigin (GetPointFrom (Game.coins[collidedCoin.CoinNumber].GetOrigin (), coinAngle, Game.coins[collidedCoin.CoinNumber].initialHitTime));

                                        //Deviate striker also
                                        //Game.striker.SetOrigin (Game.striker.GetInitialPoint ());

                                        //Find angle between line through both origins and bottom line
                                        Game.coins[collidedCoin.CoinNumber - 1].currentVelocity = Game.striker.currentVelocity * 0.4;
                                        HitCoin (ref Game.coins[collidedCoin.CoinNumber - 1], Game.striker.currentVelocity * 0.4, coinAngle);
                                        }
                                    //Stop this timer
                                    changeStrikerValues.Stop ();
                                    break;

                                case CollisionResult.Edge:

                                    break;
                                }
                            }
                        }
                    }
                catch (ArgumentOutOfRangeException)
                    {
                    changeStrikerValues.Stop ();
                    }
                //end
            };
            changeStrikerValues.Start ();
            }

        public void HitCoin (ref Coin obj, double velocity, double angle)
            {
            int coinNumber = obj.CoinNumber;
            obj.currentVelocity = velocity;
            obj.intialVelocity = obj.currentVelocity;
            obj.initialHitTime = 0;

            DispatcherTimer changeCoinValues = new DispatcherTimer ();
            changeCoinValues.Interval = TimeSpan.FromMilliseconds (10);
            changeCoinValues.Tick += (sender, e) =>
            {
                try
                    {
                    Game.coins[coinNumber - 1].Move (angle, 0);
                    List<Coin> collidedCoins = new List<Coin> ();
                    List<CollisionResult> collisionResult = isCollided (Game.coins[coinNumber - 1], ref collidedCoins);
                    foreach (var collisionObject in collisionResult)
                        {
                        switch (collisionObject)
                            {
                            case CollisionResult.None:
                                if (Game.coins[coinNumber - 1].currentVelocity <= 0.0F)
                                    {
                                    changeCoinValues.Stop ();
                                    }
                                else
                                    {
                                    Game.coins[coinNumber - 1].Move (angle, 0);
                                    }
                                break;

                            case CollisionResult.Pocket:
                                changeCoinValues.Stop ();
                                Game.coins[coinNumber - 1].GetBaseElement ().Fill = new SolidColorBrush (Colors.White);
                                break;

                            case CollisionResult.Edge:
                                changeCoinValues.Stop ();
                                Game.coins[coinNumber - 1].GetBaseElement ().Fill = new SolidColorBrush (Colors.Blue);
                                break;

                            //What if Queen hits one or more coins
                            case CollisionResult.Coin:
                                changeCoinValues.Stop ();
                                for (int i = 0; i < collidedCoins.Count; i++)
                                    {
                                    //recalculate angle and velocity
                                    Game.coins[collidedCoins[i].CoinNumber -1].GetBaseElement ().Fill = new SolidColorBrush (Colors.Red);
                                    HitCoin (ref Game.coins[collidedCoins[i].CoinNumber - 1], Game.coins[coinNumber - 1].currentVelocity * 0.8, 4.71239);
                                    }
                                break;
                            }
                        }
                    }
                catch (ArgumentOutOfRangeException)
                    {
                    changeCoinValues.Stop ();
                    }
            };
            changeCoinValues.Start ();
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

        public static List<CollisionResult> isCollided (CarromObject obj, ref List<Coin> collidedCoins)
            {
            List<CollisionResult> result = new List<CollisionResult> ();

            //Check if pocket is in pocket
            #region Collision with Pockets
            for (int i = 0; i < 4; i++)
                {
                var pocketDx = obj.GetOrigin ().X - Game.pockets[i].GetOrigin ().X;
                var pocketDy = obj.GetOrigin ().Y - Game.pockets[i].GetOrigin ().Y;

                var pocketDistance = Math.Sqrt (pocketDx * pocketDx + pocketDy * pocketDy);

                //This condition will pass if provided CarromObject is pocketed
                if (pocketDistance < Game.pockets[0].Radius - 2)
                    {
                    obj.GetBaseElement ().Fill = new SolidColorBrush (Colors.Black);
                    //changeCoinValues.Stop ();
                    result.Add(CollisionResult.Pocket);
                    break;
                    }
                }
            #endregion

            //Check if is collided with Edge
            #region Collision with Edge
            
            #endregion

            //Check if it is collided with coins and queen
            #region Collision with Coins and Queen

            double dx, dy, distance;
            for (int i = 0; i < 19; i++)
                {
                if (obj is Coin && (obj as Coin).CoinNumber == Game.coins[i].CoinNumber)
                    {
                    continue;
                    }
                    dx = obj.GetOrigin ().X - Game.coins[i].GetOrigin ().X;
                    dy = obj.GetOrigin ().Y - Game.coins[i].GetOrigin ().Y;

                    distance = Math.Sqrt (dx * dx + dy * dy);

                    if (distance < obj.Radius + Game.coins[i].Radius)
                        {
                        obj.GetBaseElement ().Fill = new SolidColorBrush (Colors.Red);
                        if (Game.coins[i].IsQueen)
                            {
                            result.Add (CollisionResult.Queen);
                            }
                        else
                            {
                            result.Add (CollisionResult.Coin);
                            }
                        collidedCoins.Add (Game.coins[i]);
                    }
                }
            #endregion

            #region No Collision
            if (result.Count == 0)
                {
                result.Add (CollisionResult.None);
                }
            #endregion

            return result;
            }

        //public static double GetStoppingDistance (double force)
        //    {
        //    double coefficientOfFriction = 0.25 * 9.8;
        //    double stoppingDistance = 0.0F;

        //    stoppingDistance = (force * force) / (2 * coefficientOfFriction);

        //    return stoppingDistance;
        //    }

        //public static List<Point> GetAllPointsBetweenTwoPoints (Point one, Point two)
        //    {
        //    List<Point> allPoints = new List<Point> ();
        //    int w = (int) Math.Round (two.X - one.X);
        //    int h = (int)Math.Round (two.Y - one.Y);
        //    int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        //    if (w < 0)
        //        dx1 = -1;
        //    else if (w > 0)
        //        dx1 = 1;
        //    if (h < 0)
        //        dy1 = -1;
        //    else if (h > 0)
        //        dy1 = 1;
        //    if (w < 0)
        //        dx2 = -1;
        //    else if (w > 0)
        //        dx2 = 1;
        //    int longest = Math.Abs (w);
        //    int shortest = Math.Abs (h);
        //    if (!(longest > shortest))
        //        {
        //        longest = Math.Abs (h);
        //        shortest = Math.Abs (w);
        //        if (h < 0)
        //            dy2 = -1;
        //        else if (h > 0)
        //            dy2 = 1;
        //        dx2 = 0;
        //        }
        //    int numerator = longest >> 1;
        //    for (int i = 0; i <= longest; i++)
        //        {
        //        allPoints.Add (one);
        //        numerator += shortest;
        //        if (!(numerator < longest))
        //            {
        //            numerator -= longest;
        //            one.X += dx1;
        //            one.Y += dy1;
        //            }
        //        else
        //            {
        //            one.X += dx2;
        //            one.Y += dy2;
        //            }
        //        }
        //    return allPoints;
        //    }

        public static double AngleBetweenTwoLines (Point a, Point b, Point c, Point d)
            {
            double theta1 = Math.Atan2 (b.Y - a.Y, b.X - a.X);
            double theta2 = Math.Atan2 (d.Y - c.Y, d.X - c.X);

            double diff = Math.Abs (theta1 - theta2);
            double angle = Math.Min (diff, Math.Abs (180 - diff));

            return angle;
            }
        }
    }
