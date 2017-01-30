using System;
using System.Collections.Generic;
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
                    Game.striker.Move (strikerAngle, 0);
                    Game.striker.Move (strikerAngle, 0);
                    Game.striker.Move (strikerAngle, 0);

                    List<Coin> collidedCoins = new List<Coin> ();
                    List<CollisionResult> collisionResult = isCollided (Game.striker, ref collidedCoins);
               //     if collisionResult[0] is None
                    if (collisionResult[0] == CollisionResult.None)
                            {
                            if (Game.striker.currentVelocity <= 0.001)
                                {
                                changeStrikerValues.Stop ();
                                Game.striker.SetOrigin (new Point (370, 633));
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
                            //case CollisionResult.None:
                            //    if (Game.striker.currentVelocity <= 0.0F)
                            //        {
                            //        changeStrikerValues.Stop ();
                            //        }
                            //    else
                            //        {
                            //        //Game.striker.SetOrigin (GetPointFrom (Game.striker.GetOrigin (), strikerAngle, Game.striker.initialHitTime));
                            //        Game.striker.Move (strikerAngle, 0);
                            //        }
                            //    break;
                                case CollisionResult.Queen:
                                    coinOrigin = Game.coins[18].GetOrigin ();

                                    //Move coin to new direction so that collide will not detect again
                                    //Game.coins[18].SetOrigin (GetPointFrom (Game.striker.GetOrigin (), strikerAngle, Game.striker.initialHitTime));
                                    // temporary direction to coin
                                    //Game.coins[18].Move (1.5708 + strikerAngle, 0);

                                    //Find angle between line through both origins and bottom line
                                    double angleBetweenQueenAndStriker = AngleBetweenTwoLines (strikerOrigin, coinOrigin, new Point (0, 740), new Point (740, 740));
                                    if (Game.striker.GetOrigin ().X > Game.coins[18].GetOrigin ().X)
                                        {
                                        HitCoin (ref Game.coins[18], Game.striker.currentVelocity, 1.5708 + angleBetweenQueenAndStriker);
                                        HitStriker ((Game.striker.currentVelocity) * 2, (6.28319 + strikerAngle) / 2);
                                        }
                                    else if (Game.striker.GetOrigin ().X < Game.coins[18].GetOrigin ().X)
                                        {
                                        HitCoin (ref Game.coins[18], Game.striker.currentVelocity, -1.5708 + angleBetweenQueenAndStriker);
                                        HitStriker ((Game.striker.currentVelocity) * 2, (3.14159 + strikerAngle) / 2);
                                        }
                                    //Stop this timer
                                    changeStrikerValues.Stop ();
                                    break;

                                case CollisionResult.Coin:
                                    foreach (var collidedCoin in collidedCoins)
                                        {
                                        coinOrigin = Game.coins[collidedCoin.CoinNumber - 1].GetOrigin ();

                                        //Find angle between line through both origins and bottom line
                                        double angleBetweenCoinAndStriker = AngleBetweenTwoLines (strikerOrigin, coinOrigin, new Point (0, 740), new Point (740, 740));

                                        if (Game.striker.GetOrigin ().X > Game.coins[collidedCoin.CoinNumber - 1].GetOrigin ().X)
                                            {
                                            HitCoin (ref Game.coins[collidedCoin.CoinNumber - 1], Game.striker.currentVelocity * 0.75, 1.5708 + angleBetweenCoinAndStriker);
                                            HitStriker ((Game.striker.currentVelocity) * 3, (6.28319 + strikerAngle) /2);
                                            }
                                        else if (Game.striker.GetOrigin ().X < Game.coins[collidedCoin.CoinNumber - 1].GetOrigin ().X)
                                            {
                                            HitCoin (ref Game.coins[collidedCoin.CoinNumber - 1], Game.striker.currentVelocity * 0.75, -1.5708 + angleBetweenCoinAndStriker);
                                            HitStriker ((Game.striker.currentVelocity) * 3, (3.14159 + strikerAngle) /2);
                                            }
                                        }
                                    //Stop this timer
                                    changeStrikerValues.Stop ();
                                    break;

                                case CollisionResult.RightEdge:
                                changeStrikerValues.Stop ();
                                Game.striker.GetBaseElement ().Fill = new SolidColorBrush (Colors.Blue);
                                HitStriker ((Game.striker.currentVelocity * 10), AngleBetweenTwoLines (Game.striker.GetInitialPoint (), pointOfIntersection, new Point(740,740), new Point(740,0)) + 3.14159);
                                Game.striker.SetInitialPoint (Game.striker.GetOrigin ());
                                break;
                                case CollisionResult.BottomEdge:
                                    changeStrikerValues.Stop ();
                                    Game.striker.GetBaseElement ().Fill = new SolidColorBrush (Colors.Blue);
                                    HitStriker ((Game.striker.currentVelocity * 10), AngleBetweenTwoLines (Game.striker.GetInitialPoint (), pointOfIntersection, new Point (0, 740), new Point (740, 740)) + 1.5708);
                                    Game.striker.SetInitialPoint (Game.striker.GetOrigin ());
                                    break;
                                case CollisionResult.TopEdge:
                                    changeStrikerValues.Stop ();
                                    Game.striker.GetBaseElement ().Fill = new SolidColorBrush (Colors.Blue);
                                    HitStriker ((Game.striker.currentVelocity * 10), AngleBetweenTwoLines (Game.striker.GetInitialPoint (), pointOfIntersection, new Point (0, 0), new Point (740, 0)) - 6.28319);
                                    Game.striker.SetInitialPoint (Game.striker.GetOrigin ());
                                    break;
                                case CollisionResult.LeftEdge:
                                    changeStrikerValues.Stop ();
                                    Game.striker.GetBaseElement ().Fill = new SolidColorBrush (Colors.Blue);
                                    HitStriker ((Game.striker.currentVelocity * 10), AngleBetweenTwoLines (Game.striker.GetInitialPoint (), pointOfIntersection, new Point (0, 740), new Point (0, 0)) - 1.5708);
                                    Game.striker.SetInitialPoint (Game.striker.GetOrigin ());
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

                            case CollisionResult.LeftEdge:
                                changeCoinValues.Stop ();
                                Game.coins[coinNumber - 1].GetBaseElement ().Fill = new SolidColorBrush (Colors.Blue);
                                break;

                            //What if Queen hits one or more coins
                            case CollisionResult.Coin:
                                changeCoinValues.Stop ();
                                for (int i = 0; i < collidedCoins.Count; i++)
                                    {
                                    //recalculate angle and velocity
                                    double angleBetweenCoins = AngleBetweenTwoLines (Game.coins[coinNumber - 1].GetOrigin (), Game.coins[collidedCoins[i].CoinNumber - 1].GetOrigin (), new Point (0, 740), new Point (740, 740));
                                    HitCoin (ref Game.coins[collidedCoins[i].CoinNumber - 1], Game.coins[coinNumber - 1].currentVelocity , angleBetweenCoins);
                                    HitCoin (ref Game.coins[coinNumber - 1], Game.coins[coinNumber - 1].currentVelocity, (angle + angleBetweenCoins) / 2);
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
            RightEdge,
            LeftEdge,
            BottomEdge,
            TopEdge,
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
            Point[] edgePoints = new Point[5];
            edgePoints[0] = new Point (0, 0);
            edgePoints[1] = new Point (740, 0);
            edgePoints[2] = new Point (740, 740);
            edgePoints[3] = new Point (0, 740);
            edgePoints[4] = new Point (0, 0);
            for (var i = 0; i < 4; i++)
                {
                var edgePointx1 = edgePoints[i].X;
                var edgePointy1 = edgePoints[i].Y;
                var edgePointx2 = edgePoints[i + 1].X;
                var edgePointy2 = edgePoints[i + 1].Y;
                Point CarromObjectOrigin = obj.GetOrigin ();
                var xCircle = CarromObjectOrigin.X;
                var yCircle = CarromObjectOrigin.Y;
                var radius = obj.Radius;
                double dx1, dy1, A, B, C, det, t;

                dx1 = edgePointx2 - edgePointx1;
                dy1 = edgePointy2 - edgePointy1;

                A = dx1 * dx1 + dy1 * dy1;
                B = 2 * (dx1 * (edgePointx1 - xCircle) + dy1 * (edgePointy1 - yCircle));
                C = (edgePointx1 - xCircle) * (edgePointx1 - xCircle) + (edgePointy1 - yCircle) * (edgePointy1 - yCircle) - radius * radius;

                det = B * B - 4 * A * C;
                if (det >= 0)
                    {
                    if (i == 0)
                        {
                        result.Add (CollisionResult.TopEdge);
                        t = -B / (2 * A);
                        pointOfIntersection = new Point (edgePointx1 + t * dx1, edgePointy1 + t * dy1);
                        break;
                        }
                    else if (i == 1)
                        {
                        result.Add (CollisionResult.RightEdge);
                        t = -B / (2 * A);
                        pointOfIntersection = new Point (edgePointx1 + t * dx1, edgePointy1 + t * dy1);
                        break;
                        }

                    else if (i == 2)
                        {
                        result.Add (CollisionResult.BottomEdge);
                        t = -B / (2 * A);
                        pointOfIntersection = new Point (edgePointx1 + t * dx1, edgePointy1 + t * dy1);
                        break;
                        }

                    else
                        {
                        result.Add (CollisionResult.LeftEdge);
                        t = -B / (2 * A);
                        pointOfIntersection = new Point (edgePointx1 + t * dx1, edgePointy1 + t * dy1);
                        break;
                        }
                    }
                }

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
                    //obj.GetBaseElement ().Fill = new SolidColorBrush (Colors.Red);
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
